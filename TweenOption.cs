using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOption : MonoBehaviour {

    TweenScale ts;
    TweenPosition tp;
    public GameObject message;

    private void Start()
    {
        ts = GetComponent<TweenScale>();
        tp = GetComponent<TweenPosition>();
    }

    public void CencleFunc()
    {
        if(message.activeSelf)
        {
            message.SetActive(false);

        }
    }
}
