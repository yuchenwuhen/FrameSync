using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserInfo
{
    public string tocken;
    public bool isLogin;

    public UserInfo(string _socketIp, bool _isLogin)
    {
        tocken = _socketIp;
        isLogin = _isLogin;
    }
}

public class UserManage {
    private static readonly object umlockObj = new object();
    private static UserManage instance = null;

    public static UserManage Instance
    {
        get
        {
            lock (umlockObj)
            {
                if (instance == null)
                {
                    instance = new UserManage();
                }
            }
            return instance;
        }
    }

    private int userUid;
    private Dictionary<int, UserInfo> dic_userInfo;
    private Dictionary<string, int> dic_tokenUid;

    private UserManage()
    {
        userUid = 0;
        dic_userInfo = new Dictionary<int, UserInfo>();
        dic_tokenUid = new Dictionary<string, int>();
    }

    public int UserLogin(string _token, string _socketIp)
    {
        int _uid;
        if (dic_tokenUid.ContainsKey(_token))
        {
            _uid = dic_tokenUid[_token];
        }
        else
        {
            userUid++;
            _uid = userUid;
            dic_tokenUid[_token] = userUid;
        }

        UserInfo _userInfo = new UserInfo(_token, true);
        dic_userInfo[_uid] = _userInfo;

        return _uid;
    }

    public UserInfo GetUserInfo(int _uid)
    {
        return dic_userInfo[_uid];
    }
}
