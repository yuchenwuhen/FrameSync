using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetGlobal {

    public string serverIP;
    public int userUid;
    public int udpSendPort;

    private static NetGlobal singleInstance;

    public static NetGlobal Instance()
    {
        // 如果类的实例不存在则创建，否则直接返回
        if (singleInstance == null)
        {
            singleInstance = new NetGlobal();
        }
        return singleInstance;
    }

    //互斥锁
    private Mutex mutex_actionList = new Mutex();
    private List<Action> list_action = new List<Action>();

    private NetGlobal()
    {
        GameObject obj = new GameObject("NetGlobal");
        obj.AddComponent<NetUpdate>();
        GameObject.DontDestroyOnLoad(obj);
    }

    public void AddAction(Action _action)
    {
        //申请
        mutex_actionList.WaitOne();

        list_action.Add(_action);

        //释放
        mutex_actionList.ReleaseMutex();
    }

    public void DoForAction()
    {
        mutex_actionList.WaitOne();

        for (int i = 0; i < list_action.Count; i++)
        {
            list_action[i]();
        }
        list_action.Clear();
        mutex_actionList.ReleaseMutex();
    }
}

public class NetUpdate : MonoBehaviour
{
    void Update()
    {
        NetGlobal.Instance().DoForAction();
    }

    void OnApplicationQuit()
    {

        MyTcp.Instance.EndClient();
    }
}
