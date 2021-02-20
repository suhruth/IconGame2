using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource AS;
    public AudioSource BGM;
    public Sprite MusicOn, MusicOff;
    public GameObject AudioOnOffButton;
    bool isAudioOn = true;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void OnOptionClick(AudioClip ac)
    {
        AS.clip = ac;
        AS.Play();
    }

    public void OnAudioBtnClick()
    {
        if(isAudioOn)
        {
            BGM.volume = 0;
            isAudioOn = false;
            AudioOnOffButton.GetComponent<Image>().sprite = MusicOff;
        }
        else
        {
            BGM.volume = 1;
            isAudioOn = true;
            AudioOnOffButton.GetComponent<Image>().sprite = MusicOn;
        }
    }
}
