using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class FPConfirmationPage : HtmlPage
    {
        [FindsBy(How = How.CssSelector, Using = "h1")]
        private HtmlLink msg;

        public FPConfirmationPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {

        }

        public string message
        {
            get { return msg.Text; }
        }
    }
}
