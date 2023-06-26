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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
