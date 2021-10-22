using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Sel.TestAuto
{
    public class LandingPage : AutomationCore
    {
        private IWebDriver driver;

        #region Common Object Collection

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent rwLoading']")]
        private IWebElement Win_PDFReportLoad { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent']")]
        private IWebElement Win_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'RegisterReportWindow') or contains(@name,'PayrollProcessWindow') or contains(@name,'YearEndWindows') or contains(@name,'YearEndReportWindows')]")]
        private IWebElement Frame_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }

        #endregion

        #region Home Page Object Collection
        //[FindsBy(How = How.Id, Using = "lblLoggedInValue")]
        //private IWebElement Lbl_user { get; set; }

        [FindsBy(How = How.Id, Using = "UserEmail")]
        private IWebElement Lbl_user { get; set; }

        [FindsBy(How = How.Id, Using = "UserInitials")]
        private IWebElement Lbl_userInitial { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[contains(@id,'company') and @class='styleCompany']")]
        private IWebElement Lbl_CompanyHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_NewProfileGroup_ProfileGroup_Input")]
        private IWebElement Drpdwn_UserGroup { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ProfileGroup_ProfileGroup_DropDown")]
        private IWebElement List_UserGroup { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@style,'display:block;') or contains(@style,'display: block;')]//span[text()='Home' or text()='Maison']")]
        private IWebElement Menu_Home { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='WebsiteLanguage']")]
        private IWebElement Link_Language { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_WebsiteLanguage_Input")]
        private IWebElement Drpdwn_Language { get; set; }


        private bool IsAt()
        {
            return Browsers.Title.Contains("WorkLinks");
        }
        #endregion

        #region Constructor
        public LandingPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Landing Page reusable Methods
        //Gets the signed in user name
        public String Fn_GetSignInUser()
        {
            try
            {
                Assert.IsTrue(IsAt());
                if(Lbl_userInitial.Exists())
                {
                    Lbl_userInitial.Click();
                    Thread.Sleep(1000);
                    Lbl_user.Highlight();
                }
                
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return Lbl_user.Text.ToString();
        }

        //Gets the company name from page header
        public String Fn_GetCompanyName()
        {
            try
            {
                Assert.IsTrue(IsAt());
                Lbl_CompanyHeader.Highlight();
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return Lbl_CompanyHeader.Text.ToString();
        }

        //Change the Website language
        public bool Fn_ChangeUserLanguage(string language)
        {
            Boolean flag = false;
            try
            {
                if(Drpdwn_Language.Exists())
                {
                    flag = Drpdwn_Language.SelectValueFromDropDown(language);
                }
                if(flag)
                {
                    Report.Pass("User Language changed to :" + language);
                }
                else
                {
                    Report.Fail("Unable to change Language to : " + language);
                }
                //if(Link_Language.Exists(10))
                //{
                //    string lng = Link_Language.Text;
                    
                //    if (language.ToLower().Equals("english")) language = "EN";
                //    else language = "FR";

                //    if(!language.Equals(lng))
                //    {
                //        Link_Language.Click();
                //        Report.Pass("User Language changed to :" + language);
                //        flag = true;
                //    }
                //    else
                //    {
                //        Report.Pass("User Language already in :" + language);
                //        flag = true;
                //    }
                //}
                //else
                //{
                //    Report.Fail("Unable to change Language");
                //    flag = false;
                //}
                
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Selects the Signed in user profile as Administrator or Others
        public bool Fn_SelectUserProfile(string Option)
        {
            Boolean flag = false;
            try
            {
                flag = Drpdwn_UserGroup.SelectValueFromDropDown(Option);
                if (flag)
                {
                    Report.Pass("User Type selected : " + Option);
                }
                else
                {
                    Report.Fail("Failed to select User Type: " + Option);
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            
            return flag;
        }

        //Navigate to Home Screen
        public bool Fn_NavigateToHomeScreen()
        {
            Boolean flag = false;
            try
            {
                if (Menu_Home.Exists(30))
                {
                    Menu_Home.Highlight();
                    Menu_Home.Click();
                    IWebElement Link_Home = driver.FindElement(By.XPath("*//a[contains(text(),'Home') or contains(text(),'Maison')]"));
                    if (Link_Home.Exists(20))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag= false;
                    }
                }
                else
                {
                    Report.Fail("Unabe to find 'Home' Menu");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify records in a column in UI table
        public bool Fn_VerifyRecordsInSingleColumn(IWebElement table, string columnName, string colValueString)
        {
            Boolean flag = false;
            int colNum, rowCount, finalCount = 0;
            IReadOnlyList<IWebElement> rows;
            string[] colVals;
            try
            {
                if (table.Exists(10))
                {
                    colNum = table.GetColumnNumber(columnName);
                    rows = table.FindElements(By.XPath("./tbody/tr"));
                    rowCount = table.FindElements(By.XPath("./tbody/tr")).Count;
                    colVals = colValueString.Split(';');

                    if (rowCount > 0)
                    {
                        foreach (string colVal in colVals)
                        {
                            int valCount = 0;
                            for (int i = 0; i < rowCount; i++)
                            {
                                if (rows[i].FindElements(By.TagName("td"))[colNum].Text.ToLower().Equals(colVal.ToLower()))
                                {
                                    rows[i].FindElements(By.TagName("td"))[colNum].Highlight();
                                    valCount++;
                                    Report.Pass("Value : " + colVal + " found in Column : " + columnName);
                                    break;
                                }
                            }
                            if (valCount == 0)
                            {
                                Report.Fail("Value : " + colVal + " Not found in Column : " + columnName);
                                flag = false;
                            }
                            else
                            {
                                finalCount++;
                            }
                        }

                        if (finalCount == colVals.Length)
                        {
                            Report.Pass("All records found in Column : " + columnName);
                            flag = true;
                        }
                        else
                        {
                            Report.Fail("Few or all of the records are missing in Column : " + columnName);
                            flag = false;
                        }
                    }
                    else
                    {
                        Report.Fail("No rows present in table");
                        flag = false;
                    }
                }
                else
                {
                    Report.Fail("Required table does not Exist");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Payroll reports from DB Stored Proc
        //spParams format - param1:val1;param2:val2;etc
        //dataToVerify format - col1:val1;col2:val2;etc
        public bool Fn_Verify_ReportsData_In_DataBase(string storedProc, string spParams, string dataToVerify, [Optional] string connection)
        {
            bool flag = false;
            DataSet ds;

            try
            {
                if(connection=="" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), Convert.ToBoolean("winAuth".AppSettings()));
                }

                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                Report.Info("Stored Procedure : " + storedProc);
                Report.Info("SP Parameters : " + spParams);
                Report.Info("Connection parameters: " + connection);

                if (dataToVerify.ToLower().Contains("data exists"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Report.Pass("Data Exists in the Report and is not empty");
                        flag = true;
                    }
                    else
                    {
                        Report.Fail("No data present or Report is empty");
                    }
                }
                else
                {
                    string[] data = dataToVerify.Split(';');
                    flag = true;
                    foreach (string dt in data)
                    {
                        int cnt = 0;

                        foreach (DataRow dRow in ds.Tables[0].Rows)
                        {
                            cnt++; 
                            string aval = Convert.ToString(dRow[dt.Split(':')[0]]);
                            string eval = dt.Split(':')[1];
                            
                            if(eval.ToLower().Contains("non zero"))
                            {
                                if(aval!="")
                                {
                                    int a = (int)Convert.ToDouble(aval);
                                    if (!a.Equals(0))
                                    {
                                        Report.Pass("column : " + dt.Split(':')[0] + " has Non Zero value in the report DB");
                                        break;
                                    }
                                    else
                                    {
                                        Report.Fail("column: " + dt.Split(':')[0] + " has Zero value in the report DB");
                                        flag = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    Report.Fail("column: " + dt.Split(':')[0] + " has Zero value in the report DB");
                                    flag = false;
                                    break;
                                }
                                
                            }

                            else if (aval.Contains(eval))
                            {
                                Report.Pass(eval + " : found in the report DB under column : "+ dt.Split(':')[0]);
                                break;
                            }

                            if (cnt == ds.Tables[0].Rows.Count)
                            {
                                Report.Fail(eval + ": not found in the report DB under column : " + dt.Split(':')[0]);
                                flag = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify report columns in DB through SP
        //colNames: col1;col2;col3;etc
        public bool Fn_Verify_ReportHeaders_In_DataBase(string storedProc, string spParams, string colNames, [Optional] string connection)
        {
            bool flag = true;
            DataSet ds;
            try
            {
                if (connection == "" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), Convert.ToBoolean("winAuth".AppSettings()));
                }
                
                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                foreach(string col in colNames.Split(';'))
                {
                    if (ds.Tables[0].Columns.Contains(col))
                    {
                        Report.Pass("Column Name : " + col + " found in Report DB");
                    }
                    else
                    {
                        Report.Fail("Column Name : " + col + " not found in Report DB");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Gets all the values from the specified column from report DB and returns a List
        public List<string> Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName, [Optional] string connection)
        {
            List<string> listVals = new List<string>();
            DataSet ds;
            string colval;
            try
            {
                if (connection == "" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), Convert.ToBoolean("winAuth".AppSettings()));
                }

                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        colval = row[colName].ToString();
                        listVals.Add(colval);
                    }
                }
                else
                {
                    listVals.Add("");
                }   
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return listVals;
        }

        //Get data from specified column and row number
        public string Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName, int row, [Optional] string connection)
        {
            DataSet ds;
            string colval = "";
            try
            {
                if (connection == "" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), Convert.ToBoolean("winAuth".AppSettings()));
                }

                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    colval = ds.Tables[0].Rows[row][colName].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return colval;
        }

        //Get data from specified column with reference to reference column and value
        public string Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName, string refCol, string refVal, [Optional] string connection)
        {
            DataSet ds;
            string colval = "";
            try
            {
                if (connection == "" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), Convert.ToBoolean("winAuth".AppSettings()));
                }

                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        bool flag = true;
                        for(int i = 0; i<refCol.Split(';').Length; i++)
                        {
                            if(!row[refCol.Split(';')[i]].ToString().Equals(refVal.Split(';')[i]))
                            {
                                flag = false;
                            }
                        }
                        if(flag)
                        {
                            colval = row[colName].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return colval;
        }


        #endregion

        #region Common Methods for any page

        //Verify PDF window opened
        public bool Fn_Verify_PDF_Opened_In_Window()
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                if (Win_PDFReportLoad.Exists(5))
                {
                    if (wait.Until(driver => Win_PDFReport.Exists(45)))
                    {
                        flag = true;
                    }
                }

                if (Win_PDFReport.Exists(10))
                {
                    string frameName = Win_PDFReport.FindElements(By.XPath(".//iframe")).FirstOrDefault().GetAttribute("name");
                    driver.SwitchTo().Frame(frameName);
                    if (driver.FindElements(By.XPath("*//embed[@type='application/pdf']")).Count > 0)
                    {
                        Report.Pass("PDF report opened successfully");
                        flag = true;
                    }
                    else
                    {
                        Report.Fail("Failed to verify PDF Report");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                    driver.SwitchTo().DefaultContent();
                    Btn_CloseX.Click();
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Downloaded Excel
        public bool Fn_Save_And_Verify_Excel(string downloadFileName="")
        {
            bool flag = false;
            try
            {

                if (GenericMethods.WaitForFileExists(downloadsFolder, downloadFileName + "*.xls*", 60))
                {
                    Report.Pass("Excel report download successful");
                    flag = true;
                }
                else if (GenericMethods.SaveFileFromDialog(downloadsFolder, downloadFileName + ".xlsx", 30))
                {
                    GenericMethods.CaptureScreenshot();
                    Report.Pass("Excel report download successful");
                    flag = true;
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    Report.Fail("Failed to verify Excel report download");
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
            
        }


        #endregion


    }
}
