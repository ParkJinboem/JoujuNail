using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Setting = 0,
    Alert = 1,
    Alarm = 2
}

[System.Serializable]
public struct PopupData
{
    public PopupType type;
    public GameObject prefab;
    public bool isMultiple;
}

[CreateAssetMenu(fileName = "PopupSetting", menuName = "ScriptableObjects/PopupSettings")]
public class PopupSetting : ScriptableObject
{
    [SerializeField] private List<PopupData> datas;

    public PopupData GetData(PopupType popupType)
    {
        return datas.Find(x => x.type == popupType);
    }
}
