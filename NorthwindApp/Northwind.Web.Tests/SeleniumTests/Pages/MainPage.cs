using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Northwind.Web.Tests.SeleniumTests.Pages
{
    public class MainPage : HtmlPage
    {
        [FindsBy(How = How.CssSelector, Using = "a[href*='Categories'].nav-link")]
        private HtmlLink categoriesLink;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Identity/Account/Register'].nav-link")]
        private HtmlLink RegistrationLink;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Identity/Account/Login'].nav-link")]
        private HtmlLink LoginLink;

        [FindsBy(How = How.CssSelector, Using = "a[href*='Identity/Account/Manage'].nav-link")]
        private HtmlLink AccountManageLink;

        public MainPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper)
        {
        }

        public CategoryListPage GoToCategoriesListPage()
        {
            categoriesLink.Click();
            return PageObjectFactory.Create<CategoryListPage>(this);
        }

        public RegistrationPage GoToRegistrationPage()
        {
            RegistrationLink.Click();
            return PageObjectFactory.Create<RegistrationPage>(this);
        }

        public LoginPage GoToLoginPage()
        {
            LoginLink.Click();
            return PageObjectFactory.Create<LoginPage>(this);
        }

        public AccountManagePage GoToAccountManagePage()
        {
            AccountManageLink.Click();
            return PageObjectFactory.Create<AccountManagePage>(this);
        }
    }
}
