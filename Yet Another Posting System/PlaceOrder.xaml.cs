using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            }
            else
            {
                WPFUtils.EnableAll(mainGrid);
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
                totalCost *= Math.Pow(1.2, (Double.Parse(weightBox.Text) - 0.5) % 0.5);
            }

            return totalCost;
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            costBlock.Text = CalculateCost().ToString();
        }
    }
}
