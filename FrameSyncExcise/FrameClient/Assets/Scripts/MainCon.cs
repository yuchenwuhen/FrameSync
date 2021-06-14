using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PBCommon;
using PBMatch;
using UnityEngine.SceneManagement;

public class MainCon : MonoBehaviour {

    public GameObject waitMatchObj;
    public GameObject startMatchObj;

    void Start()
    {
        waitMatchObj.SetActive(false);

        TCPPB.Instance().mes_request_match_result = Message_Reauest_Match_Result; // 请求对局
        TCPPB.Instance().mes_cancel_match_result = Message_Cancel_Match_Result;
        TCPPB.Instance().mes_enter_battle = Message_Enter_Battle;   // 进入战场
    }

    public void OnClickMatch()
    {
        waitMatchObj.SetActive(true);
        startMatchObj.SetActive(false);

        //开始匹配 
        TcpRequestMatch _mes = new TcpRequestMatch();
        _mes.uid = NetGlobal.Instance().userUid;
        _mes.roleID = 1;
        MyTcp.Instance.SendMessage(CSData.GetSendMessage<TcpRequestMatch>(_mes, CSID.TCP_REQUEST_MATCH));
    }

    public void OnCancelMatch()
    {
        waitMatchObj.SetActive(false);
        startMatchObj.SetActive(true);

        //取消匹配
        TcpCancelMatch _mes = new TcpCancelMatch();
        _mes.uid = NetGlobal.Instance().userUid;
        MyTcp.Instance.SendMessage(CSData.GetSendMessage<TcpCancelMatch>(_mes, CSID.TCP_CANCEL_MATCH));
    }

    void Message_Reauest_Match_Result(TcpResponseRequestMatch _result)
    {
        //waitMatchObj.SetActive(true);
        //startMatchObj.SetActive(false);
    }

    void Message_Cancel_Match_Result(TcpResponseCancelMatch _result)
    {
        //waitMatchObj.SetActive(false);
        //startMatchObj.SetActive(true);
    }

    // 进入战场
    void Message_Enter_Battle(PBBattle.TcpEnterBattle _mes)
    {
        Debug.Log(_mes.battleUserInfo + "进入战场");
        BattleData.Instance.UpdateBattleInfo(_mes.randSeed, _mes.battleUserInfo);

        SceneManager.LoadScene(GameConfig.battleScene);
    }


    void OnDestroy()
    {
        TCPPB.Instance().mes_request_match_result = null;
        TCPPB.Instance().mes_cancel_match_result = null;
    }
}
