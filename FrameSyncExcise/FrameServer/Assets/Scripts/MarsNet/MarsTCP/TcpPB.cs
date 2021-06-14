using PBCommon;
using PBLogin;
using PBMatch;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TcpPB {

    private static TcpPB singleInstance;
    public static TcpPB Instance()
    {
        // 如果类的实例不存在则创建，否则直接返回
        if (singleInstance == null)
        {
            singleInstance = new TcpPB();
        }
        return singleInstance;
    }

    private TcpPB()
    {

    }
    public void AnalyzeMessage(PBCommon.CSID messageId, byte[] bodyData, string _socketIp,Socket mySocket)
    {
        Debug.Log("收到客户端消息..."+messageId );
        switch (messageId)
        {
            case CSID.TCP_LOGIN:
                {

                    TcpLogin _info = CSData.DeserializeData<TcpLogin>(bodyData);

                    ServerTCP.Instance.AddClientSocket (_info.token, mySocket);

                    int _uid = UserManage.Instance.UserLogin(_info.token, ServerTCP.Instance.token);


                    Debug.Log("客户端ID" + _uid +"token"+_info.token + "ip" + ServerTCP.Instance.token);

                    TcpResponseLogin _result = new TcpResponseLogin();
                    _result.result = true;
                    _result.uid = _uid;
                    _result.udpPort = UdpManager.Instance.recvPort;

                    ServerTCP.Instance.SendMessage(ServerTCP.Instance.token, CSData.GetSendMessage<TcpResponseLogin>(_result, SCID.TCP_RESPONSE_LOGIN));
                }
                break;
            case CSID.TCP_REQUEST_MATCH:
                {
                    TcpRequestMatch _mes = CSData.DeserializeData<TcpRequestMatch>(bodyData);
                    if (MatchManage.Instance != null)
                    {
                        MatchManage.Instance.NewMatchUser(_mes.uid, _mes.roleID);
                    }

                    TcpResponseRequestMatch rmRes = new TcpResponseRequestMatch();
                    ServerTCP.Instance.SendMessage(_socketIp, CSData.GetSendMessage<TcpResponseRequestMatch>(rmRes, SCID.TCP_RESPONSE_REQUEST_MATCH));
                }
                break;
            case CSID.TCP_CANCEL_MATCH:
                {
                    TcpCancelMatch _mes = CSData.DeserializeData<TcpCancelMatch>(bodyData);
                    MatchManage.Instance.CancleMatch(_mes.uid);

                    TcpResponseCancelMatch cmRes = new TcpResponseCancelMatch();
                    ServerTCP.Instance.SendMessage(_socketIp, CSData.GetSendMessage<TcpResponseCancelMatch>(cmRes, SCID.TCP_RESPONSE_CANCEL_MATCH));
                }
                break;

            default:
                break;
        }
    }
}
