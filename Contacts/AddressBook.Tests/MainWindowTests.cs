using AddressBook.Tests.Windows;
using Bogus.DataSets;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using FluentAssertions;
using Microsoft.Communications.Contacts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Markup;

namespace AddressBook.Tests
{
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        ContactManager contactManager;
        private AutomationBase automation;
        private Application app;


        [SetUp]
        public void Setup()
        {
            contactManager = new ContactManager();
            ClearContacts();

            automation = new UIA3Automation();
            app = FlaUI.Core.Application
                .Launch(@"..\..\..\..\AddressBook\AddressBook.exe");
        }

        [TearDown]
        public void Clear()
        {
            app?.Close();
            automation?.Dispose();

            ClearContacts();
        }

        [Test]
        public void ReadContactList()
        {
            var contacts = GenerateContacts(10).ToList();
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            var contactNames = mainWindow.Contacts.Select(c => c.ContactName);
            var expectedNames = contacts.Select(c => c.Names.First().FormattedName);

            contactNames.Should().BeEquivalentTo(expectedNames);
        }

        [Test]
        public void OpenFirstContact()
        {
            var contacts = GenerateContacts(10).ToList();
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            var contactWindow = mainWindow.Contacts.First().OpenContactWindow();
            contactWindow.Close();
        }

        [Test]
        public void Create_New_Contact()
        {
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            Contact cont = new Contact();

            mainWindow.FindFirstDescendant(c => c.ByText("New Contact")).Click();
            Thread.Sleep(1000);

            var contactWindow = app.GetMainWindow(automation).As<ContactWindow>();
            Bogus.Person person = new Bogus.Person();

            var personName = new Microsoft.Communications.Contacts.Name(person.FirstName, "", person.LastName, NameCatenationOrder.GivenFamily);
            var personPhoneNumber = new PhoneNumber(person.Phone);
            var personAddress = new PhysicalAddress("", person.Address.Street, person.Address.City, person.Address.State, "", "", "", "");

            var nameBox = contactWindow.FindFirstDescendant(c => c.ByName("Name")).FindAllChildren();
            nameBox[1].AsTextBox().Text = person.FullName;
            nameBox[3].AsTextBox().Text = person.FirstName;
            nameBox[7].AsTextBox().Text = person.LastName;
            nameBox[9].AsTextBox().Text = person.UserName;

            contactWindow.FindFirstDescendant(c => c.ByName("Phone Numbers")).Click();

            Thread.Sleep(1000);

            var homePhoneBox = contactWindow.FindFirstDescendant(c => c.ByName("Home Phone Numbers")).FindAllChildren();
            homePhoneBox[1].AsTextBox().Text = person.Phone;

            contactWindow.FindFirstDescendant(c => c.ByName("Locations")).Click();
            
            Thread.Sleep(1000);

            var locationBox = contactWindow.FindFirstDescendant(c => c.ByName("Work Address")).FindAllChildren().First().FindAllChildren();
            locationBox[1].AsTextBox().Text = person.Address.Street;
            locationBox[3].AsTextBox().Text = person.Address.City;
            locationBox[5].AsTextBox().Text = person.Address.State;

            contactWindow.FindFirstDescendant(c => c.ByName("Save Changes")).Click();
            var contact = contactManager.GetContactCollection().First();

            contact.Names.Default.Should().BeEquivalentTo(personName);
            contact.PhoneNumbers.Default.Should().BeEquivalentTo(personPhoneNumber);
            contact.Addresses.Default.Should().BeEquivalentTo(personAddress);

            mainWindow.Close();
        }

