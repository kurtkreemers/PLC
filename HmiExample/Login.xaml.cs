#region using
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
#endregion
namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static bool loginAdmin;
        private static bool loginExpert;

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public Login()
        {
            InitializeComponent();
            passwordBox.Clear();
            foreach ( Users userName in Enum.GetValues(typeof(Users)))
            {
                comboboxUserLogin.Items.Add(userName);
            }
            comboboxUserLogin.SelectedItem = Users.USER;
           
        }

        
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {       
            if((e.Key == Key.Return) && ((comboboxUserLogin.SelectedItem.ToString() == Users.ADMIN.ToString())&&(passwordBox.Password == Properties.Settings.Default.AdminPass)))
            {
                LoginUsers(Users.ADMIN);
               this.Close();
            }
            else if((comboboxUserLogin.SelectedItem.ToString() == Users.EXPERT.ToString() )&&(passwordBox.Password == Properties.Settings.Default.ExpertPass))
            {
                LoginUsers(Users.EXPERT);
                this.Close();
            }
            else if ((comboboxUserLogin.SelectedItem.ToString() == Users.USER.ToString())&&(e.Key == Key.Return))
            {
                LoginUsers(Users.USER);
                this.Close();
            }
            else if (e.Key == Key.Return)
            {
                Log.writeLog("LOGIN Failure " + comboboxUserLogin.SelectedItem.ToString() + " : wrong password" );      
                passwordBox.Clear();
                throw new Exception(TXT.PswNotValid);               
            }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButton.OK, MessageBoxImage.Information); ;
            }
       
        }
        public static void LoginUsers(Users user)
        {
            if (user == Users.ADMIN)
            {
                loginAdmin = true;
                loginExpert = false;
            }
            else if (user == Users.EXPERT)
            {
                loginExpert = true;
                loginAdmin = false;
            }
            else
            {
                loginExpert = false;
                loginAdmin = false;
            }
            Log.writeLog("LOGIN " + user.ToString());

        }

        public static Users LoginStatus()
        {
            if (loginAdmin)                        
                return Users.ADMIN;        
            else if (loginExpert)
                return Users.EXPERT;
            else
                return Users.USER;
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
        public static void IdleControl()
        {
            if ((GetLastInputTime() > 9)&& (loginExpert))           
                LoginUsers(Users.USER);
        }
        
        

        
    }
}
