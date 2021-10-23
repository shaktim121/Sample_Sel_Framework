using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;

namespace Sel.TestAuto
{
    public static class Pages
    {
        private static T GetPages<T>() where T : new()
        {
            var page = new T();
            PageFactory.InitElements(Browsers.GetDriver, page);
            return page;
        }

        //LogIn Page
        public static LogInPage LogIn
        {
            get { return GetPages<LogInPage>(); }
        }

        //Home Page
        public static LandingPage Home
        {
            get { return GetPages<LandingPage>(); }
        }

        //Notifications page
        public static NotificationsPage Notifications
        {
            get { return GetPages<NotificationsPage>(); }
        }

        //LogIn Page
        public static NYSE Nyse
        {
            get { return GetPages<NYSE>(); }
        }

    }
}
