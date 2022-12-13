using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class AccountManagePage : HtmlPage
    {
        [FindsBy(How = How.Id, Using = "Input_PhoneNumber")]
        private HtmlInput inputPhoneNumber;

        [FindsBy(How = How.Id, Using = "change-password")]
        private HtmlInput changePass;

        [FindsBy(How = How.Id, Using = "update-profile-button")]
        private HtmlLink saveButton;


        public AccountManagePage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {

        }

        public string PhoneNumber
        {
            get { return inputPhoneNumber.Text; }
            set { inputPhoneNumber.SendKeys(value); }
        }

        public void Save_Changes()
        {
            saveButton.Click();
        }

        public AMChangePasswordPage Go_to_Password()
        {
            changePass.Click();
            return PageObjectFactory.Create<AMChangePasswordPage>(this);
        }
    }
}
