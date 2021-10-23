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
    public class NYSE : AutomationCore
    {
        private IWebDriver driver;

        #region Object Repo

        [FindsBy(How = How.XPath, Using = "*//input[@type='search' and @id='page-search']")]
        private IWebElement txt_NYSE_search { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[text()='To']//preceding::input[1]")]
        private IWebElement txt_FromDate_Hist { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[text()='To']//following::input[1]")]
        private IWebElement txt_ToDate_Hist { get; set; }
        //.//span[contains(text(), 'GO')]
        [FindsBy(How = How.XPath, Using = ".//span[contains(text(), 'GO')]")]
        private IWebElement btn_Go { get; set; }
        //.//div[@class='d-vbox']
        [FindsBy(How = How.XPath, Using = ".//div[@class='d-vbox']")]
        private IWebElement tbl_HistSearch { get; set; }

        #endregion

        #region Constructor
        public NYSE()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        public bool Fn_SearchInNYSE(string stock)
        {
            //EPAM SYS INC
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            try
            {
                if(txt_NYSE_search.Exists(30))
                {
                    txt_NYSE_search.SetText(stock);
                    txt_NYSE_search.SendKeys(Keys.Enter);
                }
                

                if(driver.FindElements(By.XPath(".//a[contains(text(),'"+stock+"')]")).Count > 0)
                {
                    driver.FindElement(By.XPath(".//a[contains(text(),'" + stock + "')]")).Highlight();
                    driver.FindElement(By.XPath(".//a[contains(text(),'" + stock + "')]")).Click();

                    wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[text()='" + stock + "']")));
                    if(driver.FindElement(By.XPath(".//*[text()='" + stock + "']")).Exists())
                    {
                        flag = true;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
            return flag;
        }

        public bool Fn_NavigateToHistoricPricesAndSearch(string fromDate, string toDate)
        {
            
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            try
            {
                IWebElement element = driver.FindElement(By.XPath(".//span[text()='HISTORIC PRICES']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

                Thread.Sleep(10000);

                if(tbl_HistSearch.Exists(30))
                {
                    if (txt_FromDate_Hist.Exists(20))
                    {
                        //txt_FromDate_Hist.SetText(fromDate);
                        txt_FromDate_Hist.Click();
                        txt_FromDate_Hist.SendKeys(Keys.Control + "a");
                        txt_FromDate_Hist.SendKeys(Keys.Delete);
                        txt_FromDate_Hist.SendKeys(fromDate);
                    }
                    Thread.Sleep(2000);
                    if (txt_ToDate_Hist.Exists(20))
                    {
                        txt_ToDate_Hist.Click();
                        txt_ToDate_Hist.SendKeys(Keys.Control + "a");
                        txt_ToDate_Hist.SendKeys(Keys.Delete);
                        txt_ToDate_Hist.SendKeys(toDate);
                    }

                    if (btn_Go.Exists(20))
                    {
                        btn_Go.Click();
                    }
                }

                if(tbl_HistSearch.Exists(30))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
            return flag;
        }

        public static string MyDictionaryToJson(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"date\": \"{0}\",\"value\": {1}\"}\"", d.Key, string.Join(",", d.Value)));

            //string outtext = "{" + string.Join(",", entries) + "}";
            return "{" + string.Join(",", entries) + "}";
        }

        public string Fn_GetDateAndClosePrice(string fromDate, string toDate)
        {
            string output = string.Empty;
            Dictionary<string, string> dictDatePrice = new Dictionary<string, string>();
            try
            {
                var dateList = driver.FindElements(By.XPath(".//div[@class='flex_td Time']//div[@class='data-table-cell']"));

                var closePriceList = driver.FindElements(By.XPath(".//div[@class='flex_td Close']//div[@class='data-table-cell-price-align']"));

                if(dateList.Count == closePriceList.Count)
                {
                    for(int i=0;i<dateList.Count;i++)
                    {
                        dictDatePrice.Add(dateList[i].Text, closePriceList[i].Text);
                    }
                }

                if(dictDatePrice!=null)
                {
                    string json = MyDictionaryToJson(dictDatePrice);
                    json = "{\"period\": {\"startDate\": \""+fromDate+ "\",\"endDate\": \"" + toDate + "\"},\"stockData\": [" + json + "]";
                    output = json;
                }
            }
            catch (Exception ex)
            {
                Report.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
            return output;
        }

    }
}
