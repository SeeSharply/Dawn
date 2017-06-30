using GalaSoft.MvvmLight;
using System;
using System.Net.NetworkInformation;

namespace Dawn.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private NetworkInterface[] nicArr;  //网卡集合

        private string upspeed="0";
        public string UpSpeed
        {
            get
            {
                return GenrateSpeed(upspeed);
            }
            set
            {
                upspeed = value;
                base.RaisePropertyChanged("UpSpeed");
            }
        }

        private string downspeed="0";
        public string DownSpeed
        {
            get
            {
                return GenrateSpeed(downspeed);
            }
            set
            {
                downspeed = value;
                base.RaisePropertyChanged("DownSpeed");
            }
        }

        public long send { get; set; }

        public long down { get; set; }

        private string GenrateSpeed(string speed)
        {
            var spStr = string.Empty;
            if (string.IsNullOrEmpty(speed))
            {
                speed = "0";
            }
            var speedNum = Convert.ToDecimal(speed);
            if (speedNum > 1000)
            {
                spStr = (speedNum / 1000) + "m/s";
            }
            else
            {
                spStr = (speedNum) + "kb/s";
            }
            return spStr;
        }

        System.Windows.Threading.DispatcherTimer dtimer;
        private void dtimer_Tick(object sender, EventArgs e)
        {
            DoGetNetworkInfo();
        }

        private void DoGetNetworkInfo()
        {
            UpdateNetworkInterface();
        }

        /// <summary>
        /// 初始化网卡
        /// </summary>
        private void InitNetworkInterface()
        {
            nicArr = NetworkInterface.GetAllNetworkInterfaces();
        }
        /// <summary>
        /// 获取网络数据并更新到UI
        /// </summary>
        private void UpdateNetworkInterface()
        {
            NetworkInterface nic = nicArr[0];
            IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();

            int bytesSentSpeed = (int)(interfaceStats.BytesSent-send) ;
            
            if (send!=0)
            {
                UpSpeed = (bytesSentSpeed / 1024).ToString();
            }
            send = interfaceStats.BytesSent;
            int bytesReceivedSpeed = (int)(interfaceStats.BytesReceived -down);
            if (down!=0)
            {
                DownSpeed = (bytesReceivedSpeed / 1024).ToString();
            }
            down = interfaceStats.BytesReceived;

            //更新控件
            //txtbSpeed.Text = nic.Speed.ToString();
            //txtbInterfaceType.Text = nic.NetworkInterfaceType.ToString();
            //txtbSpeed.Text = nic.Speed.ToString();
            //txtbBytesReceived.Text = interfaceStats.BytesReceived.ToString();
            //txtbBytesSent.Text = interfaceStats.BytesSent.ToString();
            //txtbSentSecond.Text = bytesSentSpeed.ToString() + " KB/s";
            //txtbReceivedSecond.Text = bytesReceivedSpeed.ToString() + " KB/s";

        }
        public MainViewModel()
        {
            InitNetworkInterface();
            if (dtimer == null)
            {
                dtimer = new System.Windows.Threading.DispatcherTimer();
                dtimer.Interval = TimeSpan.FromSeconds(1);
                dtimer.Tick += dtimer_Tick;
            }
            dtimer.Start();
        }
    }
}