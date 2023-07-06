using System;
using System.Windows;
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for PlaceOrder.xaml
    /// </summary>
    public partial class PlaceOrder : Window
    {
        public PlaceOrder()
        {
            InitializeComponent();
        }

        private void idSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.usersDb.TypeUserExists(customerIDBox.Text, "Customer") == false)
            {
                CustomerSignup customerSignup = new CustomerSignup();
                customerSignup.ShowDialog();
                WPFUtils.ChangeIsEnabled(mainGrid, false);
            }
            else
            {
                WPFUtils.ChangeIsEnabled(mainGrid, true);
            }
        }

        private double CalculateCost()
        {
            double totalCost = 10;

            switch (contentBox.SelectedIndex)
            {
                case 1:
                    totalCost *= 1.5;
                    break;

                case 2:
                    totalCost *= 2;
                    break;

                default:
                    break;
            }

            switch (postTypeBox.SelectedIndex)
            {
                case 1:
                    totalCost *= 1.5;
                    break;

                default:
                    break;
            }

            if (expensiveCheckBox.IsChecked == true)
            {
                totalCost *= 2;
            }

            if (Double.Parse(weightBox.Text) > 0.5)
            {
                totalCost *= Math.Pow(1.2, (Double.Parse(weightBox.Text) - 0.5) / 0.5);
            }

            return totalCost;
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            costBlock.Text = CalculateCost().ToString();
        }

        private void placeOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(costBlock.Text))
            {
                throw new Exception("Calculate the cost first");
            }

            if (WPFUtils.AreTextBoxesEmpty(mainGrid))
            {
                throw new Exception("Please fill in all the data");
            }

            WPFUtils.CheckPhone(phoneBox);

            // TODO if balance not enough go to rechargin window
            if (App.usersDb.UserBalance(customerIDBox.Text) > CalculateCost())
            {
                App.usersDb.CreateOrder(customerIDBox.Text, senderAddressBox.Text, receiverAddressBox.Text, contentBox.SelectedIndex, postTypeBox.SelectedIndex, expensiveCheckBox.IsChecked == true ? 1 : 0, Double.Parse(weightBox.Text), phoneBox.Text, CalculateCost());
            }
            else
            {
                App.usersDb.CreateOrder(customerIDBox.Text, senderAddressBox.Text, receiverAddressBox.Text, contentBox.SelectedIndex, postTypeBox.SelectedIndex, expensiveCheckBox.IsChecked == true ? 1 : 0, Double.Parse(weightBox.Text), phoneBox.Text, CalculateCost());
            }
        }
    }
}
