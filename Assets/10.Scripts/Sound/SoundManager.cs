using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class SoundManager : PersistentSingleton<SoundManager>
{
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource drawSource;
    [SerializeField] private AudioClipScriptable audioTable;
    [SerializeField] private PoolSystem poolSystem;
    [SerializeField] private Option optionData;
    private float bgmTime;

    //private List<AudioScript> audioScripts = new List<AudioScript>();

    public void IntroInit()
    {
        float sounVolume = PlayerDataManager.Instance.GetOptionData().soundVolume;
        bgSource.volume = sounVolume;
        effectSource.volume = sounVolume;
        drawSource.volume = sounVolume;

        bgSource.clip = audioTable.GetClip("BGM_01");
        bgSource.loop = true;
        bgSource.Play();
        SoundCheck();
        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
    }

    private void OnDestroy()
    {
        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
    }
    private void Update()
    {
        bgmTime = bgSource.time;
    }

    public void MainInit(Transform manager)
    {
        PlayBGM("BGM_05");
        transform.SetParent(manager);
    }

    void OnAudioConfigurationChanged(bool deviceWasChanged)
    {
        Debug.Log(deviceWasChanged ? "Device was changed" : "Reset was called");
        if (deviceWasChanged)
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.dspBufferSize = 64;
            AudioSettings.Reset(config);
        }
        bgSource.time = bgmTime;
        bgSource.Play();
    }

    public void SoundCheck()
    {
        optionData = PlayerDataManager.Instance.GetOptionData();
        if (!optionData.isSound)
        {
            bgSource.Pause();
            effectSource.mute = true;
            drawSource.mute = true;
        }
        else
        {
            bgSource.Play();
            effectSource.mute = false;
            drawSource.mute = false;
        }
    }

    public void ChangeVolume(float value)
    {
        bgSource.volume = value;
        effectSource.volume = value;
        drawSource.volume = value;
    }


    public void PlayBGM(string key)
    {
        if (audioTable.GetClip(key) != null)
        {
            bgSource.clip = audioTable.GetClip(key);
            bgSource.loop = true;
            SoundCheck();
        }
    }

    public void PlayEffectSound(string key)
    {
        if (audioTable.GetClip(key) != null)
        {
            AudioClip effectAudioClip = audioTable.GetClip(key);
            PlayAudioClip(effectAudioClip);
        }
    }

    public void PlayAudioClip(AudioClip key)
    {
        effectSource.PlayOneShot(key);
    }

    public AudioClip DrawAudioKey(string key)
    {
        AudioClip drawAudioClip = audioTable.GetClip(key);
        return drawAudioClip;
    }

    public void PlayDrawSound(string key)
    {
        AudioClip drawAudioClip = audioTable.GetClip(key);
        drawSource.clip = drawAudioClip;
        drawSource.loop = true;
        drawSource.Play();
    }

    public void StopDrawSound()
    {
        drawSource.loop = false;
        drawSource.clip = null;
        drawSource.Stop();
    }

}
