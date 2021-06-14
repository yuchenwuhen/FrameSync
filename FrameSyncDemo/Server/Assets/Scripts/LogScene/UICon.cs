﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICon : MonoBehaviour {

	public Text serverIP;
	public InputField input;
	public Button startserver;

	public RectTransform logContent;
	private GameObject logTextPrefab;
	private float logTextHeght;
	private int logTextNum;
    GameObject _logObj;
    RectTransform _logTextTran;
    Text logText;
    void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		serverIP.text = ServerGlobal.Instance.serverIp;

		ServerConfig.battleUserNum = PlayerPrefs.GetInt ("battleNumber",1);
		input.text = ServerConfig.battleUserNum.ToString();

		logTextPrefab = Resources.Load<GameObject> ("LogText");
		logTextHeght = 60f;
		logTextNum = 0;

		LogManage.Instance.logChange = LogChange;
          _logObj = Instantiate(logTextPrefab, logContent);
        _logTextTran =  _logObj.GetComponent<RectTransform>();
        logText = _logObj.GetComponent<Text>();
        Debug.Log("server start ");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerTcp.Instance.EndServer();
        }
    }

    public void StartServer(){

         
        int _number;
		if (int.TryParse(input.text,out _number)){
			ServerConfig.battleUserNum = _number;
			PlayerPrefs.SetInt("battleNumber",_number);
		}

		input.interactable = false;
		startserver.interactable = false;

		ServerTcp.Instance.StartServer ();
	}

    //UdpManager.Instance.Creat ();
    void LogChange(string _log){
        //GameObject _logObj = Instantiate (logTextPrefab,logContent);
        Debug.Log(_log);
		//_logTextTran.anchoredPosition = new Vector2 (0,-logTextNum * logTextHeght);
  //      logText.text = _log;

		//logTextNum++;
		//logContent.sizeDelta = new Vector2 (logContent.sizeDelta.x,logTextNum * logTextHeght);
	}
		
}
