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

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static bool loginAdmin;
        public static bool loginExpert;
        private static List<string> loginUser = new List<string>{"USER","EXPERT","ADMIN"} ;
        public Login()
        {
            InitializeComponent();
            passwordBox.Clear();
            foreach (string passw in loginUser)
            {
                comboboxUserLogin.Items.Add(passw);
            }
            comboboxUserLogin.SelectedItem = loginUser[0];
           
        }

        
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {          



            if((e.Key == Key.Return) && ((comboboxUserLogin.SelectedItem.ToString() == loginUser[2])&&(passwordBox.Password == Properties.Settings.Default.AdminPass)))
            {
               loginAdmin = true;
               loginExpert = false;
               this.Close();
            }
            else if((comboboxUserLogin.SelectedItem.ToString() == loginUser[1])&&(passwordBox.Password == Properties.Settings.Default.ExpertPass))
            {
                loginExpert = true;
                loginAdmin = false;
                this.Close();
            }
            else if ((comboboxUserLogin.SelectedItem.ToString() == loginUser[0])&&(e.Key == Key.Return))
            {
                loginExpert = false;
                loginExpert = false;
                this.Close();
            }
            else if (e.Key == Key.Return)
            {
                MessageBox.Show("The password you entered\n is not valid!!!","",MessageBoxButton.OK,MessageBoxImage.Information);
                passwordBox.Clear();
            }
       
        }
        public static string LoginStatus()
        {
            if (loginAdmin)
                return loginUser[2];
            else if (loginExpert)
                return loginUser[1];
            else
                return loginUser[0];
        }

        
    }
}
