using PBCommon;
using PBLogin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour {

    public InputField inputField;

    public InputField inputName;

    private void Start()
    {
        NetGlobal.Instance();
        TCPPB.Instance().mes_login_result = Message_Login_Result;
    }

    public void OnClickLogin()
    {

        string _ip = inputField.text;
        MyTcp.Instance.ConnectServer(_ip, (_result) => {
            if (_result)
            {
                Debug.Log("连接成功");
                NetGlobal.Instance().serverIP = _ip;
                TcpLogin _loginInfo = new TcpLogin();
                //_loginInfo.token = SystemInfo.deviceUniqueIdentifier; // 客户端凭证
                // 连接成功后发送消息
                _loginInfo.token = inputName.text;
                Debug.Log("发送登录请求");
                MyTcp.Instance.SendMessage(CSData.GetSendMessage<TcpLogin>(_loginInfo, CSID.TCP_LOGIN));
            }
            else
            {
                Debug.Log("连接失败");
            }
        });
    }

    void Message_Login_Result(TcpResponseLogin _mes)
    {
        if (_mes.result)
        {
            NetGlobal.Instance().userUid = _mes.uid;
            NetGlobal.Instance().udpSendPort = _mes.udpPort;
            Debug.Log("登录成功～～～" + NetGlobal.Instance().userUid);
            Debug.Log("进入匹配场景");
            SceneManager.LoadScene(GameConfig.MainScene);
            //ClearSceneData.LoadScene(GameConfig.mainScene);
        }
        else
        {
            Debug.Log("登录失败～～～");
        }
    }
}
