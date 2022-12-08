using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class RegistrationPage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "Input_Email")]
        private HtmlInput RegisterEmail;

        [FindsBy(How = How.Id, Using = "Input_Password")]
        private HtmlInput RegisterPass;

        [FindsBy(How = How.Id, Using = "Input_ConfirmPassword")]
        private HtmlInput RegisterConfirmPass;

        [FindsBy(How = How.Id, Using = "registerSubmit")]
        private HtmlLink createButton;

        public RegistrationPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {
        }

        public string Email
        {
            get { return RegisterEmail.Value; }
            set { RegisterEmail.SendKeys(value); }
        }

        public string Password
        {
            get { return RegisterPass.Value; }
            set { RegisterPass.SendKeys(value); }
        }

        public string ConfirmPassword
        {
            get { return RegisterConfirmPass.Value; }
            set { RegisterConfirmPass.SendKeys(value); }
        }

        public void Register_New_User()
        {
            createButton.Click();
        }

    }
}
