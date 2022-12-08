using FluentAssertions;
using HtmlElements;
using HtmlElements.Elements;
using Northwind.Web.Tests.SeleniumTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System;

namespace Northwind.Web.Tests.SeleniumTests
{
    public class CategoryFunctionalSeleniumTests : SeleniumTestsBase
    {
        public CategoryFunctionalSeleniumTests(BrowserTypes browserType)
            : base(browserType)
        { }

        [Test]
        public void ShowCategoriesList()
        {
            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            var categoryLink = webDriver
                .FindElement(By.CssSelector("a[href*='Categories'].nav-link"));
            categoryLink.Click();

            // Получаем и сверяем только первые 8, которые заносятся
            // при деплое базы
            var categoryNames = webDriver
                .FindElements(By.CssSelector("td[data-tid='category-name']"))
                .Select(e => e.Text)
                .Take(8);

            var names = new[] {
                "Beverages", "Condiments", "Confections", "Dairy Products",
                "Grains/Cereals", "Meat/Poultry", "Produce", "Seafood" };

            categoryNames.Should().BeEquivalentTo(names);
        }

        [Test]
        public void CreateNewCategory()
        {
            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var categoriesList = mainPage.GoToCategoriesListPage();
            var currentCategoryCount = categoriesList.Categories.Count;

            var categoryForAdd = new
            {
                Name = $"New Category {currentCategoryCount + 1}",
                Description = "New Description",
            };

            var createNewPage = categoriesList.GoToCreateNewCategoryPage();
            createNewPage.CategoryName = categoryForAdd.Name;
            createNewPage.Description = categoryForAdd.Description;
            createNewPage.AddPictureFile(Path.Combine(testFilesPath, "json.jpg"));

            categoriesList = createNewPage.CreateAndGoToList();
            var newCategoryRow = categoriesList.Categories.Last();
            var newCategory = new
            {
                Name = newCategoryRow.CategoryName,
                newCategoryRow.Description,
            };

            categoriesList.Categories.Count.Should().Be(currentCategoryCount + 1);
            newCategory.Should().BeEquivalentTo(categoryForAdd);
        }

        [Test]
        public void Register_New_User()
        {
            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Registration = mainPage.GoToRegistrationPage();

            Registration.Email = email;
            Registration.Password = pass;
            Registration.ConfirmPassword = pass;

            Registration.Register_New_User();

            var TestHelper = new IdentityTestHelper();
            var user = TestHelper.GetUser();

            TestHelper.DeleteAllUsers();

            user.Email.Should().BeEquivalentTo(email);
        }

        [Test]
        public void Register_Existing_User()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string expected = $"Username '{email}' is already taken.";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Registration = mainPage.GoToRegistrationPage();

            Registration.Email = email;
            Registration.Password = pass;
            Registration.ConfirmPassword = pass;

            Registration.Register_New_User();

