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
using System.Diagnostics;


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
                int i = 0;
                string row = "";
                foreach (Match match in matches)
                {
                    if (++i == 3) continue;
                    if (i < 4) row += match.Value + " ";
                    else
                    {
                        i = 0;
                        listData1.Items.Add(row);
                        row = "";
                    }
                    
                }
                    
            }
            else
            {
                 MessageBox.Show("Совпадений не найдено");
            }

        }

        private void StaffSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Selected: " + e.AddedItems[0].ToString());
            Regex item = new Regex(@"^\d+");
            MatchCollection itemIds = item.Matches(e.AddedItems[0].ToString());
            if (itemIds.Count > 0)
            {
                foreach (Match itemId in itemIds)
                {
                    // match.Value
                    Uri url = new Uri("http://localhost/api.cgi?id=" + itemId.Value);
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
                        int i = 0;
                        foreach (Match match in matches)
                        {
                            // match.Value
                            switch(i++)
                            {
                                case 0:
                                    id.Text = match.Value;
                                    break;
                                case 1:
                                    name.Text = match.Value;
                                    break;
                                case 2:
                                    position.Text = match.Value;
                                    break;
                                case 3:
                                    date.Text = match.Value;
                                    break;
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Совпадений не найдено");
                    }

                }

            }
            else
            {
                MessageBox.Show("Совпадений не найдено");
            }
        }


        private void OnButtonSave(object sender, RoutedEventArgs e)
        {
            if (id.Text == "")
            {
                MessageBox.Show("Выберите элемент.");
                return;
            }
            string urlPath = "http://localhost/api.cgi?edit=1&id=" 
                + id.Text + "&name=" + name.Text + "&position=" + position.Text + "&birthday=" + date.Text;
            Uri url = new Uri(urlPath);
            string s;
            using (HttpClient client = new HttpClient())
            {
                s = client.GetStringAsync(url).Result;
            }
            MessageBox.Show("Вы сохранили запись.");
        }

    }
}