using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yet_Another_Posting_System
{
    public static class WPFUtils
    {
        public static bool AreTextBoxesEmpty(StackPanel stackPanel)
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

                if (item is StackPanel stackPanelRecursion)
                {
                    isEmpty = AreTextBoxesEmpty(stackPanelRecursion);
                }
            }

            return isEmpty;
        }

        public static void CheckName(TextBox textBox)
        {
            if (Regex.IsMatch(textBox.Text, @"^[a-zA-Z]+$") == false)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The name needs to be from 3 to 32 characters");
            }
        }

        public static void CheckEmail(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^\S{3,32}@\S{3,32}\.\S{2,3}$") == false)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The email isn't in the correct format");
            }
        }

        public static void CheckPhone(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^09\d{9}$") == false)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The phone isn't in the correct format");
            }
        }

        public static void CheckPassword(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$") == false)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The password isn't in the correct format");
            }
        }

        public static void CheckEmployeeID(TextBox textBox)
        {
            if(!Regex.IsMatch(textBox.Text, @"^\d{2}9\d{2}$"))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                throw new Exception("The Employee ID isn't in the correct format");
            }
        }
    }
}
