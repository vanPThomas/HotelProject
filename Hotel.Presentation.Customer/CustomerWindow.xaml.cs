using Hotel.Domain.Model;
using Hotel.Presentation.Customer.Model;
using System.Windows;

namespace Hotel.Presentation.Customer
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerUI CustomerUI { get; set; }

        public CustomerWindow(CustomerUI customerUI)
        {
            InitializeComponent();
            this.CustomerUI = customerUI;
            if (CustomerUI != null)
            {
                IdTextBox.Text = CustomerUI.Id.ToString();
                NameTextBox.Text = CustomerUI.Name;
                EmailTextBox.Text = CustomerUI.Email;
                PhoneTextBox.Text = CustomerUI.Phone;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerUI == null)
            {
                //Nieuw
                //wegschrijven
                //TODO nrofmembers
                Address address = new Address(
                    CityTextBox.Text,
                    StreetTextBox.Text,
                    ZipTextBox.Text,
                    HouseNumberTextBox.Text
                );
                CustomerUI = new CustomerUI(
                    NameTextBox.Text,
                    EmailTextBox.Text,
                    address.ToString(),
                    PhoneTextBox.Text,
                    0
                );
            }
            else
            {
                //Update
                //update DB
                CustomerUI.Email = EmailTextBox.Text;
                CustomerUI.Phone = PhoneTextBox.Text;
                CustomerUI.Name = NameTextBox.Text;
            }
            DialogResult = true;
            Close();
        }
    }
}
