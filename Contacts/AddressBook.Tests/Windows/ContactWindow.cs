using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Tests.Windows
{
    public class ContactWindow : Window
    {
        private IEnumerable<AutomationElement> nameBox =>
            FindFirstDescendant(n => n.ByName("Name")).FindAllChildren();
        private TextBox formattedNameBox =>
            nameBox.ElementAt(1).AsTextBox();
        private TextBox firstNameBox =>
            nameBox.ElementAt(3).AsTextBox();
        private TextBox lastNameBox =>
            nameBox.ElementAt(7).AsTextBox();
        private TextBox nickNameBox =>
            nameBox.ElementAt(9).AsTextBox();

        private IEnumerable<AutomationElement> homePhoneBox =>
            FindFirstDescendant(h => h.ByName("Home Phone Numbers")).FindAllChildren();
        private TextBox phoneBox =>
            homePhoneBox.ElementAt(1).AsTextBox();

        private IEnumerable<AutomationElement> workAddressBox =>
            FindFirstDescendant(c => c.ByName("Work Address")).FindAllChildren().First().FindAllChildren();
        private TextBox streetBox =>
            workAddressBox.ElementAt(1).AsTextBox();
        private TextBox cityBox =>
            workAddressBox.ElementAt(3).AsTextBox();
        private TextBox stateBox =>
            workAddressBox.ElementAt(5).AsTextBox();

        public ContactWindow(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public string name =>
            FindFirstDescendant(c => c.ByAutomationId("_header")).FindFirstChild(c => c.ByClassName("TextBlock")).Name;

        public Button phoneNumbersButton =>
            FindFirstDescendant(c => c.ByName("Phone Numbers")).AsButton();

        public Button locationsButton =>
            FindFirstDescendant(c => c.ByName("Locations")).AsButton();

        public Button saveChangesButton =>
            FindFirstDescendant(c => c.ByName("Save Changes")).AsButton();

        public string formattedName
        {
            set { formattedNameBox.Text = value; }
        }

        public string firstName
        {
            set { firstNameBox.Text = value; }
        }

        public string lastName
        {
            set { lastNameBox.Text = value; }
        }

        public string nickName
        {
            set { nickNameBox.Text = value; }
        }

        public string phone
        {
            set { phoneBox.Text = value; }
        }

        public string street
        {
            set { streetBox.Text = value; }
        }

        public string city
        {
            set { cityBox.Text = value; }
        }

        public string state
        {
            set { stateBox.Text = value; }
        }

    }
}
