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

namespace _2025_02_10_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        async void LoadData()
        {
            try
            {
                usersTable.DataContext = await API.GetUsers();
                peopleTable.DataContext = await API.GetPeople();
                if (API.ActiveUser != null)
                {
                    createForm.IsEnabled = true;
                    loginName.Content = API.ActiveUser.Name;
                    loginButton.Header = "_Logout";
                }
                else
                {
                    createForm.IsEnabled = false;
                    loginName.Content = "";
                    loginButton.Header = "_Login or register";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "API Error");
            }
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (API.ActiveUser == null)
            {
                new LoginWindow().ShowDialog();
            }
            else
            {
                API.ActiveUser = null;
            }
            LoadData();
        }
        private async void createPersonButton_Click(object sender, RoutedEventArgs e)
        {
            await API.NewPerson(nameField.Text, int.Parse(ageField.Text));
            LoadData();
        }
        private async void deletePersonButton_Click(object sender, RoutedEventArgs e)
        {
            Button s = sender as Button;
            Person person = s.DataContext as Person;
            await API.DeletePerson(person.id);
            LoadData();
        }
    }
}
