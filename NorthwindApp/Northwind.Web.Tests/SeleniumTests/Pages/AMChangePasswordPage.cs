using HtmlElements;
using HtmlElements.Elements;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class AMChangePasswordPage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "Input_OldPassword")]
        private HtmlInput inputOldPass;

        [FindsBy(How = How.Id, Using = "Input_NewPassword")]
        private HtmlInput inputNewPass;

        [FindsBy(How = How.Id, Using = "Input_ConfirmPassword")]
        private HtmlInput inputConfirmPass;

        [FindsBy(How = How.CssSelector, Using = "button.btn-primary")]
        private HtmlLink updateButton;


        public AMChangePasswordPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {
        }

        public string OldPass
        {
            get { return inputOldPass.Text; }
            set { inputOldPass.SendKeys(value); }
        }

        public string NewPass
        {
            get { return inputNewPass.Text; }
            set { inputNewPass.SendKeys(value); }
        }

        public string ConfirmPass
        {
            get { return inputConfirmPass.Text; }
            set { inputConfirmPass.SendKeys(value); }
        }

        public void Update_Changes()
        {
            updateButton.Click();
        }
    }
}
