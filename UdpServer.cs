using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using System.Text;


namespace ImmerUDP
{
    public class UdpServer : MonoBehaviour
    {
        [Header("本地端口"), SerializeField]
        private int localPort;

        [Header("目标端口"), SerializeField]
        private int endPort;

        [Header("打开接收功能")]
        public bool openReceive = true;

        [Header("打开发送功能")]
        public bool openSend = true;
        //服务器Socket对象
        private Socket serverSocket;
        //udp客户端socket
        private UdpClient client;
        private EndPoint epSender;
        private IPEndPoint endPoint;
        //接收数据的字符串
        private byte[] ReceiveData = new byte[1024];
        //配置文件管理类
        public ConfigTest configTest;
        //显示接收的信号
        public string receiveString = "";
        //对应IP地址
        private string serverIP;
        //是否可以接收字符串
        public bool isCanReceive = true;
        //接收到的数据显示TextMeshPro
        public TMP_Text reciveSpeed;
        public TMP_Text reciveSpeed1;
        public TMP_Text reciveTime;
        public TMP_Text reciveGearboxMode;
        public TMP_Text recivePower;
        public TMP_Text SetSpeed;
        public TMP_Text SetSpeed1;
        public TMP_Text PossessTimeText;
        public TMP_Text PossessDistanceText;
        //车速数据
        float floatstr;
        int intstr;
        public string speed;
        //灯光
        public string left_indicator = "";
        public string right_indicator = "";
        public string dipped_lights = "";
        public string full_lights = "";
        public string fog_lights = "";
        public string rear_fog_lights = "";
        //档位
        public string GearboxMode = "";
        public string showGear = "P";
        //时间
        public string clock = "0:0";
        //引擎状态
        public string EngineReady = "";
        //电量
        public float BatteryStateOfCharge;
        //功率
        public string Power;
        //本次仿真时间
        public string possessHour;
        public string possessMinute;
        public string possessSecond;
        //本次仿真路程
        public double floatDistance;
        public string possessDistance;
        //ADAS
        public string AD = "0";
        public string ACC = "0";
        public string LCC = "0";
        public string SetSpeedVal;


