using PBLogin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ServerTCP {

    private static readonly object stLockObj = new object();
    private static ServerTCP instance;
    public static ServerTCP Instance
    {
        get
        {
            lock (stLockObj)
            {
                if (instance == null)
                {
                    instance = new ServerTCP();
                }
            }
            return instance;
        }
    }

    static Socket serverSocket;
    private bool isRun = false;

    public void StartServer()
    {
        try
        {
            IPAddress ip = IPAddress.Parse(ServerGlobal.Instance.serverIp);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, ServerConfig.servePort));
            serverSocket.Listen(20);
            Debug.Log("启动监听" + serverSocket.LocalEndPoint.ToString() + "成功");
            isRun = true;

            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }
        catch (Exception ex)
        {
            Debug.LogFormat("服务器启动失败：{0}", ex);
        }
    }

    private void ListenClientConnect()
    {
        while(isRun)
        {
            try
            {
                Socket clientSocket = serverSocket.Accept();

                //开启处理消息线程
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
            catch (Exception ex)
            {
                Debug.Log("监听失败:" + ex.Message);
            }
        }
    }

    private Dictionary<string, Socket> dic_clientSocket = new Dictionary<string, Socket>();

    public string token;

    public void AddClientSocket(string _token, Socket clientSocket)
    {
        token = _token;
        dic_clientSocket[_token] = clientSocket;
    }

    private void ReceiveMessage(object clientSocket)
    {
        Socket myClientSocket = (Socket)clientSocket;
        Debug.Log(myClientSocket.RemoteEndPoint.ToString());
        //string _socketIp = myClientSocket.RemoteEndPoint.ToString().Split(':')[0];

        //dic_clientSocket[_socketIp] = myClientSocket;

        bool _flag = true;

        byte[] resultData = new byte[1024];

        while (isRun && _flag)
        {
            try
            {
                //				Debug.Log("_socketName是否连接:" + myClientSocket.Connected);
                //通过clientSocket接收数据  
                if (myClientSocket.Poll(1000, SelectMode.SelectRead))
                {
                    throw new Exception("客户端关闭了1~");
                }

                int _size = myClientSocket.Receive(resultData);

                if (_size <= 0)
                {
                    throw new Exception("客户端关闭了2~");
                }

                byte packMessageId = resultData[PackageConstant.PackMessageIdOffset];     //消息id (1个字节)
                Int16 packlength = BitConverter.ToInt16(resultData, PackageConstant.PacklengthOffset);  //消息包长度 (2个字节)
                int bodyDataLenth = packlength - PackageConstant.PacketHeadLength;
                byte[] bodyData = new byte[bodyDataLenth];
                Array.Copy(resultData, PackageConstant.PacketHeadLength, bodyData, 0, bodyDataLenth);


                TcpPB.Instance().AnalyzeMessage((PBCommon.CSID)packMessageId, bodyData, token, myClientSocket);

            }
            catch (Exception ex)
            {
                Debug.Log(token + "接收客户端数据异常: " + ex.Message);

                _flag = false;
                break;
            }
        }

        CloseClientTcp(token);
    }

    public void CloseClientTcp(string _socketIp)
    {
        try
        {
            if (dic_clientSocket.ContainsKey(_socketIp))
            {
                if (dic_clientSocket[_socketIp] != null)
                {
                    dic_clientSocket[_socketIp].Close();
                }
                dic_clientSocket.Remove(_socketIp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("关闭客户端..." + ex.Message);
        }

    }

    public void SendMessage(string _socketName, byte[] _mes)
    {
        Debug.Log("SendMessage aaa  ----- _socketName  " + _socketName);
        if (isRun)
        {
            try
            {
                dic_clientSocket[_socketName].Send(_mes);
            }
            catch (Exception ex)
            {
                Debug.Log("发数据给异常:" + ex.Message);
            }
        }

    }

    public void EndServer()
    {

        if (!isRun)
        {
            return;
        }

        isRun = false;
        try
        {
            foreach (var item in dic_clientSocket)
            {
                item.Value.Close();
            }

            dic_clientSocket.Clear();

            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }

            Debug.Log("tcp 服务器关闭成功...");
        }
        catch (Exception ex)
        {
            Debug.Log("tcp服务器关闭失败:" + ex.Message);
        }
    }
}


