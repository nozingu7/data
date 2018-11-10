using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAction : MonoBehaviour
{
    public static SoundAction instance = null;
    public AudioClip[] clips;
    public AudioSource audio;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            return;
        }
    }
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            audio.volume = 0.5f;
            audio.PlayOneShot(clips[0]);
        }

    }
}
