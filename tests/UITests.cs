using NUnit.Framework;
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace Automation
{
    public class UITests
    {
        [ThreadStatic] private static ExtentTest test;
        private ExtentReports extent;
        [ThreadStatic] private static IWebDriver driver;
        [ThreadStatic] private bool testPassed;

        [OneTimeSetUp]
        public void OneTimeSetUp(){
            extent = new ExtentReports();
        }

        [SetUp]
        public void Setup(){
            var html = new ExtentV3HtmlReporter(@"E:\Github\docker-automation\Report\ui.html");
            extent.AttachReporter(html);
            driver = new WebDriver().driver;
            testPassed = false;
            test = extent.CreateTest($"{NUnit.Framework.TestContext.CurrentContext.Test.Name}");
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        public void ParameterizedTest([Range(0, 9)] int x){

            driver.Navigate().GoToUrl("http://google.com");

            testPassed = true;
        }

        [Test]
        [Parallelizable]
        public void LargeDOM(){
        
            Utility u = new Utility();
            string target = u.LoadJsonConfig("uitestconfig.json", "largeDOMwebsite");
            driver.Navigate().GoToUrl(target);
            driver.FindElement(By.XPath("//a[text()='Large & Deep DOM']")).Click();
            string s = driver.FindElement(By.Id("sibling-43.1")).Text.Split('\r', StringSplitOptions.TrimEntries)[0].Split('\n')[0];
            Assert.AreEqual(s, "43.1");

            testPassed = true;
            
        }

        [Test]
        [Parallelizable]
        public void VerifyInsuranceFilter(string nameOfInsurance = "Cigna"){

            driver.Navigate().GoToUrl("https://www.psychologytoday.com/");
            driver.FindElement(By.XPath("//div[contains(@class, 'menu large')]")).Click();
            driver.FindElement(By.XPath("//ul[@class='active']//li[contains(text(), 'Psychiatrists')]")).Click();
            driver.FindElement(By.XPath("//input[contains(@placeholder, 'City or Zip') and contains(@class, 'large')]")).SendKeys("80601");
            driver.FindElement(By.XPath("//input[contains(@placeholder, 'City or Zip') and contains(@class, 'large')]")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath($"//a[contains(text(), '{nameOfInsurance}')]")).Click();
            while(driver.FindElements(By.XPath("//a[contains(@class, 'btn-default btn-next')]")).Count != 0){
                var rows = driver.FindElements(By.XPath("//div[contains(@class, 'result-row')]"));
                int numberOfRows = rows.Count;
                for(int i = 0; i < numberOfRows; i++){
                    driver.FindElements(By.XPath("//div[contains(@class, 'result-row')]"))[i].Click();
                    Assert.True(driver.FindElement(By.XPath($"//li[contains(text(), '{nameOfInsurance}')]")).Displayed);
                    driver.Navigate().Back();
                }
                driver.FindElement(By.XPath("//a[contains(@class, 'btn-default btn-next')]")).Click();
            }

            testPassed = true;
        }

        [TearDown]
        public void TearDown(){
            driver.Quit();
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
    }
}