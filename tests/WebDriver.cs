using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

[assembly:LevelOfParallelism(100)]

namespace Automation{
    public class WebDriver {

        public IWebDriver driver;
        public string currentDirectory = $"{Environment.CurrentDirectory}\\..\\..\\..";

        public WebDriver(){
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
        }
    }
}