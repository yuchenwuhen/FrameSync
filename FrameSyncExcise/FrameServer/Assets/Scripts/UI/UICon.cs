using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICon : MonoBehaviour {

    public Text serverIP;
    public InputField input;
    public Button startserver;

    public void StartServer()
    {
        int number;
        if (int.TryParse(input.text,out number))
        {
            ServerConfig.battleUserNum = number;
            PlayerPrefs.SetInt("battleNumber", number);
        }

        input.interactable = false;
        startserver.interactable = false;

        ServerTCP.Instance.StartServer();
    }

}
