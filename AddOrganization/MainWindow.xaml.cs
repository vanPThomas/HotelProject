using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;
using System;
using System.Windows;

namespace AddOrganization
{
    public partial class MainWindow : Window
    {
        private OrganisationManager organisationManager;

        public MainWindow()
        {
            InitializeComponent();
            organisationManager = new OrganisationManager(RepositoryFactory.OrganisationRepository);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Extract values from textboxes
                int id = int.Parse(IdTextBox.Text);
                string name = NameTextBox.Text;
                string city = CityTextBox.Text;
                string zip = ZipTextBox.Text;
                string email = EmailTextBox.Text;
                string street = StreetTextBox.Text;
                string phone = PhoneTextBox.Text;
                string houseNumber = HouseNumberTextBox.Text;

                Address address = new Address(city, street, zip, houseNumber);
                ContactInfo contactInfo = new ContactInfo(email, phone, address);
                Organisation organisation = new Organisation(id, name, contactInfo);

                organisationManager.AddOrganisation(organisation);

                MessageBox.Show("Organisation added successfully!");
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., parsing errors, etc.)
                MessageBox.Show($"Error adding organisation: {ex.Message}");
            }
        }
    }
}
