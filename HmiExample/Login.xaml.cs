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

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static bool loginAdmin;
        private static bool loginExpert;
        private static List<string> loginVar = new List<string>{"USER","EXPERT","ADMIN"} ;

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public Login()
        {
            InitializeComponent();
            passwordBox.Clear();
            foreach (string passw in loginVar)
            {
                comboboxUserLogin.Items.Add(passw);
            }
            comboboxUserLogin.SelectedItem = loginVar[0];
           
        }

        
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {          



            if((e.Key == Key.Return) && ((comboboxUserLogin.SelectedItem.ToString() == loginVar[2])&&(passwordBox.Password == Properties.Settings.Default.AdminPass)))
            {
                LoginAdmin();
               this.Close();
            }
            else if((comboboxUserLogin.SelectedItem.ToString() == loginVar[1])&&(passwordBox.Password == Properties.Settings.Default.ExpertPass))
            {
                LoginExpert();
                this.Close();
            }
            else if ((comboboxUserLogin.SelectedItem.ToString() == loginVar[0])&&(e.Key == Key.Return))
            {
                LoginUser();
                this.Close();
            }
            else if (e.Key == Key.Return)
            {
                Log.writeLog("LOGIN Failure " + comboboxUserLogin.SelectedItem.ToString() + " : wrong password" );
                MessageBox.Show("The password you entered\n is not valid!!!","",MessageBoxButton.OK,MessageBoxImage.Information);
                passwordBox.Clear();
            }
       
        }
        public static void LoginAdmin()
        {
            loginAdmin = true;
            loginExpert = false;
            Log.writeLog("LOGIN " + loginVar[2]);
            
        }

        public static void LoginExpert()
        {
            loginExpert = true;
            loginAdmin = false;
            Log.writeLog("LOGIN " + loginVar[1]);
            
        }
        public static void LoginUser()
        {
            loginExpert = false;
            loginExpert = false;
            Log.writeLog("LOGIN " + loginVar[0]);
           
        }
        public static string LoginStatus()
        {
            if(loginAdmin)
                return loginVar[2];
            else if (loginExpert)
                return loginVar[1];
            else
                return loginVar[0];
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        public static int GetLastInputTime()
        {
            int nIdleTime = 0;
            LASTINPUTINFO liiInfo = new LASTINPUTINFO();
            liiInfo.cbSize = Marshal.SizeOf(liiInfo);
            liiInfo.dwTime = 0;
            int nEnvTicks = Environment.TickCount;
            if (GetLastInputInfo(ref liiInfo))
            {
                int nLastInputTick = (int)liiInfo.dwTime;
                nIdleTime = nEnvTicks - nLastInputTick;
            }       
            return ((nIdleTime > 0) ? (nIdleTime / 1000) : nIdleTime);
        }
        public static void loginIdleUser()
        {
            if ((GetLastInputTime() > 9)&& (loginExpert))           
                LoginUser();
        }
        
        

        
    }
}
