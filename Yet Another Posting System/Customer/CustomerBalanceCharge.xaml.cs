using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Windows;
using System.Windows.Controls;
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for CustomerBalanceCharge.xaml
    /// </summary>
    public partial class CustomerBalanceCharge : Window
    {
        string username { get; set; }

        string creditCardChargeReport { get; set; } = "";

        public CustomerBalanceCharge(string username)
        {
            this.username = username;

            InitializeComponent();
            usernameLabel.Content = username;
        }

        public int[] StringToIntArray(string str)
        {
            int[] ints = new int[str.Length];

            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    ints[i] = int.Parse(str[i].ToString());
                }
            }
            catch (FormatException)
            {
                throw new Exception("Can't convert string to digits (possible wrong in credit card number)");
            }

            return ints;
        }

        public bool VerifyLuhn(TextBox textBox)
        {
            int[] ints = StringToIntArray(textBox.Text);
            int sum = 0;

            int multipliedNum;

            for (int i = 0; i < ints.Length; i++)
            {
                if ((ints.Length - i) % 2 == 0)
                {
                    multipliedNum = ints[i] * 2;

                    if (multipliedNum >= 10)
                    {
                        sum += (1 + multipliedNum % 10);
                    }
                    else
                    {
                        sum += multipliedNum;
                    }
                }

                else
                {
                    sum += ints[i];
                }
            }

            return (sum % 10 == 0);
        }

        private void chargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WPFUtils.AreTextBoxesEmpty(mainGrid))
            {
                throw new Exception("Please fill in all the data");
            }

            if (VerifyLuhn(creditNumberBox) == false)
            {
                throw new Exception("Credit card is not in the correct format");
            }

            WPFUtils.CheckCVV(cvvBox);
            WPFUtils.CheckMonth(monthBox);
            WPFUtils.CheckYear(yearBox);

            double currentUserBalance = App.usersDb.UserBalance(this.username);
            App.usersDb.ChangeUserBalance(username, currentUserBalance + int.Parse(amountBox.Text));

            this.creditCardChargeReport = $"{creditNumberBox.Text} {cvvBox.Text} {monthBox.Text} {yearBox.Text}";
            exportButton.IsEnabled = true;

            MessageBox.Show("The charge has been successfully done");
        }

        private void exportPDF(string username, string creditCardInfo, string amount)
        {
            //var renderer = new ChromePdfRenderer();
            //PdfDocument pdf = renderer.RenderHtmlAsPdf($"<h1>Balance charge info</h1> balance of {username} has been charged the amount of {amount} with the credit card info of {creditCardInfo}");
            //pdf.SaveAs("ChargeReport.pdf");

            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(20));

        page.Header()
            .Text("Balance Charge")
            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(x =>
            {
                x.Spacing(20);

                x.Item().Text($"balance of {username} has been charged the amount of {amount} with the credit card info of {creditCardInfo}");
                x.Item().Image(Placeholders.Image(200, 100));
            });
    });
})
.GeneratePdf("hello.pdf");


        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            exportPDF(username, creditCardChargeReport, amountBox.Text);
        }
    }



}