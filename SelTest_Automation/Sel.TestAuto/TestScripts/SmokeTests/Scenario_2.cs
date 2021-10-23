using System;
using System.Net;
using System.Threading;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Data;
using Bogus.Extensions.Canada;

namespace Sel.TestAuto
{
    [TestClass]
    public class Scenario_2 : AutomationCore
    {

        [TestMethod]
        public void Scenario2()
        {
            string browser = "browser".AppSettings();
            string url = "urlScenario2".AppSettings();
            Browsers.Init(browser, url);

            if(Browsers.GetDriver.FindElements(By.XPath(".//button[text()='Accept All Cookies']")).Count > 0)
            {
                Browsers.GetDriver.FindElements(By.XPath(".//button[text()='Accept All Cookies']"))[0].Click();
            }
            
            if(Pages.Nyse.Fn_SearchInNYSE("EPAM SYS INC"))
            {
                Report.Pass("Navigate to EPAM Stock details site - Pass");
            }
            else
            {
                Report.Fail("Navigate to EPAM Stock details site - Fail");
                Assert.Fail();
            }

            if(Pages.Nyse.Fn_NavigateToHistoricPricesAndSearch("2021-09-01", "2021-09-30"))
            {
                Report.Pass("Historic price search - Pass");
            }
            else
            {
                Report.Fail("Historic price search - Fail");
                Assert.Fail();
            }

            string jsonOutput = Pages.Nyse.Fn_GetDateAndClosePrice("2021-09-01", "2021-09-30");

            var replySc2 = API.API_SendReceive("post", "https://testathon-service.herokuapp.com/api/v2/stocks/data", jsonOutput, "json");
        }


    }
}
