using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yet_Another_Posting_System
{
    public static class WPFUtils
    {
        public static bool AreTextBoxesEmpty(Panel stackPanel)
        {
            bool isEmpty = false;
            foreach (var item in stackPanel.Children)
            {
                if (item is TextBox text && string.IsNullOrEmpty(text.Text))
                {
                    isEmpty = true;
                    text.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (item is PasswordBox password && string.IsNullOrEmpty(password.Password))
                {
                    isEmpty = true;
                    password.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (item is Panel panelRecursion)
                {
                    isEmpty = AreTextBoxesEmpty(panelRecursion);
                }
            }

            return isEmpty;
        }

        public static void ChangeIsEnabled(Panel panel, bool isEnabled)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(panel); i++)
            {
                var child = VisualTreeHelper.GetChild(panel, i);

                if (child is UIElement uiElement)
                {
                    uiElement.IsEnabled = isEnabled;
                }

                if (child is Panel castedPanel)
                {
                    ChangeIsEnabled(castedPanel, isEnabled);
                }

            }
        }
        
        public static void ClearAll(Panel panel)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(panel); i++)
            {
                var child = VisualTreeHelper.GetChild(panel, i);

                if (child is Label label && label.Name != "")
                {
                    label.Content = "";
                }

                if (child is ComboBox comboBox)
                {
                    comboBox.SelectedItem = null;
                }

                if (child is Panel castedPanel)
                {
                    ClearAll(castedPanel);
                }

            }
        }

        public static void CheckName(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^[a-zA-Z]+$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The name needs to be from 3 to 32 characters");
            }
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        public static void CheckEmail(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^\S{3,32}@\S{3,32}\.\S{2,3}$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The email isn't in the correct format");
            }
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        public static void CheckPhone(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^09\d{9}$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The phone isn't in the correct format");
            }
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        public static void CheckPassword(PasswordBox passwordBox)
        {
            if (!Regex.IsMatch(passwordBox.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$"))
            {
                passwordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The password isn't in the correct format");
            }
            passwordBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        public static void CheckEmployeeID(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^\d{2}9\d{2}$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The Employee ID isn't in the correct format");
            }
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        public static void CheckCustomerID(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^00\d{8}$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The Customer ID isn't in the correct format");
            }
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
        }
    }
}
