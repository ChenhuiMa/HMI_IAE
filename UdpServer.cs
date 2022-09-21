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
        [Header("���ض˿�"), SerializeField]
        private int localPort;

        [Header("Ŀ��˿�"), SerializeField]
        private int endPort;

        [Header("�򿪽��չ���")]
        public bool openReceive = true;

        [Header("�򿪷��͹���")]
        public bool openSend = true;
        //������Socket����
        private Socket serverSocket;
        //udp�ͻ���socket
        private UdpClient client;
        private EndPoint epSender;
        private IPEndPoint endPoint;
        //�������ݵ��ַ���
        private byte[] ReceiveData = new byte[1024];
        //�����ļ�������
        public ConfigTest configTest;
        //��ʾ���յ��ź�
        public string receiveString = "";
        //��ӦIP��ַ
        private string serverIP;
        //�Ƿ���Խ����ַ���
        public bool isCanReceive = true;
        //���յ���������ʾTextMeshPro
        public TMP_Text reciveSpeed;
        public TMP_Text reciveSpeed1;
        public TMP_Text reciveTime;
        public TMP_Text reciveGearboxMode;
        public TMP_Text recivePower;
        public TMP_Text SetSpeed;
        public TMP_Text SetSpeed1;
        public TMP_Text PossessTimeText;
        public TMP_Text PossessDistanceText;
        //��������
        float floatstr;
        int intstr;
        public string speed;
        //�ƹ�
        public string left_indicator = "";
        public string right_indicator = "";
        public string dipped_lights = "";
        public string full_lights = "";
        public string fog_lights = "";
        public string rear_fog_lights = "";
        //��λ
        public string GearboxMode = "";
        public string showGear = "P";
        //ʱ��
        public string clock = "0:0";
        //����״̬
        public string EngineReady = "";
        //����
        public float BatteryStateOfCharge;
        //����
        public string Power;
        //���η���ʱ��
        public string possessHour;
        public string possessMinute;
        public string possessSecond;
        //���η���·��
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
        /// ��ʼ�������ļ�
        /// </summary>
        void BeginString()
        {
            localPort = int.Parse(configTest.dic["�˿ں�"]["����"]);
            endPort = int.Parse(configTest.dic["�˿ں�"]["��Ӧ"]);
            serverIP = configTest.dic["IP"]["ip0"];
            //  ToGameString = configTest.dic["UDP"]["������Ϸ"];
        }

        /// <summary>
        /// ��ʼ��Socket��Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator ToBeginSocket0()
        {
            yield return new WaitForSeconds(0.1f);
            ToBeginSocket();
        }

        /// <summary>
        /// ��ʼ��Socket
        /// </summary>
        void ToBeginSocket()
        {
            if (openReceive)
            {
                //������Socket��ʵ����
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //Socket�����������IP�Ͷ˿ڹ̶�
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, localPort));
                //�����Ķ˿ں͵�ַ
                epSender = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                //��ʼ�첽��������
                serverSocket.BeginReceiveFrom(ReceiveData, 0, ReceiveData.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveFromClients), epSender);
            }
            if (openSend)
            {
                client = new UdpClient();
                //Ŀ��˿ں͵�ַ
                endPoint = new IPEndPoint(IPAddress.Parse(serverIP), endPort);
            }
        }
        // Update is called once per frame
        void Update()
        {
            UdpControl();
        }

        /// <summary>
        /// �첽���أ���������
        /// </summary>
        /// <param name="iar"></param>
        void ReceiveFromClients(IAsyncResult iar)
        {
            int reve = serverSocket.EndReceiveFrom(iar, ref epSender);
            //���ݴ���
            string str = System.Text.Encoding.UTF8.GetString(ReceiveData, 0, reve);
            //�ѵõ������ݴ������ݴ�������
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
            //�����յ�������str
            else
            {
                //��ȡ����
                speed = str.Substring(0, 5);
                floatstr = float.Parse(speed);

                //�ƹ�
                left_indicator = str.Substring(5, 1);
                right_indicator = str.Substring(6, 1);
                dipped_lights = str.Substring(7, 1);
                full_lights = str.Substring(8, 1);
                fog_lights = str.Substring(9, 1);
                rear_fog_lights = str.Substring(10, 1);
                //��λ
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
                //ʱ��
                float hour = float.Parse(str.Substring(13, 2));
                string strhour;
                if (hour < 10) strhour = "0" + hour.ToString("F0");
                else strhour = hour.ToString("F0");
                float minute = float.Parse(str.Substring(15, 2));
                string strMins;
                if (minute < 10) strMins = "0" + minute.ToString("F0");
                else strMins = minute.ToString("F0");
                clock = strhour + ":" + strMins;
                //����
                EngineReady = str.Substring(17, 1);
                //���
                BatteryStateOfCharge = float.Parse(str.Substring(18, 4));
                //����
                Power = str.Substring(22, 4);
                //ADAS
                AD = str.Substring(26, 1);
                ACC = str.Substring(27, 1);
                LCC = str.Substring(28, 1);
                SetSpeedVal = str.Substring(29, 3);
                float floatspeed = float.Parse(SetSpeedVal);
                SetSpeedVal = floatspeed.ToString("F0");
                //ADASDistace = str.Substring(32,1);
                //���η���ʱ��
                string possessTime = str.Substring(33, 8);
                float floatTime = float.Parse(possessTime);
                int intTime = (int)floatTime;
                int possess_hour = intTime / 3600;
                possessHour = possess_hour.ToString("F0");
                int possess_minute = (intTime % 3600) / 60;
                possessMinute = possess_minute.ToString("F0");
                int possess_second = intTime % 3600 % 60;
                possessSecond = possess_second.ToString("F0");
                //���η���·��
                floatDistance += floatstr / 360000;
                //int intDistance = (int)floatDistance;
                possessDistance = floatDistance.ToString("F1");
            }
        }

        /// <summary>
        /// ��������������
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
        /// �ر�Socket
        /// </summary>
        public void SocketQuit()
        {
            if (serverSocket != null)
            {
                serverSocket.Close();
            }
        }

        /// <summary>
        /// Ӧ�ùر�ʱ�ر�Socket
        /// </summary>
        private void OnApplicationQuit()
        {
            SocketQuit();
        }

        /// <summary>
        /// ���رմ˶���ʱ�ر�Socket
        /// </summary>
        private void OnDisable()
        {
            SocketQuit();
        }

        /// <summary>
        /// �������ݵ�UIģ��TextMeshPro
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
