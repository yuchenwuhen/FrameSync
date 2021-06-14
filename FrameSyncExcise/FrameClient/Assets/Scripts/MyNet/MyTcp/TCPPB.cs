using PBCommon;
using PBLogin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBMatch;
using PBBattle;

/// <summary>
/// TCP连接的  Protobuff相关
/// </summary>
public class TCPPB {

    private static TCPPB instance;
    public static TCPPB Instance()
    {
        // 如果类的实例不存在则创建，否则直接返回
        if (instance == null)
        {
            instance = new TCPPB();
        }
        return instance;
    }

    //返回给游戏的delegate
    public delegate void DelegateReceiveMessage<T>(T message);

    public DelegateReceiveMessage<TcpResponseLogin> mes_login_result { get; set; }
    public DelegateReceiveMessage<TcpResponseRequestMatch> mes_request_match_result { get; set; }
    public DelegateReceiveMessage<TcpResponseCancelMatch> mes_cancel_match_result { get; set; }
    public DelegateReceiveMessage<TcpEnterBattle> mes_enter_battle { get; set; }

    public void AnalyzeMessage(SCID messageId, byte[] bodyData)
    {
        Debug.Log("收到服务器下发消息  " + messageId);
        switch (messageId)
        {
            case SCID.TCP_RESPONSE_LOGIN:
                {
                    TcpResponseLogin pb_ReceiveMes = CSData.DeserializeData<TcpResponseLogin>(bodyData);
                    NetGlobal.Instance().AddAction(() =>
                    {
                        if (mes_login_result != null)
                        {
                            mes_login_result(pb_ReceiveMes);
                        }
                    });
                }
                break;
            case SCID.TCP_RESPONSE_REQUEST_MATCH:
                {
                    TcpResponseRequestMatch pb_ReceiveMes = CSData.DeserializeData<TcpResponseRequestMatch>(bodyData);
                    NetGlobal.Instance().AddAction(() => {
                        mes_request_match_result(pb_ReceiveMes);
                    });
                }
                break;
            case SCID.TCP_RESPONSE_CANCEL_MATCH:
                {
                    TcpResponseCancelMatch pb_ReceiveMes = CSData.DeserializeData<TcpResponseCancelMatch>(bodyData);
                    NetGlobal.Instance().AddAction(() => {
                        mes_cancel_match_result(pb_ReceiveMes);
                    });
                }
                break;
            case SCID.TCP_ENTER_BATTLE:
                {
                    TcpEnterBattle pb_ReceiveMes = CSData.DeserializeData<TcpEnterBattle>(bodyData);
                    NetGlobal.Instance().AddAction(() => {
                        mes_enter_battle(pb_ReceiveMes);
                    });
                }
                break;
            default:
                break;
        }
    }
}
