using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace AddCustomer
{
    public partial class MainWindow : Window
    {
        private Customer _currentCustomer;
        private ObservableCollection<Member> _memberList;
        private CustomerManager _customerManager;

        public MainWindow()
        {
            InitializeComponent();
            _memberList = new ObservableCollection<Member>();
            MemberDataGrid.ItemsSource = _memberList;
            _customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate and create ContactInfo
                var contactInfo = new ContactInfo
                {
                    Email = EmailTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Address = new Address
                    {
                        City = CityTextBox.Text,
                        PostalCode = ZipTextBox.Text,
                        Street = StreetTextBox.Text,
                        HouseNumber = HouseNumberTextBox.Text
                    }
                };

                // Create Customer
                _currentCustomer = new Customer(
                    int.Parse(IdTextBox.Text),
                    NameTextBox.Text,
                    contactInfo
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error creating customer: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate and create Member
                var member = new Member
                {
                    Name = MemberNameTextBox.Text,
                    Birthday = DatePicker.SelectedDate.HasValue
                        ? DateOnly.FromDateTime(DatePicker.SelectedDate.Value.Date)
                        : DateOnly.MinValue
                };

                // Add Member to the list
                _memberList.Add(member);
                _currentCustomer.AddMember(member);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error adding member: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void FinishCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _customerManager.AddCustomer(_currentCustomer);

                MessageBox.Show(
                    "Customer saved to the database successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving customer to the database: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void GetCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get customer by ID from the database
                int customerId = int.Parse(IdTextBox.Text);
                _currentCustomer = _customerManager.GetCustomerById(customerId);

                // Display customer details
                NameTextBox.Text = _currentCustomer.Name;
                EmailTextBox.Text = _currentCustomer.Contact.Email;
                PhoneTextBox.Text = _currentCustomer.Contact.Phone;
                CityTextBox.Text = _currentCustomer.Contact.Address.City;
                ZipTextBox.Text = _currentCustomer.Contact.Address.PostalCode;
                StreetTextBox.Text = _currentCustomer.Contact.Address.Street;
                HouseNumberTextBox.Text = _currentCustomer.Contact.Address.HouseNumber;

                // Display members in the DataGrid
                _memberList.Clear();
                foreach (var member in _currentCustomer.Members)
                {
                    _memberList.Add(member);
                }

                MessageBox.Show(
                    "Customer details loaded successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error getting customer details: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update customer details
                _currentCustomer.Name = NameTextBox.Text;
                _currentCustomer.Contact.Email = EmailTextBox.Text;
                _currentCustomer.Contact.Phone = PhoneTextBox.Text;
                _currentCustomer.Contact.Address.City = CityTextBox.Text;
                _currentCustomer.Contact.Address.PostalCode = ZipTextBox.Text;
                _currentCustomer.Contact.Address.Street = StreetTextBox.Text;
                _currentCustomer.Contact.Address.HouseNumber = HouseNumberTextBox.Text;

                // Update members list
                _currentCustomer.Members.Clear();
                foreach (var member in _memberList)
                {
                    _currentCustomer.AddMember(member);
                }

                // Update customer in the database
                _customerManager.UpdateCustomer(_currentCustomer);

                MessageBox.Show(
                    "Customer updated successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating customer: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get customer by ID from the database
                int customerId = int.Parse(IdTextBox.Text);
                _customerManager.DeleteCustomer(customerId);

                MessageBox.Show(
                    "Customer deleted successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting customer: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
