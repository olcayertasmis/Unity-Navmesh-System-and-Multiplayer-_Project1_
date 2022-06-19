using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public static float MusicV = 1f;


    private void Start()
    {
        slider.value = MusicV;
    }

    public void SetMusicVol(float value)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        MusicV = value;
    }
}
