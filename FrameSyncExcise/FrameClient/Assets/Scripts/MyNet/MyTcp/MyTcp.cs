using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class MyTcp {

    private static MyTcp singleInstance;
    private static readonly object padlock = new object();

    public static MyTcp Instance
    {
        get
        {
            lock (padlock)  // 加锁保证单例唯一
            {
                if (singleInstance == null)
                {
                    singleInstance = new MyTcp();
                }
                return singleInstance;
            }
        }
    }

    private Socket clientSocket;

    public bool isRun = false;

    private Action<bool> ac_connect;
    public void ConnectServer(string inputIp, Action<bool> result)
    {
        //设定服务器IP地址  
        ac_connect = result;
        IPAddress ip;
        bool isRight = IPAddress.TryParse(inputIp, out ip);

        if (!isRight)
        {
            Debug.Log("无效地址......" + ip);
            result(false);
            return;
        }

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint _endpoint = new IPEndPoint(ip, NetConfig.TcpServerPort);
        Debug.Log("开始连接tcp~");
        clientSocket.BeginConnect(_endpoint, requestConnectCallBack, clientSocket);
    }

    private void requestConnectCallBack(IAsyncResult iar)
    {
        try
        {
            //还原原始的TcpClient对象
            Socket client = (Socket)iar.AsyncState;
            //
            client.EndConnect(iar);

            Debug.Log("连接服务器成功:" + client.RemoteEndPoint.ToString());
            isRun = true;

            NetGlobal.Instance().AddAction(() =>
            {
                if (ac_connect != null)
                {
                    ac_connect(true);
                }
            });


            Thread myThread = new Thread(ReceiveMessage);
            myThread.Start();
        }
        catch (Exception e)
        {
            NetGlobal.Instance().AddAction(() =>
            {
                if (ac_connect != null)
                {
                    ac_connect(false);
                }
            });

            Debug.Log("tcp连接异常:" + e.Message);
        }
    }

    private byte[] result = new byte[1024];

    private void ReceiveMessage()
    {
        while(isRun)
        {
            try
            {
                if (!clientSocket.Connected)
                {
                    throw new Exception("tcp 客户端关闭了...");
                }

                //通过clientsocket 接收数据
                int _size = clientSocket.Receive(result);
                if (_size <= 0)
                {
                    throw new Exception("客户端关闭了2...");
                }

                byte packMessageId = result[PackageConstant.PackMessageIdOffset];     //消息id (1个字节)
                Int16 packlength = BitConverter.ToInt16(result, PackageConstant.PacklengthOffset);  //消息包长度 (2个字节)
                int bodyDataLenth = packlength - PackageConstant.PacketHeadLength;  // 计算包体长度
                byte[] bodyData = new byte[bodyDataLenth];
                Array.Copy(result, PackageConstant.PacketHeadLength, bodyData, 0, bodyDataLenth);

                TCPPB.Instance().AnalyzeMessage((PBCommon.SCID)packMessageId, bodyData);

            }
            catch(Exception e)
            {
                Debug.Log("接收服务端数据异常:" + e.Message);
                EndClient();
                break;
            }
        }
    }

    public void EndClient()
    {
        isRun = false;

        if (clientSocket != null)
        {
            try
            {
                clientSocket.Close();
                clientSocket = null;

                Debug.Log("tcp连接");
            }
            catch(Exception e)
            {
                Debug.Log("关闭TCP连接异常：" + e.Message);
            }
        }
    }

    public void SendMessage(byte[] _mes)
    {
        if (isRun)
        {
            try
            {
                clientSocket.Send(_mes);
            }
            catch (Exception ex)
            {
                EndClient();
                Debug.Log("发送数据异常:" + ex.Message);
            }
        }
    }
}
