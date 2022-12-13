using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class LoginPage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "forgot-password")]
        private HtmlLink forgotPassword;

        [FindsBy(How = How.Id, Using = "resend-confirmation")]
        private HtmlLink resendEmail;

        [FindsBy(How = How.Id, Using = "Input_Email")]
        private HtmlInput LoginEmail;

        [FindsBy(How = How.Id, Using = "Input_Password")]
        private HtmlInput LoginPass;

        [FindsBy(How = How.Id, Using = "login-submit")]
        private HtmlLink LoginButton;

        public LoginPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {
        }

        public string Email
        {
            get { return LoginEmail.Value; }
            set { LoginEmail.SendKeys(value); }
        }

        public string Password
        {
            get { return LoginPass.Value; }
            set { LoginPass.SendKeys(value); }
        }

        public MainPage Login_User()
        {
            LoginButton.Click();
            return PageObjectFactory.Create<MainPage>(this);
        }

        public ForgotPasswordPage Go_to_ForgotPassword()
        {
            forgotPassword.Click();
            return PageObjectFactory.Create<ForgotPasswordPage>(this);
        }

        public ResendEmailPage Go_to_ResendEmail()
        {
            resendEmail.Click();
            return PageObjectFactory.Create<ResendEmailPage>(this);
        }
    }
}
