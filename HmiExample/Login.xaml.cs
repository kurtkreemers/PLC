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
        public static string loginAdminString;
        private string adminPsw = "1234";
        public Login()
        {
            InitializeComponent();
            passwordBox.Clear();
           
        }

        
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {          

            if((e.Key == Key.Return) && (passwordBox.Password == adminPsw))
            {
               loginAdmin = true;
               loginAdminString = "ADMIN";
               this.Close();
            }
            else if (e.Key == Key.Return)
            {
                MessageBox.Show("The password you entered\n is not valid!!!","",MessageBoxButton.OK,MessageBoxImage.Information);
                passwordBox.Clear();
            }
       
        }

        
    }
}
