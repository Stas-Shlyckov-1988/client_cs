using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Policy;


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Uri url = new Uri("http://localhost/api.cgi");
            string s;
            using (HttpClient client = new HttpClient())
            {
                s = client.GetStringAsync(url).Result;
            }
            s = Regex.Replace(s, @"^\[", "");
            s = Regex.Replace(s, @"]$", "");

            Regex regex = new Regex(@"[A-Za-z0-9_\-]+");
            MatchCollection matches = regex.Matches(s);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    listData1.Items.Add(match.Value);
            }
            else
            {
                 MessageBox.Show("Совпадений не найдено");
            }

        }

    }
}