﻿using System;
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
    //[TestClass]
    public class Smoke_TestSuite1 : AutomationCore
    {

        //[TestMethod]
        public void Test001_Test_Method()
        {
            //Launch and Close Google
            Report.Info("Test Method");

            var resCovid19 = API.API_SendReceive("get", "https://data.covid19india.org/data.json", "", "json");
            Thread.Sleep(5000);
        }
       

    }
}
