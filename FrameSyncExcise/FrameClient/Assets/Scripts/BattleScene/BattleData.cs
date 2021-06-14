using PBBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战场数据
/// </summary>
public class BattleData {

    private static BattleData instance;
    public static BattleData Instance
    {
        get
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (instance == null)
            {
                instance = new BattleData();
            }
            return instance;
        }
    }

    public int randSeed; //随机种子
    public int battleID;    //战场ID

    public List<BattleUserInfo> list_battleUser;

    public PlayerOperation selfOperation;

    private Dictionary<int, int> dic_rightOperationID;

    private BattleData()
    {

        //mapTotalGrid = mapRow * mapColumn;
        //mapWidth = mapColumn * gridLenth;
        //mapHeigh = mapRow * gridLenth;

        //curOperationID = 1;
        selfOperation = new PlayerOperation();
        selfOperation.move = 121;
        //ResetRightOperation();

        //dic_speed = new Dictionary<int, GameVector2>();
        ////初始化速度表
        //GlobalData.Instance().GetFileStringFromStreamingAssets("Desktopspeed.txt", _fileStr => {
        //    InitSpeedInfo(_fileStr);
        //});

        //curFramID = 0;
        //maxFrameID = 0;
        //maxSendNum = 5;

        //lackFrame = new List<int>();
        dic_rightOperationID = new Dictionary<int, int>();
        //dic_frameDate = new Dictionary<int, AllPlayerOperation>();
    }

    public void UpdateBattleInfo(int _randseed, List<BattleUserInfo> _userInfo)
    {
        Debug.Log("UpdateBattleInfo  更新战场信息 " + Time.realtimeSinceStartup);
        randSeed = _randseed;
        list_battleUser = new List<BattleUserInfo>(_userInfo);

        foreach (var item in list_battleUser)
        {
            if (item.uid == NetGlobal.Instance().userUid)
            {
                battleID = item.battleID;
                selfOperation.battleID = battleID;
                Debug.Log("自己的战斗id:" + battleID);
            }

            dic_rightOperationID[item.battleID] = 0;
        }
    }
}
