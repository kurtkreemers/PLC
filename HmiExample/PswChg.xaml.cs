using System;
using S7NetWrapper;
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
using System.Runtime.InteropServices;
using HmiExample.enums;
using HmiExample.Resources;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class PswChg : Window
    {
        public PswChg()
        {
            InitializeComponent();
            passwordBox1.Clear();
            passwordBox2.Clear();
            passwordBox3.Clear();
            foreach (Users userName in  Enum.GetValues(typeof(Users)))
            {
               if(userName != Users.USER)
               {
                   comboboxUserLogin.Items.Add(userName);
               }
            }              
        }


        private void btnChgPswOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBox2.Password != passwordBox3.Password)
                {                   
                    passwordBox1.Clear();
                    passwordBox2.Clear();
                    passwordBox3.Clear();
                   throw new Exception(TXT.CompPswNok);
                }
                else if ((comboboxUserLogin.SelectedItem.ToString() == Users.ADMIN.ToString()) && (passwordBox1.Password == Properties.Settings.Default.AdminPass))
                {
                    HmiExample.Properties.Settings.Default.AdminPass = passwordBox3.Password;
                    this.Close();
                    throw new Exception(TXT.ChangePswOk);

                }
                else if ((comboboxUserLogin.SelectedItem.ToString() == Users.EXPERT.ToString()) && (passwordBox1.Password == Properties.Settings.Default.ExpertPass))
                {
                    HmiExample.Properties.Settings.Default.AdminPass = passwordBox3.Password;                 
                    this.Close();
                    throw new Exception(TXT.ChangePswOk);
                }
                HmiExample.Properties.Settings.Default.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButton.OK, MessageBoxImage.Information); ;
            }
           
        }
        private void passwordBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((comboboxUserLogin.SelectedItem.ToString() == Users.ADMIN.ToString()) && (passwordBox1.Password != Properties.Settings.Default.AdminPass))
            {
                MessageBox.Show(TXT.PswNotValid, "", MessageBoxButton.OK, MessageBoxImage.Information);
                passwordBox1.Clear();
            }
            else if ((comboboxUserLogin.SelectedItem.ToString() == Users.EXPERT.ToString()) && (passwordBox1.Password != Properties.Settings.Default.ExpertPass))
            {
                MessageBox.Show(TXT.PswNotValid, "", MessageBoxButton.OK, MessageBoxImage.Information);
                passwordBox1.Clear();
            }
        }

       
        

       

        
        
    }
}
