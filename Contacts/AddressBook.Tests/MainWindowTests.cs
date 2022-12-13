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

            var contactWindow = mainWindow.OpenNewContactWindow();
            Thread.Sleep(1000);

            Bogus.Person person = new Bogus.Person();

            var personName = new Microsoft.Communications.Contacts.Name(person.FirstName, "", person.LastName, NameCatenationOrder.GivenFamily);
            var personPhoneNumber = new PhoneNumber(person.Phone);
            var personAddress = new PhysicalAddress("", person.Address.Street, person.Address.City, person.Address.State, "", "", "", "");

            contactWindow.formattedName = person.FullName;
            contactWindow.firstName = person.FirstName;
            contactWindow.lastName = person.LastName;
            contactWindow.nickName = person.UserName;

            contactWindow.phoneNumbersButton.Click();

            Thread.Sleep(1000);

            contactWindow.phone = person.Phone;

            contactWindow.locationsButton.Click();
            
            Thread.Sleep(1000);

            contactWindow = mainWindow.OpenNewContactWindow();
            contactWindow.street = person.Address.Street;
            contactWindow.city = person.Address.City;
            contactWindow.state = person.Address.State;

            contactWindow.saveChangesButton.Click();

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
            contactWindow.formattedName = person.FullName;
            contactWindow.firstName = person.FirstName;
            contactWindow.lastName = person.LastName;
            contactWindow.nickName = person.UserName;

            contactWindow.phoneNumbersButton.Click();

            Thread.Sleep(1000);

            contactWindow.phone = person.Phone;

            contactWindow.locationsButton.Click();
            
            Thread.Sleep(1000);

            contactWindow.street = person.Address.Street;
            contactWindow.city = person.Address.City;
            contactWindow.state = person.Address.State;

            contactWindow.saveChangesButton.Click();

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

            var contact = mainWindow.Contacts.First();
            contact.Click();

            mainWindow.deleteButton.Click();

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

            mainWindow.search = person.FirstName;

            Thread.Sleep(6000); //Возможно задержку придется еще выше поставить поскольку не всегда открывает тот контакт (через отладку все работает)

            var contactWindow = mainWindow.Contacts.First().OpenContactWindow();

            var expected = personName.GivenName + " " +personName.FamilyName;

            contactWindow.name.Should().Be(expected);

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