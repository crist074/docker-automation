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
        const string selenoidUrl = "http://localhost:4444/wd/hub";
        const string localBinary = @".";
        string currentDirectory = $"{Environment.CurrentDirectory}\\..\\..\\..";

        public WebDriver(){
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");
            /*var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
            capabilities.SetCapability(CapabilityType.BrowserVersion, "90.0");
            capabilities.SetCapability("enableVNC", true);
            driver = new RemoteWebDriver(new Uri(selenoidUrl), capabilities);*/
            //driver = new ChromeDriver(currentDirectory, options);
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
        }
    }
}