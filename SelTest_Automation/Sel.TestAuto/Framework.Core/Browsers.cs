﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Microsoft.Edge.SeleniumTools;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Sel.TestAuto
{
    public static class Browsers
    {
        //private static readonly string baseURL = ConfigurationManager.AppSettings["url"];
        //private static readonly string browser = ConfigurationManager.AppSettings["browser"];
        static string projectFolderPath = Environment.CurrentDirectory;
        public static void Init(string browser, string baseURL)
        {
            AutomationCore.Report.Info("Application : " + baseURL + " and Browser : " + browser);
            switch (browser.ToLower())
            {
                case "chrome":
                    Utilities.Kill_Process("chromedriver");
                    //string chromeVersion = new ChromeConfig().GetMatchingBrowserVersion();
                    //new DriverManager().SetUpDriver(new ChromeConfig(),chromeVersion);
                    GetDriver = new ChromeDriver();
                        
                    break;

                case "chromebeta":
                    Utilities.Kill_Process("chromedriver");
                    ChromeOptions co = new ChromeOptions()
                    {
                        BinaryLocation = @"C:\Program Files\Google\Chrome Beta\Application\chrome.exe"
                    };
                    co.AddArgument("no-sandbox");
                    //GetDriver = new ChromeDriver(co);
                    GetDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(),co,TimeSpan.FromSeconds(120));
                    break;

                case "ie":
                    //_driver = new InternetExplorerDriver();
                    break;

                case "firefox":
                    Utilities.Kill_Process("geckodriver");
                    GetDriver = new FirefoxDriver();
                    break;

                case "edge":
                    string edgeDriverPath = projectFolderPath.Substring(0, projectFolderPath.LastIndexOf("bin"))+ "Framework.Core\\";
                    EdgeOptions options = new EdgeOptions()
                    {
                        UseChromium = true
                    };
                    GetDriver = new EdgeDriver(edgeDriverPath, options);
                    //GetDriver = new EdgeDriver(@"C:\Users\sahus\Downloads\edgedriver_win64");
                    break;
            }

            Goto(baseURL);
        }
        public static string Title
        {
            get { return GetDriver.Title; }
        }
        public static IWebDriver GetDriver { get; private set; }
        public static void Goto(string url)
        {
            GetDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            GetDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            GetDriver.Manage().Cookies.DeleteAllCookies();
            GetDriver.Manage().Window.Maximize();
            GetDriver.Url = url;
            AutomationCore.Report.Info("Launched Application : " + url);
        }
        public static void Close()
        {
            //_driver.Close();
            if (GetDriver != null)
            {
                GetDriver.Quit();
                GetDriver.Dispose();
                GetDriver = null;
            }

        }
    }
}
