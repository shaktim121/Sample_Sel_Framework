using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WL.TestAuto
{
    public class LogInPage : AutomationCore
    {
        private IWebDriver driver;

        #region Login Page Object Collection
        //Page Factory, FindsBy and CacheLookUp
        [FindsBy(How = How.Id, Using = "Username")]
        private IWebElement Txt_UserName_Aut { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[translate(@id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='username' or @id='Login1_UserName' or @id='1-email']")]
        public IWebElement Txt_UserName { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@id='Password' or @id='password' or @id='Login1_Password' or @name='password']")]
        public IWebElement Txt_Password { get; set; }

        [FindsBy(How = How.XPath, Using = "*//*[@value='Log In' or text()='Login' or text()='Continue' and @type='submit' or text()='Log In']")]
        public IWebElement Btn_Login { get; set; }

        [FindsBy(How = How.Id, Using = "Password")]
        private IWebElement Txt_Password_Aut { get; set; }

        [FindsBy(How = How.XPath, Using = "*//button[text()=\"Login\" and @type=\"submit\"]")]
        private IWebElement Btn_Login_Aut { get; set; }

        [FindsBy(How = How.Id, Using = "Login1_UserName")]
        private IWebElement Txt_UserName_Wlat { get; set; }

        [FindsBy(How = How.Id, Using = "Login1_Password")]
        private IWebElement Txt_Password_Wlat { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='Log In' and @type='submit']")]
        private IWebElement Btn_Login_Wlat { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='LoginStatus1' and text()=\"Logout\" or text()=\"Déconnexion\"]")]
        private IWebElement Lnk_Logout { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='log-status' and text()='Logout']")]
        private IWebElement Lnk_LogoutNG { get; set; }

        [FindsBy(How = How.XPath, Using = "*//small[text()=\"You are now logged out\"]")]
        private IWebElement Lbl_LogOutScreen { get; set; }

        [FindsBy(How = How.Id, Using = "UserInitials")]
        private IWebElement Lbl_UserInitial { get; set; }

        //div[@class='dropdown']//button[@id='dropdownMenuButton']
        [FindsBy(How = How.Id, Using = "dropdownMenuButton")]
        private IWebElement Lbl_UserInitialNG { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[contains(text(),'Your password is more than 30 days old')]")]
        private IWebElement Lbl_PwdExpired { get; set; }
       
        [FindsBy(How = How.XPath, Using = ".//span[contains(text(),'Wrong email or password')]")]
        private IWebElement Lbl_WrongEmailPwd { get; set; }

        private void ClickOnLogInButton()
        {
            Btn_Login.Click();
            /*string url = ConfigurationManager.AppSettings["url"];

            if (url.Contains("automation"))
            {
                Btn_Login_Aut.Click();
            }
            else if (url.Contains("wlat"))
            {
                Btn_Login_Wlat.Click();
            }
            else
            {
                Btn_Login_Wlat.Click();
            }*/
            
        }

        private bool IsAt()
        {
            try
            {
                if (Browsers.Title.Contains("WebApplication") || Browsers.Title.Contains("Log On") || Browsers.Title.Contains("Log in to Worklinks"))
                {
                    return true;
                }
                else
                {
                    test.Fail("Failed to Launch Application or VPN is not connected");
                    return false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
        }
        #endregion
        //************************************
        #region Constructor
        public LogInPage()
        {
            driver = Browsers.GetDriver;
            //PageFactory.InitElements(driver, this);
        }
        #endregion

        #region LogIn page reusable methods
        public bool Fn_LogInToApplication(string user, string pwd)
        {
            Boolean flag = false;
            //string user = "user".AppSettings();
            //string pwd = "pwd".AppSettings();

            try
            {
                
                Assert.IsTrue(IsAt());

                Txt_UserName.SetText(user);
                Txt_Password.SetText(pwd);

                /*if(url.Contains("automation"))
                {
                    Txt_UserName_Aut.SetText(user);
                    Txt_Password_Aut.SetText(pwd);
                    
                }
                else if (url.Contains("wlat"))
                {
                    Txt_UserName_Wlat.SetText(user);
                    Txt_Password_Wlat.SetText(pwd);
                    
                }
                else
                {
                    Txt_UserName_Wlat.SetText(user);
                    Txt_Password_Wlat.SetText(pwd);
                }*/

                ClickOnLogInButton();

                //if (ConfigurationManager.AppSettings["browser"].ToLower().Equals("firefox"))
                //{
                //    driver.SwitchTo().Alert().Accept();

                //    /*foreach (string handle in driver.WindowHandles)
                //    {
                //        IWebDriver popup = driver.SwitchTo().Window(handle);
                //        if (popup.Title.Contains("Security Warning"))
                //        {
                //            break;
                //        }
                //    }*/
                //}

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                wait.Until(ExpectedConditions.ElementExists(By.Id("UserInitials")));

                string lblUserText = Pages.Home.Fn_GetSignInUser();
                if (!lblUserText.ToLower().Contains(user.ToLower()))
                {
                    test.Fail("Failed to verify Signed in user in Landing Page");
                    test.Fail("Login to application Failed for user : " + user);
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }
                else
                {
                    test.Pass("Login to application successful for user : " + user);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                if (Lbl_WrongEmailPwd.Exists()) test.Error("Wrong Email or Password entered");
                if (Lbl_PwdExpired.Exists()) test.Error("Password is Expired for the user");

                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            //Change it to Explicit wait later
            Thread.Sleep(2000);
            return flag;
        }

        public bool Fn_LogOutOfApplication()
        {
            Boolean flag = false;
            try
            {
                //Assert.IsTrue(Lbl_UserInitial.Exists(10));
                if(Lbl_UserInitial.Exists(10))
                {
                    Lbl_UserInitial.Click();
                    Thread.Sleep(1000);
                    Lnk_Logout.Highlight();
                    Lnk_Logout.Click();
                    Thread.Sleep(1000);
                    
                }
                else if(Lbl_UserInitialNG.Exists(10))
                {
                    Lbl_UserInitialNG.Click();
                    Thread.Sleep(1000);
                    Lnk_LogoutNG.Highlight();
                    Lnk_LogoutNG.Click();
                    Thread.Sleep(1000);
                }

                if (Txt_UserName.Exists(10))
                {
                    test.Pass("Logout screen verified");
                    flag = true;
                }
                else if (Lbl_LogOutScreen.Exists(10))
                {
                    Lbl_LogOutScreen.Highlight();
                    test.Pass("Logout screen verified");
                    flag = true;
                }
                else
                {
                    test.Fail("Failed to verify logout screen");

                }

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                test.Fail("Failed to Logout from application");
                GenericMethods.CaptureScreenshot();
                return false;
            }
            if(flag)
            {
                test.Pass("Logged Out of application successfully");
            }
            else
            {   
                test.Fail("Failed to Logout from application");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Logout from application");
            }
            return flag;
        }
        #endregion

    }
}
