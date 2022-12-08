using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class ForgotPasswordPage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "Input_Email")]
        private HtmlInput InputEmail;

        [FindsBy(How = How.CssSelector, Using = "button.btn-primary")]
        private HtmlLink ResetButton;

        public ForgotPasswordPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {

        }

        public string Email
        {
            get { return InputEmail.Value; }
            set { InputEmail.SendKeys(value); }
        }

        public FPConfirmationPage Go_to_ConfirmationPage()
        {
            ResetButton.Click();
            return PageObjectFactory.Create<FPConfirmationPage>(this);
        }

    }
}
