using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class PopupManager : MonoSingleton<PopupManager>
{
    public PopupSetting popupSetting;
    public Transform popupRoot;

    public List<UIPopupBehaviour> allPools = new List<UIPopupBehaviour>();

    private UIPopupBehaviour GetPopupBehaviour(PopupType popupType)
    {
        PopupData data = popupSetting.GetData(popupType);
        List<UIPopupBehaviour> pools = allPools.FindAll(x => x.name == data.prefab.name);
        UIPopupBehaviour pool = null;
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].InPool)
            {
                pool = pools[i];
                break;
            }
        }
        if (pool == null)
        {
            GameObject poolObject = Instantiate(data.prefab, popupRoot);
            poolObject.name = data.prefab.name;
            pool = poolObject.GetComponent<UIPopupBehaviour>();
            allPools.Add(pool);
        }
        pool.transform.SetAsLastSibling();
        return pool;
    }

    public void Show(PopupType popupType)
    {
        UIPopupBehaviour popupBehaviour = GetPopupBehaviour(popupType);
        popupBehaviour.Show();
    }

    public void ShowSetting()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        UIPopupBehaviour popupBehaviour = GetPopupBehaviour(PopupType.Setting);
        popupBehaviour.Show();
    }

    public void ShowAlert(string category, int id)
    {
        UIPopupBehaviour popupBehaviour = GetPopupBehaviour(PopupType.Alert);
        UIPopupAlert uiPopupAlert = popupBehaviour.GetComponent<UIPopupAlert>();
        uiPopupAlert.Init(category, id);
        popupBehaviour.Show();
    }

    public void ShowAlarm(string category)
    {
        UIPopupBehaviour popupBehaviour = GetPopupBehaviour(PopupType.Alarm);
        UIPopupAlarm uiPopupAlarm = popupBehaviour.GetComponent<UIPopupAlarm>();
        uiPopupAlarm.Init(category);
        popupBehaviour.Show();
    }
}
