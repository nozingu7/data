using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour {

    public void Check()
    {
        Application.Quit();
    }

    public void Cancle()
    {
        Manager.instance.exitMsg.SetActive(false);
    }
}