        [Test]
        public void Change_Contact()
        {
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            Contact cont = new Contact();

            var contacts = GenerateContacts(1).ToList();

            var contactWindow = mainWindow.Contacts.First().OpenContactWindow();
            
            Bogus.Person person = new Bogus.Person();

            var personName = new Microsoft.Communications.Contacts.Name(person.FirstName, "", person.LastName, NameCatenationOrder.GivenFamily);
            var personPhoneNumber = new PhoneNumber(person.Phone);
            var personAddress = new PhysicalAddress("", person.Address.Street, person.Address.City, person.Address.State, "", "", "", "");

            var nameBox = contactWindow.FindFirstDescendant(c => c.ByName("Name")).FindAllChildren();
            nameBox[1].AsTextBox().Text = person.FullName;
            nameBox[3].AsTextBox().Text = person.FirstName;
            nameBox[7].AsTextBox().Text = person.LastName;
            nameBox[9].AsTextBox().Text = person.UserName;

            contactWindow.FindFirstDescendant(c => c.ByName("Phone Numbers")).Click();

            Thread.Sleep(1000);

            var homePhoneBox = contactWindow.FindFirstDescendant(c => c.ByName("Home Phone Numbers")).FindAllChildren();
            homePhoneBox[1].AsTextBox().Text = person.Phone;

            contactWindow.FindFirstDescendant(c => c.ByName("Locations")).Click();
            
            Thread.Sleep(1000);

            var locationBox = contactWindow.FindFirstDescendant(c => c.ByName("Work Address")).FindAllChildren().First().FindAllChildren();
            locationBox[1].AsTextBox().Text = person.Address.Street;
            locationBox[3].AsTextBox().Text = person.Address.City;
            locationBox[5].AsTextBox().Text = person.Address.State;

            contactWindow.FindFirstDescendant(c => c.ByName("Save Changes")).Click();
            var contact = contactManager.GetContactCollection().First();

            contact.Names.Default.Should().BeEquivalentTo(personName);
            contact.PhoneNumbers.Default.Should().BeEquivalentTo(personPhoneNumber);
            contact.Addresses.Default.Should().BeEquivalentTo(personAddress);

            mainWindow.Close();
        }

        [Test]
        public void Delete_Contact()
        {
            var contacts = GenerateContacts(5).ToList();
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            var expected = contactManager.GetContactCollection().Count();

            var contact = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("_contactPanel")).FindAllChildren(cf => cf.ByClassName("ListBoxItem"));
            contact[0].Click();

            mainWindow.Menu.FindFirstDescendant(c => c.ByAutomationId("_deleteContactButton")).Click();

            var result = contactManager.GetContactCollection().Count();

            result.Should().Be(expected-1);

            mainWindow.Close();
        }

        [Test]
        public void Find_Contact()
        {
            var contacts = GenerateContacts(5).ToList();
            var mainWindow = app.GetMainWindow(automation).As<MainWindow>();

            var person = new Bogus.Person();
            var contact = new Contact();
            Microsoft.Communications.Contacts.Name personName= new Microsoft.Communications.Contacts.Name(person.FirstName, "", person.LastName,NameCatenationOrder.GivenFamily);
            contact.Names.Add(personName);
            contactManager.AddContact(contact);

            var search = mainWindow.FindFirstDescendant(m => m.ByAutomationId("_wordwheel")).AsTextBox();
            search.Text = person.FirstName;

            Thread.Sleep(2000);

            var contactWindow = mainWindow.Contacts.First().OpenContactWindow();

            var expected = personName.GivenName + " " +personName.FamilyName;
            var result = contactWindow.FindFirstDescendant(c => c.ByAutomationId("_header")).FindFirstChild(c => c.ByClassName("TextBlock")).Name;

            result.Should().Be(expected);

            contactWindow.Close();
        }


        private void ClearContacts()
        {
            var contacts = contactManager.GetContactCollection();
            foreach (var contact in contacts)
            {
                contactManager.Remove(contact.Id);
            }
        }

        private IEnumerable<Contact> GenerateContacts(int count)
        {
            for (uint i = 0; i < count; i++)
            {
                var person = new Bogus.Person();

                var contact = new Contact();
                contact.Names.Add(new Microsoft.Communications.Contacts.Name(person.FirstName, "", person.LastName,
                    NameCatenationOrder.GivenFamily));

                contactManager.AddContact(contact);

                yield return contact;
            }

        }

    }
}