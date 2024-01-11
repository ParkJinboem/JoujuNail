using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopupAlarm : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    private string category;

    public void Init(string category)
    {
        this.category = category;
        SetUI();
    }
    public void ClickedOk()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        GetComponent<UIPopupBehaviour>().Hide();
    }

    private void SetUI()
    {

    }
}
