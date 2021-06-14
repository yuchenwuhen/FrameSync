using PBBattle;
using PBCommon;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BattleCon {

    private int battleID;
    private Dictionary<int, int> dic_battleUserUid;

    //存储用户UDP连接 字典
    private Dictionary<int, ClientUdp> dic_udp;

    private Dictionary<int, bool> dic_battleReady;

    public void CreateBattle(int _battleId,List<MatchUserInfo> _battleUser)
    {
        //创建随机种子
        int randSeed = Random.Range(0, 100);
        //排队执行的方法。该方法在线程池线程可用时执行。
        ThreadPool.QueueUserWorkItem((obj) =>
        {
            battleID = _battleId;
            dic_battleUserUid = new Dictionary<int, int>();
            dic_udp = new Dictionary<int, ClientUdp>();
            dic_battleReady = new Dictionary<int, bool>();

            int userBattleID = 0;

            TcpEnterBattle _mes = new TcpEnterBattle();
            _mes.randSeed = randSeed;
            for (int i = 0; i < _battleUser.Count; i++)
            {
                int _userUid = _battleUser[i].uid;
                userBattleID++;  // 为每个user设置一个battleID， 这里就从1开始。

                dic_battleUserUid[_userUid] = userBattleID;

                string _ip = UserManage.Instance.GetUserInfo(_userUid).tocken;
                var _udp = new ClientUdp();

                _udp.StartClientUdp(_ip, _userUid);
                _udp.delegate_analyze_message = AnalyzeMessage;
                dic_udp[userBattleID] = _udp;
                dic_battleReady[userBattleID] = false;

                BattleUserInfo _bUser = new BattleUserInfo();
                _bUser.uid = _userUid;
                _bUser.battleID = userBattleID;
                _bUser.roleID = _battleUser[i].roleID;

                _mes.battleUserInfo.Add(_bUser);
            }

            for (int i = 0; i < _battleUser.Count; i++)
            {
                int _userUid = _battleUser[i].uid;
                string _ip = UserManage.Instance.GetUserInfo(_userUid).tocken;
                //为所有战场客户端发送TCP_ENTER_BATTLE
                Debug.Log("发送TCP_ENTER_BATTLE:" + _ip);
                ServerTCP.Instance.SendMessage(_ip, CSData.GetSendMessage<TcpEnterBattle>(_mes, SCID.TCP_ENTER_BATTLE));
            }
        }, null);
    }

    public void AnalyzeMessage(CSID messageId, byte[] bodyData)
    {
        Debug.Log("AnalyzeMessage   messageId == " + messageId);
        switch (messageId)
        {
            case CSID.UDP_BATTLE_READY:
                {
                    //接收战斗准备
                    UdpBattleReady _mes = CSData.DeserializeData<UdpBattleReady>(bodyData);
                    CheckBattleBegin(_mes.battleID);
                    dic_udp[_mes.battleID].RecvClientReady(_mes.uid);
                }
                break;
            //case CSID.UDP_UP_PLAYER_OPERATIONS:
            //    {
            //        UdpUpPlayerOperations pb_ReceiveMes = CSData.DeserializeData<UdpUpPlayerOperations>(bodyData);
            //        //     Debug.Log("pb_ReceiveMes.mesID == " + pb_ReceiveMes.mesID + "  move " + pb_ReceiveMes.operation.move) ;
            //        Debug.Log("pb_ReceiveMes.mesID == " + "  move " + pb_ReceiveMes.operation.move);
            //        UpdatePlayerOperation(pb_ReceiveMes.operation, pb_ReceiveMes.mesID);
            //    }
            //    break;
            //case CSID.UDP_UP_DELTA_FRAMES:
            //    {
            //        UdpUpDeltaFrames pb_ReceiveMes = CSData.DeserializeData<UdpUpDeltaFrames>(bodyData);

            //        UdpDownDeltaFrames _downData = new UdpDownDeltaFrames();

            //        for (int i = 0; i < pb_ReceiveMes.frames.Count; i++)
            //        {
            //            int framIndex = pb_ReceiveMes.frames[i];

            //            UdpDownFrameOperations _downOp = new UdpDownFrameOperations();
            //            _downOp.frameID = framIndex;
            //            _downOp.operations = dic_gameOperation[framIndex];

            //            _downData.framesData.Add(_downOp);
            //        }

            //        byte[] _data = CSData.GetSendMessage<UdpDownDeltaFrames>(_downData, SCID.UDP_DOWN_DELTA_FRAMES);
            //        dic_udp[pb_ReceiveMes.battleID].SendMessage(_data);
            //    }
            //    break;
            //case CSID.UDP_UP_GAME_OVER:
            //    {
            //        UdpUpGameOver pb_ReceiveMes = CSData.DeserializeData<UdpUpGameOver>(bodyData);
            //        UpdatePlayerGameOver(pb_ReceiveMes.battleID);

            //        UdpDownGameOver _downData = new UdpDownGameOver();
            //        byte[] _data = CSData.GetSendMessage<UdpDownGameOver>(_downData, SCID.UDP_DOWN_GAME_OVER);
            //        dic_udp[pb_ReceiveMes.battleID].SendMessage(_data);
            //    }
            //    break;
            default:
                break;
        }
    }

    bool isBeginBattle = false;

    private void CheckBattleBegin(int _userBattleID)
    {
        if (isBeginBattle)
        {
            return;
        }

        isBeginBattle = true;

        dic_battleReady[_userBattleID] = true;
        foreach (var item in dic_battleReady.Values)
        {
            isBeginBattle = item && isBeginBattle;
        }

        //所有的成员都准备完毕
        if (isBeginBattle)
        {
            Debug.Log("所有成员准备完毕，开始战斗");
            BeginBattle();
        }

    }

    int frameNum = 0;
    int lastFrame = 0;
    bool isRun = false;

    //开始战斗
    void BeginBattle()
    {
        frameNum = 0;
        lastFrame = 0;
        isRun = true;

        int playerNum = dic_battleUserUid.Keys.Count;


        Thread _threadSenfd = new Thread(Thread_SendFrameData);
        _threadSenfd.Start();
    }

    private void Thread_SendFrameData()
    {
        //向玩家发送战斗开始
        bool isFinishBS = false;
        while(!isFinishBS)
        {
            UdpBattleStart btData = new UdpBattleStart();
            byte[] _data = CSData.GetSendMessage<UdpBattleStart>(btData, SCID.UDP_BATTLE_START);
            foreach (var item in dic_udp)
            {
                //向每个客户端发送UDP_BATTLE_START消息
                item.Value.SendMessage(_data);
            }


        }
    }
}