            var result = Registration.FindElement(By.CssSelector("div.text-danger.validation-summary-errors")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Registration_Confirmation_Password_dont_Match()
        {
            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string expected = "The password and confirmation password do not match.";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Registration = mainPage.GoToRegistrationPage();

            Registration.Email = email;
            Registration.Password = pass;
            Registration.ConfirmPassword = "test";
            Registration.Register_New_User();

            var result = Registration.FindElement(By.CssSelector("span.text-danger.field-validation-error")).Text;

            result.Should().BeEquivalentTo(expected);

        }

        [Test]
        public void Registration_Confirmation_Password_WrongLength()
        {
            string email = "anne2021@mail.ru";
            string pass = "www";
            string expected = "The Password must be at least 6 and at max 100 characters long.";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Registration = mainPage.GoToRegistrationPage();

            Registration.Email = email;
            Registration.Password = pass;
            Registration.ConfirmPassword = pass;
            Registration.Register_New_User();

            var result = Registration.FindElement(By.CssSelector("span.text-danger.field-validation-error")).Text;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Registration_Confirmation_Password_WithoutSpecialCharacters()
        {
            string email = "anne2021@mail.ru";
            string pass = "wwwwww";
            string expected = 
                "Passwords must have at least one non alphanumeric character." +
                "\r\nPasswords must have at least one digit ('0'-'9')." +
                "\r\nPasswords must have at least one uppercase ('A'-'Z').";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Registration = mainPage.GoToRegistrationPage();

            Registration.Email = email;
            Registration.Password = pass;
            Registration.ConfirmPassword = pass;
            Registration.Register_New_User();

            var result = Registration.FindElement(By.CssSelector("div.text-danger.validation-summary-errors")).Text;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Existing_User_And_Exit()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string expected1 = $"Привествуем {email}!";
            string expected2 = "Войти";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var result1 = mainPage.FindElement(By.CssSelector("a[href*='Identity/Account/Manage'].nav-link")).Text;
            mainPage.FindElement(By.CssSelector("button.nav-link.btn.btn-link")).Click();
            var result2 = mainPage.FindElement(By.CssSelector("a[href*='Identity/Account/Login'].nav-link")).Text;

            TestHelper.DeleteAllUsers();

            result1.Should().BeEquivalentTo(expected1);
            result2.Should().BeEquivalentTo(expected2);
        }

        [Test]
        public void Login_Existing_User_Empty_Password()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "";
            string expected = "The Password field is required.";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;
            Login.Login_User();

            var result = Login.FindElement(By.CssSelector("span.text-danger.field-validation-error")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Existing_User_Wrong_Password()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "wwwwww";
            string expected = "Invalid login attempt.";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;
            Login.Login_User();

            var result = Login.FindElement(By.CssSelector("div.text-danger.validation-summary-errors")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Forgot_Password()
        {
            string email = "anne2021@mail.ru";
            string expected = "Forgot password confirmation";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var forgotPassword = mainPage.GoToLoginPage().Go_to_ForgotPassword();

            forgotPassword.Email = email;

            var result = forgotPassword.Go_to_ConfirmationPage().message;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Forgot_Password_Empty_Email()
        {
            string email = "";
            string expected = "The Email field is required.";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var forgotPassword = mainPage.GoToLoginPage().Go_to_ForgotPassword();

            forgotPassword.Email = email;
            forgotPassword.Go_to_ConfirmationPage();

            var result = forgotPassword.FindElement(By.CssSelector("span.text-danger.field-validation-error")).Text;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Resend_Email()
        {
            string email = "anne2021@mail.ru";
            string expected = "Verification email sent. Please check your email.";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var resendEmail = mainPage.GoToLoginPage().Go_to_ResendEmail();

            resendEmail.Email = email;
            resendEmail.Resend();

            var result = resendEmail.FindElement(By.CssSelector("div.text-danger.validation-summary-errors")).Text;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Login_Resend_Empty_Email()
        {
            string email = "";
            string expected = "The Email field is required.";

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var resendEmail = mainPage.GoToLoginPage().Go_to_ResendEmail();

            resendEmail.Email = email;
            resendEmail.Resend();

            var result = resendEmail.FindElement(By.CssSelector("div.text-danger.validation-summary-errors")).Text;

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AccountManage_Change_PhoneNumber()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string phonenumber = "666666666";
            string expected = "Your profile has been updated";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var accountManage = mainPage.GoToAccountManagePage();

            accountManage.PhoneNumber = phonenumber;
            accountManage.Save_Changes();

            var result = accountManage.FindElement(By.CssSelector("div.alert-success.alert-dismissible")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AccountManage_Change_to_Invalid_PhoneNumber()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string phonenumber = "66pasad6666666";
            string expected = "The Phone number field is not a valid phone number.";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var accountManage = mainPage.GoToAccountManagePage();

            accountManage.PhoneNumber = phonenumber;
            accountManage.Save_Changes();

            var result = accountManage.FindElement(By.CssSelector("span.text-danger.field-validation-error")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AccountManage_Change_Password()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string newpass = "Anne2001!";
            string expected = "Your password has been changed.";

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var passwordManage = mainPage.GoToAccountManagePage().Go_to_Password();

            passwordManage.OldPass = pass;
            passwordManage.NewPass = newpass;
            passwordManage.ConfirmPass = newpass;
            passwordManage.Update_Changes();

            var result = passwordManage.FindElement(By.CssSelector("div.alert-success.alert-dismissible")).Text;

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AccountManage_Change_to_Empty_Password()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string newpass = "";
            string[] expected = 
                { 
                "The Current password field is required.",
                "The New password field is required."
            };

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var passwordManage = mainPage.GoToAccountManagePage().Go_to_Password();

            passwordManage.OldPass = "";
            passwordManage.NewPass = newpass;
            passwordManage.ConfirmPass = newpass;
            passwordManage.Update_Changes();

            var tmp = passwordManage.FindElements(By.CssSelector("span.text-danger.field-validation-error"));

            List<string> result = new List<string>();
            foreach(var a in tmp)
            {
                result.Add(a.Text);
            }

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AccountManage_Change_ConfirmPass_Mismatch_NewPass_TooShort()
        {
            var TestHelper = new IdentityTestHelper();

            string email = "anne2021@mail.ru";
            string pass = "Testing2321!";
            string newpass = "www";
            string[] expected = 
                {
                "The New password must be at least 6 and at max 100 characters long.",
                "The new password and confirmation password do not match."
            };

            TestHelper.AddUser(email, pass);

            webDriver.Navigate().GoToUrl("http://localhost:5129/");
            IPageObjectFactory pageFactory = new PageObjectFactory();

            var mainPage = pageFactory.Create<MainPage>(webDriver);

            var Login = mainPage.GoToLoginPage();

            Login.Email = email;
            Login.Password = pass;

            mainPage = Login.Login_User();

            var passwordManage = mainPage.GoToAccountManagePage().Go_to_Password();

            passwordManage.OldPass = pass;
            passwordManage.NewPass = newpass;
            passwordManage.ConfirmPass = "";
            passwordManage.Update_Changes();

            var tmp = passwordManage.FindElements(By.CssSelector("span.text-danger.field-validation-error"));

            List<string> result = new List<string>();
            foreach(var a in tmp)
            {
                result.Add(a.Text);
            }

            TestHelper.DeleteAllUsers();

            result.Should().BeEquivalentTo(expected);
        }

    }
}


