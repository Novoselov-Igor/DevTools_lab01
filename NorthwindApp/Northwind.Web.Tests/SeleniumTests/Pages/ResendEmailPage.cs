using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class ResendEmailPage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "Input_Email")]
        private HtmlInput InputEmail;

        [FindsBy(How = How.CssSelector, Using = "button.btn-primary")]
        private HtmlInput resendButton;
        public ResendEmailPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {

        }

        public string Email
        {
            get { return InputEmail.Text; }
            set { InputEmail.SendKeys(value); }
        }

        public void Resend()
        {
            resendButton.Click();
        }
    }
}
