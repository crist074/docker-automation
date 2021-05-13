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
        private static ExtentReports extent;
        [ThreadStatic] private static IWebDriver driver;
        [ThreadStatic] private bool testPassed;

        [OneTimeSetUp]
        public void OneTimeSetUp(){
            
        }

        [SetUp]
        public void Setup(){
            var html = new ExtentV3HtmlReporter(@"E:\Github\docker-automation\Report\ui.html");
            extent = new ExtentReports();
            extent.AttachReporter(html);
            driver = new WebDriver().driver;
            testPassed = false;
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        public void Test1([Range(0, 10)] int x){

            test = extent.CreateTest($"{x}").Info("testing");

            //WebDriver driverSetup = new WebDriver();
            //IWebDriver driver = driverSetup.driver;
            driver.Navigate().GoToUrl("http://google.com");
            //driver.Quit();

            test.Log(Status.Pass, "Test pass");
            testPassed = true;
        }

        [Test]
        [Parallelizable]
        public void Test2(){
        
            test = extent.CreateTest("Large Dom").Info("testing");

            //WebDriver driverSetup = new WebDriver();
            //IWebDriver driver = driverSetup.driver;
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/");
            driver.FindElement(By.XPath("//a[text()='Large & Deep DOM']")).Click();
            string s = driver.FindElement(By.Id("sibling-43.1")).Text.Split('\r', StringSplitOptions.TrimEntries)[0].Split('\n')[0];
            Assert.AreEqual(s, "43.1");
            //driver.Quit();

            //test.Log(Status.Pass, "Test pass");
            testPassed = true;
            
        }

        [Test]
        [Parallelizable]
        public void Test3(){

            test = extent.CreateTest("psychology today").Info("testing");


            //WebDriver driverSetup = new WebDriver();
            //IWebDriver driver = driverSetup.driver;
            driver.Navigate().GoToUrl("https://www.psychologytoday.com/");
            driver.FindElement(By.XPath("//div[contains(@class, 'menu large')]")).Click();
            driver.FindElement(By.XPath("//ul[@class='active']//li[contains(text(), 'Psychiatrists')]")).Click();
            driver.FindElement(By.XPath("//input[contains(@placeholder, 'City or Zip') and contains(@class, 'large')]")).SendKeys("80601");
            driver.FindElement(By.XPath("//input[contains(@placeholder, 'City or Zip') and contains(@class, 'large')]")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//a[contains(text(), 'Cigna')]")).Click();
            while(driver.FindElements(By.XPath("//a[contains(@class, 'btn-default btn-next')]")).Count != 0){
                var rows = driver.FindElements(By.XPath("//div[contains(@class, 'result-row')]"));
                int numberOfRows = rows.Count;
                for(int i = 0; i < numberOfRows; i++){
                    driver.FindElements(By.XPath("//div[contains(@class, 'result-row')]"))[i].Click();
                    Assert.True(driver.FindElement(By.XPath("//li[contains(text(), 'Cigna')]")).Displayed);
                    driver.Navigate().Back();
                }
                driver.FindElement(By.XPath("//a[contains(@class, 'btn-default btn-next')]")).Click();
            }
            //driver.Quit();

            test.Log(Status.Pass, "Test pass");
            testPassed = true;
        }

        [TearDown]
        public void TearDown(){
            driver.Quit();
            if(!testPassed){
                test.Fail("Failed");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown(){
            extent.Flush();
        }
    }
}