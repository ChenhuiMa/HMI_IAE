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

    //以下默认都是私有的成员
    //Socket socketSend; //发送socket
    UdpClient socketSend;
    IPEndPoint ipSend; //客户端端口

    Socket socketReceive; //接收socket
    IPEndPoint ipReceive; //服务端端口
    List<EndPoint> clientEnds; //客户端

    string recvStr; //接收的字符串
    string sendStr; //发送的字符串
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节
    int recvLen; //接收的数据长度
    Thread connectThread; //连接线程 

    public GameObject DataCollector;
    public Toggle ADAS;
    public Toggle ACC;
    public Toggle LCC;
    public Toggle FrontWiper;
    public Toggle RearWiper;
    public Button AllLightsOff;



    //初始化
    void InitSocket()
    {
        //定义连接的服务器ip和端口，可以是本机ip，局域网，互联网
        ipSend = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 7777);
        //ipSend = new IPEndPoint(IPAddress.Parse("192.168.1.200"),50000);
        //定义套接字类型,在主线程中定义
        socketSend = new UdpClient();
        //定义服务端
        //定义侦听端口,侦听任何IP
        ipReceive = new IPEndPoint(IPAddress.Any, 23333);
        //定义套接字类型,在主线程中定义
        socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //服务端需要绑定ip
        socketReceive.Bind(ipReceive);
        //定义客户端
        IPEndPoint senderReceive = new IPEndPoint(IPAddress.Any, 0);
        clientEnds = new List<EndPoint>();
        //开启一个线程连接，必须的，否则主线程卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    /// <summary>
    /// UDP发送数据基本方法
    /// </summary>
    // <param name="sendStr"></param>
    void SocketSend(string sendStr)
    {
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.UTF8.GetBytes(sendStr);

        //发送给所有服务端
        socketSend.Send(sendData, sendData.Length, ipSend);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void ToSendString()
    {
        string strTemp = DataCollector.gameObject.GetComponent<DataCollector>().SendData;
        SocketSend(strTemp);
    }

    //服务器接收
    void SocketReceive()
    {
        //进入接收循环
        while (true)
        {
            //对data清零
            recvData = new byte[1024];
            //获取客户端，获取客户端数据，用引用给客户端赋值
            EndPoint clientEnd = new IPEndPoint(IPAddress.Any, 0);
            recvLen = socketReceive.ReceiveFrom(recvData, ref clientEnd);
            clientEnds.Add(clientEnd);
            //输出接收到的数据
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            Debug.Log(recvStr);



        }
    }

    //连接关闭
    void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭socket
        if (socketReceive != null)
            socketReceive.Close();
        if (socketSend != null)
            socketSend.Close();
    }

    // Use this for initialization
    void Start()
    {
        InitSocket(); //在这里初始化
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
