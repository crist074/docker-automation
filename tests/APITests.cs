using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Globalization;
using System.IO;
using RestSharp;
using System.Data;
using System.Collections.Generic;
using CsvHelper;
using Newtonsoft.Json;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace Automation
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class APITests
    {
        [ThreadStatic] private static RestClient client;
        [ThreadStatic] private static RestRequest request;
        [ThreadStatic] private static ExtentTest test;
        private ExtentReports extent;
        [ThreadStatic] private static bool testPassed;
        static string currentDirectory = @$"{Environment.CurrentDirectory}/../../../";

        /// <summary>
        /// Instantiate ExtentReport
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp(){
            extent = new ExtentReports();
        }

        /// <summary>
        /// Setup ExtentReports
        /// Add test to ExtentReport reporter
        /// </summary>
        [SetUp]
        public void Setup(){
            var html = new ExtentV3HtmlReporter(@$"{currentDirectory}../Report/api.html");
            extent.AttachReporter(html);
            testPassed = false;
            test = extent.CreateTest($"{NUnit.Framework.TestContext.CurrentContext.Test.Name}");
        }

        /// <summary>
        /// Parses "apitestcasesources.csv" for test cases
        /// Verifies country code / postal code are valid
        /// </summary>
        /// <param name="testCase">APITestCaseSource object representing country code, zip code, and expected response code</param>
        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(nameof(GetTestData))]
        public void ZipCodeValidator(APITestCaseSource testCase){
            test.Info($"Code:{testCase.code}, Zip:{testCase.zip}, Status:{testCase.statusCode}");
            client = new RestClient("http://api.zippopotam.us");
            request = new RestRequest($"{testCase.code}/{testCase.zip}", Method.GET);
            var response = client.Execute(request);
            Assert.AreEqual((int)response.StatusCode, int.Parse(testCase.statusCode));

            testPassed = true;
        }

        /// <summary>
        /// Test case with Json Deserialization
        /// Read response and parse object
        /// </summary>
        [Test]
        [Parallelizable]
        public void JsonDeserialization(){
            client = new RestClient("https://api.publicapis.org/");
            request = new RestRequest("/entries", Method.GET);
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);
            List<Entry> entryList = new List<Entry>();
            foreach (var e in myDeserializedClass.entries){
                entryList.Add(e);
                Console.WriteLine(e.Link);
            }

            testPassed = true;
        }

        /// <summary>
        /// Gets a random api
        /// Verify 200 status code
        /// </summary>
        [Test]
        [Parallelizable]
        public void GetRandomAPI(){
            client = new RestClient("https://api.publicapis.org/");
            request = new RestRequest("/random", Method.GET);
            request.AddQueryParameter("auth", "null");
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            testPassed = true;
        }

        /// <summary>
        /// Get health of random api
        /// Verify 200 status code
        /// Verify api health returns as alive=true
        /// </summary>
        [Test]
        [Parallelizable]
        public void GetHealth(){
            client = new RestClient("https://api.publicapis.org/");
            request = new RestRequest("/health", Method.GET);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            Assert.AreEqual(dict["alive"], "true");

            testPassed = true;
        }

        /// <summary>
        /// Get daily random cat fact
        /// Verify 200 status code
        /// </summary>
        [Test]
        [Parallelizable]
        public void CatFact(){
            client = new RestClient("https://cat-fact.herokuapp.com/");
            request = new RestRequest("/facts", Method.GET);
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);

            testPassed = true;
        }


        /// <summary>
        /// POST api test
        /// **Resource not actually updated on server, but is faked
        /// </summary>
        [Test]
        [Parallelizable]
        public void POST(){
            client = new RestClient("https://jsonplaceholder.typicode.com/");
            request = new RestRequest("/posts", Method.POST);
            request.AddJsonBody(new { title = "foo", body = "bar", userId = 1 });
            request.AddHeader("Content-type", "application/json; charset=UTF-8");
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Console.WriteLine(response.Content);

            testPassed = true;
        }   


        /// <summary>
        /// DELETE api test
        /// **Resource not actually updated on server, but is faked
        /// </summary>
        [Test]
        [Parallelizable]
        public void DELETE(){
            client = new RestClient("https://jsonplaceholder.typicode.com/");
            request = new RestRequest("/posts/1", Method.DELETE);
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);

            testPassed = true;
        }   


        [TearDown]
        public void TearDown()
        {
            if(!testPassed){
                test.Fail("Failed");
            }
            else{
                test.Pass("Passed");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown(){
            extent.Flush();
        }
    
        public static IEnumerable<APITestCaseSource> GetTestData(){
            List<APITestCaseSource> sources = new List<APITestCaseSource>();
            using (var reader = new StreamReader(@$"{currentDirectory}apitestcasesources.csv")){
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)){
                    var records = csv.GetRecords<APITestCaseSource>();
                    foreach (APITestCaseSource record in records){
                        sources.Add(record);
                    }
                }   
            }
            return sources;
        }

        public class APITestCaseSource {
            public string code {get;set;}
            public string zip {get;set;} 
            public string statusCode {get;set;}
        }

        public class Entry
        {
            public string API { get; set; }
            public string Description { get; set; }
            public string Auth { get; set; }
            public bool HTTPS { get; set; }
            public string Cors { get; set; }
            public string Link { get; set; }
            public string Category { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Entry> entries { get; set; }
        }
    }  
}