        // Use this for initialization
        void Start()
        {
            BeginString();
            StartCoroutine(ToBeginSocket0());
            floatDistance = 0;
      
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        void BeginString()
        {
            localPort = int.Parse(configTest.dic["端口号"]["本地"]);
            endPort = int.Parse(configTest.dic["端口号"]["对应"]);
            serverIP = configTest.dic["IP"]["ip0"];
            //  ToGameString = configTest.dic["UDP"]["进入游戏"];
        }

        /// <summary>
        /// 初始化Socket的协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ToBeginSocket0()
        {
            yield return new WaitForSeconds(0.1f);
            ToBeginSocket();
        }

        /// <summary>
        /// 初始化Socket
        /// </summary>
        void ToBeginSocket()
        {
            if (openReceive)
            {
                //服务器Socket对实例化
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //Socket对象服务器的IP和端口固定
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, localPort));
                //监听的端口和地址
                epSender = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                //开始异步接收数据
                serverSocket.BeginReceiveFrom(ReceiveData, 0, ReceiveData.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveFromClients), epSender);
            }
            if (openSend)
            {
                client = new UdpClient();
                //目标端口和地址
                endPoint = new IPEndPoint(IPAddress.Parse(serverIP), endPort);
            }
        }
        // Update is called once per frame
        void Update()
        {
            UdpControl();
        }

        /// <summary>
        /// 异步加载，处理数据
        /// </summary>
        /// <param name="iar"></param>
        void ReceiveFromClients(IAsyncResult iar)
        {
            int reve = serverSocket.EndReceiveFrom(iar, ref epSender);
            //数据处理
            string str = System.Text.Encoding.UTF8.GetString(ReceiveData, 0, reve);
            //把得到的数据传给数据处理中心
            serverSocket.BeginReceiveFrom(ReceiveData, 0, ReceiveData.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveFromClients), epSender);

            if (str == "********")
            {
                speed = "0";
                floatstr = 0;
                left_indicator = "0";
                right_indicator = "0";
                dipped_lights = "0";
                full_lights = "0";
                fog_lights = "0";
                rear_fog_lights = "0";
                showGear = "P";
                clock = "12:00";
                EngineReady = "0";
                BatteryStateOfCharge = 0;
                Power = "0";
                AD = "0";
                ACC = "0";
                LCC = "0";
                possessHour = "0";
                possessMinute = "0";
                possessSecond = "0";
                possessDistance = "0";

            }
            //处理收到的数据str
            else
            {
                //读取车速
                speed = str.Substring(0, 5);
                floatstr = float.Parse(speed);

                //灯光
                left_indicator = str.Substring(5, 1);
                right_indicator = str.Substring(6, 1);
                dipped_lights = str.Substring(7, 1);
                full_lights = str.Substring(8, 1);
                fog_lights = str.Substring(9, 1);
                rear_fog_lights = str.Substring(10, 1);
                //档位
                GearboxMode = str.Substring(11, 2);
                if (GearboxMode == "10")
                {
                    showGear = "D";
                }
                else if (GearboxMode == "20")
                {
                    showGear = "N";
                }
                else if (GearboxMode == "30")
                {
                    showGear = "R";
                }
                else
                {
                    showGear = "P";
                }
                //时间
                float hour = float.Parse(str.Substring(13, 2));
                string strhour;
                if (hour < 10) strhour = "0" + hour.ToString("F0");
                else strhour = hour.ToString("F0");
                float minute = float.Parse(str.Substring(15, 2));
                string strMins;
                if (minute < 10) strMins = "0" + minute.ToString("F0");
                else strMins = minute.ToString("F0");
                clock = strhour + ":" + strMins;
                //引擎
                EngineReady = str.Substring(17, 1);
                //电池
                BatteryStateOfCharge = float.Parse(str.Substring(18, 4));
                //功率
                Power = str.Substring(22, 4);
                //ADAS
                AD = str.Substring(26, 1);
                ACC = str.Substring(27, 1);
                LCC = str.Substring(28, 1);
                SetSpeedVal = str.Substring(29, 3);
                float floatspeed = float.Parse(SetSpeedVal);
                SetSpeedVal = floatspeed.ToString("F0");
                //ADASDistace = str.Substring(32,1);
                //本次仿真时间
                string possessTime = str.Substring(33, 8);
                float floatTime = float.Parse(possessTime);
                int intTime = (int)floatTime;
                int possess_hour = intTime / 3600;
                possessHour = possess_hour.ToString("F0");
                int possess_minute = (intTime % 3600) / 60;
                possessMinute = possess_minute.ToString("F0");
                int possess_second = intTime % 3600 % 60;
                possessSecond = possess_second.ToString("F0");
                //本次仿真路程
                floatDistance += floatstr / 360000;
                //int intDistance = (int)floatDistance;
                possessDistance = floatDistance.ToString("F1");
            }
        }

        /// <summary>
        /// 发送数据主函数
        /// </summary>
        /// <param name="dataStr"></param>
        //void Send(string dataStr)
        //{
        //    if (!openSend)
        //        return;
        //    byte[] SendData = System.Text.Encoding.UTF8.GetBytes(dataStr);
        //    client.Send(SendData, SendData.Length, endPoint);
        //}

        /// <summary>
        /// 关闭Socket
        /// </summary>
        public void SocketQuit()
        {
            if (serverSocket != null)
            {
                serverSocket.Close();
            }
        }

        /// <summary>
        /// 应用关闭时关闭Socket
        /// </summary>
        private void OnApplicationQuit()
        {
            SocketQuit();
        }

        /// <summary>
        /// 当关闭此对象时关闭Socket
        /// </summary>
        private void OnDisable()
        {
            SocketQuit();
        }

        /// <summary>
        /// 链接数据到UI模型TextMeshPro
        /// </summary>
        public void UdpControl()
        {
            if (isCanReceive)
            {
                intstr = Mathf.Abs((int)(Math.Round(floatstr)));
                speed = intstr.ToString("F0");
                if (Time.frameCount % 100 == 0)
                {
                    reciveSpeed.text = speed;
                    reciveSpeed1.text = speed + " km/h";
                    recivePower.text = Power + " kW/h";
                }

                reciveTime.text = clock;
                reciveGearboxMode.text = showGear;
                SetSpeed.text = SetSpeedVal + " km/h";
                SetSpeed1.text = SetSpeedVal + " km/h";

                PossessTimeText.text = possessMinute + " mins" + " " + possessSecond + " s";

                PossessDistanceText.text = possessDistance + " km";
 
            }
        }

    }
}
