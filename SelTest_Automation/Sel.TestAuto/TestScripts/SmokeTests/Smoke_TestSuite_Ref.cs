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
    public class Smoke_TestSuite_Ref : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";
        

        [TestMethod]
        //TEST-172
        public void TEST_172_WL_CAN_Reports_001000_Verify_Each_Report_Has_DSNPath()
        {
            string sql = data.GetTestData("StoredProcedure");
            //string reportNames = data.GetTestData("ReportNames");
            //string criteria = data.GetTestData("SearchCriteria");
            string DBServer = "dataSource".AppSettings();
            string DBName = "ReportServer";
            string DBuserName = "dbUser".AppSettings();
            string DBpassword = "dbPwd".AppSettings();
            try
            {
                string connection = GlobalDB.CreateConnectionString(DBServer, DBName, DBuserName, DBpassword, true);

                int failCount = 0;

                //Fetch all report details through SQL query
                DataSet reports = GlobalDB.ExecuteSQLQuery(sql, connection);
                if (reports != null && reports.Tables[0].Rows.Count > 0)
                {
                    test.Info("Total reports found : " + reports.Tables[0].Rows.Count);
                    foreach (DataRow row in reports.Tables[0].Rows)
                    {
                        if (row["DSNPath"].ToString() == "" || row["DSNName"].ToString() == "")
                        {
                            test.Fail("DSN Path or DSN Name is empty for Report : " + row["ReportName"].ToString() + " in Directory : " + row["directory"].ToString());
                            failCount++;
                        }
                        else
                        {
                            test.Pass("DSN Path exist for Report : " + row["ReportName"].ToString() + " in Directory : " + row["directory"].ToString());
                        }
                    }

                }
                else
                {
                    test.Fail("Query returned no records");
                    Assert.Fail("Test Failed");
                }

                if (failCount == 0)
                {
                    test.Pass("No records found with Null DSN Path");
                }
                else
                {
                    test.Fail(failCount + " : records found with Null DSN Path");
                    Assert.Fail("Test Failed");
                }
            }
            catch(Exception ex)
            {
                test.Error("Exception occured with message : " + ex.Message);
                throw;
            }

        }

        //[TestMethod]
        ////1.	Human Resources - Employee Screen - TEST-2
        //public void TEST_02_WL_CAN_HR_UI_000100_Verify_Human_Resources_Employee_Screen()
        //{
        //    #region Data Variables
        //    string userLang = data.GetTestData("User_Language");
        //    string userProf = data.GetTestData("User_Profile");
        //    string screen = data.GetTestData("HR_Screen");

        //    string textFields = data.GetTestData("Verify_TextFields");
        //    string buttons = data.GetTestData("Verify_Buttons");
        //    string drpDowns = data.GetTestData("Verify_DropDowns");
        //    string checkBoxes = data.GetTestData("Verify_CheckBoxes");
        //    string toolBars = data.GetTestData("Verify_ToolBars");
        //    string tableColumns = data.GetTestData("Verify_TableColumns");
        //    string labelFields = data.GetTestData("Verify_LabelFields");
        //    #endregion

        //    //Change User language after log in
        //    Pages.Home.Fn_ChangeUserLanguage(userLang);

        //    //Select User Profile
        //    Pages.Home.Fn_SelectUserProfile(userProf);

        //    //Navigate to Employee Screen in Human Resources
        //    Pages.HR.Fn_NavigateThroughHumanResources(screen);

        //    //Verify all fields in Employee Screen
        //    if (Pages.HR.Fn_Verify_Fields_In_HR_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
        //    {
        //        test.Log(Status.Pass, "All fields in Employee screen verified successfully");
        //    }
        //    else
        //    {
        //        test.Fail("Failed to verify all fields in Employee screen");
        //        Assert.Fail();
        //    }

        //    //Search Employee records present
        //    if (Pages.HR.Fn_Verify_Record_Displayed_In_EmployeeTable())
        //    {
        //        test.Log(Status.Pass, "Verified Employee records displayed");
        //    }
        //    else
        //    {
        //        test.Fail("Failed to verify Employee record displayed");
        //        Assert.Fail();
        //    }
        //}

        


       

    }
}
