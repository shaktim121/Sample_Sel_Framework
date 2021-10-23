using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Pages.youTube.searchYoutube("Epam Systems Global");
            Pages.youTube.clickOnChannelVideos();
           // Pages.youTube.sortByDateAddedNew();
            Pages.youTube.get_AllVideosWithin1Year();
        }
    }
}
