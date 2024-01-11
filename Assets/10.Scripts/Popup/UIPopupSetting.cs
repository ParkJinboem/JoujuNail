using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSetting : MonoBehaviour
{
    [SerializeField] private Option option;

    [SerializeField] private Sprite buttonOn;
    [SerializeField] private Sprite buttonOff;
    [SerializeField] private Image volButton;
    [SerializeField] private GameObject[] volumeText;
    [SerializeField] private SoundVolume soundVolume;
    [SerializeField] private Toggle[] languageToggle;

    private bool soundON;
  
    public void Init()
    {
        option = PlayerDataManager.Instance.GetOptionData();
        soundON = option.isSound;
        soundVolume.volumeScroll.value = option.soundVolume;

        if (soundON == true)
        {
            volButton.sprite = buttonOn;
        }
        else
        {
            volButton.sprite = buttonOff;
        }
        if(option.language == "Korean")
        {
            languageToggle[0].isOn = true;
        }
        else
        {
            languageToggle[1].isOn = true;
        }
        textSetUp(soundON);
    }

    public void VolumeOnOff()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        if (soundON == false)
        {
            soundON = true;
            volButton.sprite = buttonOn;
        }
        else
        {
            soundON = false;
            volButton.sprite = buttonOff;
        }
        PlayerDataManager.Instance.SaveSoundOnOff(soundON);
        SoundManager.Instance.SoundCheck();
        textSetUp(soundON);
    }

    public void textSetUp(bool isOn)
    {
        if(isOn)
        {
            volumeText[0].SetActive(true);
            volumeText[1].SetActive(false);
        }
        else
        {
            volumeText[0].SetActive(false);
            volumeText[1].SetActive(true);
        }
    }

    public void ChangeLanguage(int SelectLanguage)
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        string language;
        if(SelectLanguage == 1)
        {
            language = "Korean";
        }
        else
        {
            language = "English";
        }
        I2.Loc.LocalizationManager.CurrentLanguage = language;
        PlayerDataManager.Instance.SaveLanguage(language);
  
    }
}