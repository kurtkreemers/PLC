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
using TwinCAT.Ads;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TcAdsClient adsClient;
        public string AmsNetID;
        public int Port;
        public long IGrpIN;
        public long IGrpOUT; 
        public long[] IOffsIN = new long[7];
        public long[] IOffsOUT = new long[7];
        public bool[] IN1 = new bool[7];
        public bool[] OUT1 = new bool[7];
        public bool[] bTarbusy = new bool[2];
        public int[] iErrComm = new int[2];
        public bool bAdsErr;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Load();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;
            
        }

        private void Load()
        {
            try
            {
                Port = 801;

                IGrpIN = long.Parse("&H" + "0x3040020");
                IGrpOUT = long.Parse("&H" + "0x3040020");

                //Adressen ingangen
                IOffsIN[0] = long.Parse("&H" + "0x0");
                IOffsIN[1] = long.Parse("&H" + "0x1");
                IOffsIN[2] = long.Parse("&H" + "0x2");
                IOffsIN[3] = long.Parse("&H" + "0x3");
                IOffsIN[4] = long.Parse("&H" + "0x4");
                IOffsIN[5] = long.Parse("&H" + "0x5");
                IOffsIN[6] = long.Parse("&H" + "0x6");
                IOffsIN[7] = long.Parse("&H" + "0x7");

                //Adressen uitgangen
                IOffsOUT[0] = long.Parse("&H" + "0x0");
                IOffsOUT[1] = long.Parse("&H" + "0x1");
                IOffsOUT[2] = long.Parse("&H" + "0x2");
                IOffsOUT[3] = long.Parse("&H" + "0x3");
                IOffsOUT[4] = long.Parse("&H" + "0x4");
                IOffsOUT[5] = long.Parse("&H" + "0x5");
                IOffsOUT[6] = long.Parse("&H" + "0x6");
                IOffsOUT[7] = long.Parse("&H" + "0x7");

                bTarbusy[0] = false;
                bTarbusy[1] = false;
                //ads client aanmaken
                adsClient = new TcAdsClient();
            //connecteren met adsServer op locale AMSnetID en poort
            adsClient.Connect("172.16.17.3.1.1", Port);
            //adsClient.Connect(Port)



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Fout");
            }

        
        }
        System.Type type = typeof(Boolean);  

        public void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                IN1[0] = (bool)adsClient.ReadAny(IGrpIN, IOffsIN[0], type);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Fout");
            }
            
        }

    }
}
