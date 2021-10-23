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
        }


    }
}
