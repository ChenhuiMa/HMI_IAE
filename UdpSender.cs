using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UI;

public class UdpSender : MonoBehaviour
{

    //����Ĭ�϶���˽�еĳ�Ա
    //Socket socketSend; //����socket
    UdpClient socketSend;
    IPEndPoint ipSend; //�ͻ��˶˿�

    Socket socketReceive; //����socket
    IPEndPoint ipReceive; //����˶˿�
    List<EndPoint> clientEnds; //�ͻ���

    string recvStr; //���յ��ַ���
    string sendStr; //���͵��ַ���
    byte[] recvData = new byte[1024]; //���յ����ݣ�����Ϊ�ֽ�
    byte[] sendData = new byte[1024]; //���͵����ݣ�����Ϊ�ֽ�
    int recvLen; //���յ����ݳ���
    Thread connectThread; //�����߳� 

    public GameObject DataCollector;
    public Toggle ADAS;
    public Toggle ACC;
    public Toggle LCC;
    public Toggle FrontWiper;
    public Toggle RearWiper;
    public Button AllLightsOff;



    //��ʼ��
    void InitSocket()
    {
        //�������ӵķ�����ip�Ͷ˿ڣ������Ǳ���ip����������������
        ipSend = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 7777);
        //ipSend = new IPEndPoint(IPAddress.Parse("192.168.1.200"),50000);
        //�����׽�������,�����߳��ж���
        socketSend = new UdpClient();
        //��������
        //���������˿�,�����κ�IP
        ipReceive = new IPEndPoint(IPAddress.Any, 23333);
        //�����׽�������,�����߳��ж���
        socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //�������Ҫ��ip
        socketReceive.Bind(ipReceive);
        //����ͻ���
        IPEndPoint senderReceive = new IPEndPoint(IPAddress.Any, 0);
        clientEnds = new List<EndPoint>();
        //����һ���߳����ӣ�����ģ��������߳̿���
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    /// <summary>
    /// UDP�������ݻ�������
    /// </summary>
    // <param name="sendStr"></param>
    void SocketSend(string sendStr)
    {
        //��շ��ͻ���
        sendData = new byte[1024];
        //��������ת��
        sendData = Encoding.UTF8.GetBytes(sendStr);

        //���͸����з����
        socketSend.Send(sendData, sendData.Length, ipSend);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void ToSendString()
    {
        string strTemp = DataCollector.gameObject.GetComponent<DataCollector>().SendData;
        SocketSend(strTemp);
    }

    //����������
    void SocketReceive()
    {
        //�������ѭ��
        while (true)
        {
            //��data����
            recvData = new byte[1024];
            //��ȡ�ͻ��ˣ���ȡ�ͻ������ݣ������ø��ͻ��˸�ֵ
            EndPoint clientEnd = new IPEndPoint(IPAddress.Any, 0);
            recvLen = socketReceive.ReceiveFrom(recvData, ref clientEnd);
            clientEnds.Add(clientEnd);
            //������յ�������
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            Debug.Log(recvStr);



        }
    }

    //���ӹر�
    void SocketQuit()
    {
        //�ر��߳�
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //���ر�socket
        if (socketReceive != null)
            socketReceive.Close();
        if (socketSend != null)
            socketSend.Close();
    }

    // Use this for initialization
    void Start()
    {
        InitSocket(); //�������ʼ��
    }
    void Update()
    {
        ToSendString();
        string channel107 = "0";
        string channel108 = "0";
        if (recvStr != null)
        {
            channel107 = recvStr.Substring(0, 1);
            channel108 = recvStr.Substring(1, 1);
            if (channel107 == "1")
            {
                ADAS.gameObject.GetComponent<Toggle>().isOn = false;
                ACC.gameObject.GetComponent<Toggle>().isOn = false;
                LCC.gameObject.GetComponent<Toggle>().isOn = false;
            }
            if (channel108 == "1")
            {
                ADAS.gameObject.GetComponent<Toggle>().isOn = false;
                LCC.gameObject.GetComponent<Toggle>().isOn = false;
            }
            if (recvStr == "********")
            {
                ADAS.gameObject.GetComponent<Toggle>().isOn = false;
                ACC.gameObject.GetComponent<Toggle>().isOn = false;
                LCC.gameObject.GetComponent<Toggle>().isOn = false;
                FrontWiper.gameObject.GetComponent<Toggle>().isOn = true;
                RearWiper.gameObject.GetComponent<Toggle>().isOn = true;
                AllLightsOff.onClick.Invoke();

            }
        }

    }
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
