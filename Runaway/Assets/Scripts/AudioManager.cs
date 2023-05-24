using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool ishaptic = true;

    [SerializeField] AudioMixer mixer;
    AudioSource bgmSource;

    [Header("AudioClip")]
    [SerializeField] AudioClip clip_bgm;
    [SerializeField] AudioClip clip_jump;
    [SerializeField] AudioClip clip_btnclick;
    [SerializeField] AudioClip clip_fail;
    [SerializeField] AudioClip clip_clear;


    public void Start()
    {
        if (!instance)
        {
            instance = this;

            bgmSource = GetComponent<AudioSource>();

            SetBGMVolume(PlayerPrefs.GetFloat("BGM", -20f));
            SetSFXVolume(PlayerPrefs.GetFloat("BGM", -20f));

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBGMVolume(float v)
    {
        if (v == -40f) mixer.SetFloat("BGM", -80f);
        else mixer.SetFloat("BGM", v);
    }
    public void SetSFXVolume(float v)
    {
        if (v == -40f) mixer.SetFloat("SFX", -80f);
        else mixer.SetFloat("SFX", v);
    }
    public void SetHaptic(bool isOn)
    {
        ishaptic = isOn;
    }
}
