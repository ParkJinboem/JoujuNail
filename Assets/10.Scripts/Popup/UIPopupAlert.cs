using System;
using TMPro;
using UnityEngine;

public struct PopupAlertSetting
{
    public string message;
    public string no;
    public Action noEvent;
    public string yes;
    public Action yesEvent;
}

public class UIPopupAlert : MonoBehaviour
{
    [Header("Content")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI noText;
    public TextMeshProUGUI yesText;

    private string category;
    private int id;

    public void Init(string category, int id)
    {
        this.category = category;
        this.id = id;

        SetUI();
    }

    public void ClickNo()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        GetComponent<UIPopupBehaviour>().Hide();
    }

    public void ClickYes()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        if (category == "Cpature")
        {
            AlbumSceneManager.Instance.ScreenShot();
        }
        else if (category == "Delete")
        {
            AlbumSceneManager.Instance.DeleteAlbum(id);
        }
    }

    private void SetUI()
    {
        
    }
}
