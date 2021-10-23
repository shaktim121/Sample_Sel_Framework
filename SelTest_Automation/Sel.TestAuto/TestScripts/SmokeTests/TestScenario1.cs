using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sel.TestAuto
{
    [TestClass]
   public class TestScenario1: AutomationCore
    {
        [TestMethod]
        public void testScenario1()
        {
            string baseURL = "url".AppSettings();
            string browser = "browser".AppSettings();
            Browsers.Init(browser, baseURL);

            if(Pages.youTube.searchYoutube("Epam Systems Global"))
                Report.Pass("Navigated to Youtube and searched for Epam Systems Global - Pass");
            else
           
            {
                Report.Fail("Navigated to Youtube and searched for Epam Systems Global - Fail");
                Assert.Fail();
            }
            if(Pages.youTube.clickOnChannelVideos())
                Report.Pass("Navigated to Channel Videos- Pass");
            else

                    {
                        Report.Fail("Navigated to Channel Videos- Fail");
                        Assert.Fail();
                    }
            Pages.youTube.sortByDateAddedNew();
            IList<IWebElement> allVideos = Pages.youTube.get_AllVideosWithin1Year();
            if (allVideos.Count > 0)
            {
                Report.Pass("Able to fetch all the videos in descending order- Pass");
                foreach (IWebElement videoLink in allVideos)
                {
                    IWebElement videoUploadedName = videoLink.FindElement(By.XPath("//a"));
                    IWebElement videoUploadedTime = videoLink.FindElement(By.XPath("//span[2]"));
                    IWebElement videoUploadedViews = videoLink.FindElement(By.XPath("//span[1]"));
                    Report.Info("Video Name: " + videoUploadedName.Text + "Video Views" + videoUploadedViews.GetAttribute("innerHTML") + " Video Time: " + videoUploadedTime.GetAttribute("innerHTML"));
                }
            }
            else

            {
                Report.Fail("Able to fetch all the videos in descending order- Fail");
                Assert.Fail();
            }

        }
    }
}
