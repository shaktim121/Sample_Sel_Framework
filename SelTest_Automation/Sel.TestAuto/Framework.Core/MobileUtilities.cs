using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sel.TestAuto.TestScripts.SmokeTests
{
    class MobileUtilities
    { 
        private static Uri testServerAddress = new Uri("http:127.0.01:4723/wd/hub");
        private static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);
        private static TimeSpan IMPLICT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);
        private AppiumDriver<AndroidElement> driver;

        public void nativeApp()
        {
            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Galaxy S9");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, "tb://...");
            driver = new AndroidDriver<AndroidElement>(new Uri("https://hub.testinbot.com/wd/hub"),appiumOptions);
        }

        public void mobileBrowsersNavigate(String url)
        {
          //  DesiredCapabilities desiredCapabilities = new DesiredCapabilities();
            var caps = new ChromeOptions();
            caps.PlatformName = "Android";
            caps.BrowserVersion = "9.0";
            RemoteWebDriver remoteWebDriver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps);
            remoteWebDriver.Navigate().GoToUrl(url);
        }

        public void mobileBrowsersClick(AndroidElement androidElement)
        {
            var caps = new ChromeOptions();
            caps.PlatformName = "Android";
            caps.BrowserVersion = "9.0";
            RemoteWebDriver remoteWebDriver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps);
            androidElement.Click();
        }

        public void mobileBrowsersSendData(AndroidElement androidElement , String val)
        {
            var caps = new ChromeOptions();
            caps.PlatformName = "Android";
            caps.BrowserVersion = "9.0";
            RemoteWebDriver remoteWebDriver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps);
            androidElement.SendKeys(val);
        }
    }

}
