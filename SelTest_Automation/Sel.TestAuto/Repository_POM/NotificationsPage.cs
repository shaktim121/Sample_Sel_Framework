using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sel.TestAuto
{
    public class NotificationsPage : AutomationCore
    {
        private IWebDriver driver;

        #region Constructor
        public NotificationsPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Notifications Page Object Collection
        
        [FindsBy(How = How.XPath, Using = ".//*[contains(@style,'display:block;') or contains(@style,'display: block;')]//div[@id='ctl00_MainMenuNew']//span[@class='rmText rmExpandDown' and text()='Notifications']")]
        private IWebElement Menu_Notifications { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='ctl00_MainMenuNew']/ul/li//span[@class='rmText rmExpandDown' and text()='Notifications']//following::div[1]")]
        private IWebElement Menu_SlideNotifications { get; set; }

        #endregion

        #region Notifications Page reusable Methods

        //Navigate to screens under Notifications
        public bool Fn_Navigate_Through_Notifications(string Option)
        {
            bool flag = false;
            try
            {

                if (Option != "")
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Notifications, "Notifications", Menu_SlideNotifications, Option);
                    if (driver.FindElements(By.XPath(".//*[contains(text(),'Notifications->" + Option + "')]")).Count > 0)
                    {
                        driver.FindElement(By.XPath(".//*[contains(text(),'Notifications->" + Option + "')]")).Highlight();
                        test.Pass("Verified 'Notifications -> " + Option + "' on page");
                        test.Pass("Navigated to " + Option + " Screen under Notifications");
                        flag = true;
                    }
                    else if (driver.FindElement(By.XPath(".//a[text()='" + Option + "']")).Exists(30))
                    {
                        driver.FindElement(By.XPath(".//a[text()='" + Option + "']")).Highlight();
                        test.Pass("Verified 'Notifications -> " + Option + "' on page");
                        test.Pass("Navigated to " + Option + " Screen under Notifications");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Notifications -> " + Option + "' on page");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        #endregion
    }
}
