﻿using System;
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
    /// Interaction logic for CustomerBalance.xaml
    /// </summary>
    public partial class CustomerBalance : Window
    {
        string username { get; set; }
        public CustomerBalance(string username)
        {
            this.username = username;
            InitializeComponent();
            BalanceLabel.Content = App.usersDb.UserBalance(username);
            usernameBox.Content = username;
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerBalanceCharge balanceCharge = new CustomerBalanceCharge(username);
            balanceCharge.ShowDialog();
        }
    }
}
