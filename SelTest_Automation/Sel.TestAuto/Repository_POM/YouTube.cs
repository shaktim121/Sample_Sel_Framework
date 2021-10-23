using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sel.TestAuto
{
   public class YouTube : AutomationCore
    {
        private IWebDriver webDriver;

        [FindsBy(How = How.XPath, Using = "//input[@id='search']")]
        private IWebElement Txt_Search { get; set; }

        [FindsBy(How = How.XPath, Using = " //button[@id='search-icon-legacy']")]
        private IWebElement Btn_Search { get; set; }

        [FindsBy(How = How.XPath, Using = "//yt-formatted-string[@id='text' and contains(text(),'EPAM Systems Global')]")]
        private IWebElement Lnk_Search { get; set; }

        [FindsBy(How = How.XPath, Using = " (//tp-yt-paper-tab[@role='tab'])[2]")]
        private IWebElement Lnk_Videos { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='sort-menu']")]
        private IWebElement Btn_Sort_Videos { get; set; }

        [FindsBy(How = How.XPath, Using = "(//tp-yt-iron-dropdown[@id='dropdown' and @aria-disabled='false' and not(@aria-hidden)]//div[@id='contentWrapper']//tp-yt-paper-listbox//a)[3]")]
        private IWebElement Btn_Date_Added_New { get; set; }

        [FindsBy(How = How.XPath, Using = "//ytd-two-column-browse-results-renderer[@page-subtype='channels']//ytd-item-section-renderer//div[@id='contents']//div[@id='items']//div[@id='details']")]
        private IList<IWebElement> Link_All_Videos { get; set; }



        public YouTube()
        {
            webDriver = Browsers.GetDriver;
        }

        public bool searchYoutube(String searchKey)
        {
            bool flag = false;
            Txt_Search.SendKeys(searchKey);
            Btn_Search.Click();
            Lnk_Search.Click();
            flag = true;
            return flag;
        }

        public bool clickOnChannelVideos()
        {
            bool flag = false;
            Lnk_Videos.Click();
            flag = true;
            return flag;
        }

        public bool sortByDateAddedNew()
        {
            bool flag = false;
            Btn_Sort_Videos.Click();
           /* Actions actions = new Actions(Browsers.GetDriver);
            actions.MoveToElement(Btn_Date_Added_New).Click().Build().Perform();*/
            Btn_Sort_Videos.Click();
            flag = true;

            return flag;
        }

        public IList<IWebElement> get_AllVideosWithin1Year()
        {
            
            IList<IWebElement> allVideosNeeded = new List<IWebElement>();
            foreach(IWebElement videoLink in  Link_All_Videos){
                IWebElement videoUploaded = videoLink.FindElement(By.XPath("//span[2]"));
                IWebElement videoViews = videoLink.FindElement(By.XPath("//span[1]"));
                string timeAddedElementText = videoUploaded.GetAttribute("innerHTML");
                if (timeAddedElementText.Equals("2 years ago"))
                {
                    break;
                }
                else
                {
                    Actions actions = new Actions(Browsers.GetDriver);
                    actions.MoveToElement(videoLink).Build().Perform();
                    allVideosNeeded.Add(videoUploaded);
                    Console.WriteLine(videoViews.GetAttribute("innerHTML")+" videoViews"+timeAddedElementText);
                }
            }
            return allVideosNeeded;
        }


    }
}
