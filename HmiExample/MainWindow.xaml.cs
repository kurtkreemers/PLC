#region Using
using HmiExample.PlcConnectivity;
using S7NetWrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;
using HmiExample.Properties;
using System.ComponentModel;
using System.Runtime.InteropServices;
using HmiExample.enums;

#endregion

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
       
       
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;        
            timer.IsEnabled = true;
            txtIpAddress.Text = Settings.Default.IpAddress;
            lblLogin.Text = Login.LoginStatus().ToString();
            Log.writeLog("Program started");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            btnConnect.IsEnabled = Plc.Instance.ConnectionState == ConnectionStates.Offline;
            btnDisconnect.IsEnabled = Plc.Instance.ConnectionState != ConnectionStates.Offline;
            lblConnectionState.Text = Plc.Instance.ConnectionState.ToString();
            ledMachineInRun.Fill = Plc.Instance.Db1.BitVariable0 ? Brushes.Green : Brushes.Gray;
            lblSpeed.Content = Plc.Instance.Db1.IntVariable;
            lblTemperature.Content = Plc.Instance.Db1.RealVariable;
            lblAutomaticSpeed.Content = Plc.Instance.Db1.DIntVariable;
            lblSetDwordVariable.Content = Plc.Instance.Db1.DWordVariable;
            // statusbar
            lblReadTime.Text = Plc.Instance.CycleReadTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
            UserAttributes();               
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Login.LoginStatus() != Users.USER)
                {
                    Plc.Instance.Connect(txtIpAddress.Text);
                    Settings.Default.IpAddress = txtIpAddress.Text;
                    Settings.Default.Save();
                }
                else
                {
                    noPermissionBox();
                }

            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Login.LoginStatus() != Users.USER)
                {
                    Plc.Instance.Disconnect();
                }
                else
                {
                    noPermissionBox();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Login.LoginStatus() == Users.ADMIN)
                {
                    Plc.Instance.Write(PlcTags.BitVariable, 1);
                }
                else
                {
                    MessageBox.Show("Administrator required!!!", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                }
               
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Login.LoginStatus() == Users.ADMIN)
                {
                    Plc.Instance.Write(PlcTags.BitVariable, 0);
                }
                else
                {
                    MessageBox.Show("Administrator required!!!","", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void txtSetRealVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            double realVar;
            bool canConvert = Double.TryParse(txtSetTemperature.Text, out realVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DoubleVariable, realVar);
            }
        }

        private void txtSetWordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            short wordVar;
            bool canConvert = short.TryParse(txtSetSpeed.Text, out wordVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.IntVariable, wordVar);
            }
        }

        private void txtSetDIntVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dintVar;
            bool canConvert = int.TryParse(txtSetAutomaticSpeed.Text, out dintVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DIntVariable, dintVar);
            }
        }

        private void txtSetSetDwordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort dwordVar;
            bool canConvert = ushort.TryParse(txtSetDwordVariable.Text, out dwordVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DwordVariable, dwordVar);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {          
                Login preview = new Login();
                preview.Owner = this;
                preview.ShowDialog();
                lblLogin.Text = Login.LoginStatus().ToString();               
        }
             
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Close the program?","", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
               if (Login.LoginStatus() != Users.ADMIN )
               {
                   noPermissionBox();
                   e.Cancel = true;
               }
               else
                   Log.writeLog("Program stopped");
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void UserAttributes()
        {
            Login.IdleControl();
            lblLogin.Text = Login.LoginStatus().ToString();          
        }
        private void noPermissionBox()
        {
            MessageBox.Show("You don't have the required permissions!!!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
       
    }
}
