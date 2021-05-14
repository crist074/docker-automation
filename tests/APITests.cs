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
    public class APITests
    {
        [ThreadStatic] private RestClient client;
        [ThreadStatic] private RestRequest request;
        [ThreadStatic] private static ExtentTest test;
        private ExtentReports extent;
        [ThreadStatic] private bool testPassed;
        static string currentDirectory = @$"{Environment.CurrentDirectory}/../../../";

        [OneTimeSetUp]
        public void OneTimeSetUp(){
            extent = new ExtentReports();
        }

        [SetUp]
        public void Setup(){
            var html = new ExtentV3HtmlReporter(@$"{currentDirectory}../Report/api.html");
            extent.AttachReporter(html);
            testPassed = false;
            test = extent.CreateTest($"{NUnit.Framework.TestContext.CurrentContext.Test.Name}");
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(nameof(GetTestData))]
        public void Test1(APITestCaseSource testCase){
            client = new RestClient("http://api.zippopotam.us");
            request = new RestRequest($"{testCase.code}/{testCase.zip}", Method.GET);
            var response = client.Execute(request);
            Assert.AreEqual((int)response.StatusCode, testCase.statusCode);

            testPassed = true;
        }

        [Test]
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

        [Test]
        public void GetRandomAPI(){
            client = new RestClient("https://api.publicapis.org/");
            request = new RestRequest("/random", Method.GET);
            request.AddQueryParameter("auth", "null");
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            testPassed = true;
        }

        [Test]
        public void GetHealth(){
            client = new RestClient("https://api.publicapis.org/");
            request = new RestRequest("/health", Method.GET);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            testPassed = true;
        }

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
            public int zip {get;set;} 
            public int statusCode {get;set;}
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