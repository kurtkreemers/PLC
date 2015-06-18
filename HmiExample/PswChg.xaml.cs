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
            
                comboboxUserLogin.Items.Add(Users.ADMIN);
                comboboxUserLogin.Items.Add(Users.EXPERT);
            
            comboboxUserLogin.SelectedItem = Users.USER;
           
        }    

        
    }
}
