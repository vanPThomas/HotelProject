using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AddActivity
{
    public partial class MainWindow : Window
    {
        private ActivityManager activityManager;

        public MainWindow()
        {
            InitializeComponent();
            activityManager = new ActivityManager(RepositoryFactory.ActivityRepository);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            int organisationId = int.Parse(IdTextBox.Text);
            DateTime date = DatePicker.SelectedDate ?? DateTime.MinValue;
            int numberOfPlaces = int.Parse(NumberOfPlacesTextBox.Text);
            double adultTarif = double.Parse(AdultTarifTextBox.Text);
            double childTarif = double.Parse(ChildTarifTextBox.Text);
            double discount = double.Parse(DiscountTextBox.Text);
            int adultAge = int.Parse(AdultAgeTextBox.Text);
            string descriptionName = DescriptionNameTextBox.Text;
            string location = LocationTextBox.Text;
            int duration = int.Parse(DurationTextBox.Text);
            string descriptionText = DescriptionTextBox.Text;

            Tarif tarif = new Tarif(adultTarif, childTarif, discount, adultAge);
            Description description = new Description(
                descriptionName,
                location,
                duration,
                descriptionText
            );
            Activity activity = new Activity
            {
                Date = date,
                NumberOfPlaces = numberOfPlaces,
                Tarif = tarif,
                Description = description
            };

            activityManager.AddActivity(activity);

            MessageBox.Show(
                "Activity added successfully!",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions, e.g., show an error message
            //    MessageBox.Show(
            //        $"Error adding activity: {ex.Message}",
            //        "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error
            //    );
            //}
        }
    }
}
