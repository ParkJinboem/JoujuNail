using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorData
{
    public int id;
    public Color color;
}

public class PatternColorConverter : MonoBehaviour
{
    public delegate void TriggerOnHandler(int id);
    public static event TriggerOnHandler TriggerOnEvent;

    public ColorData colorData;
    public ColorData ColorData
    {
        get { return colorData; }
    }

    private Image image;

    private bool isSelect;
    public bool IsSelect
    {
        get { return isSelect; }
        set { isSelect = value; }
    }

    private IQuestable questable;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 아이디 및 인터페이스 셋팅 및 컬러 셋팅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="questable"></param>
    public void Init(int id, IQuestable questable)
    {
        colorData = new ColorData()
        {
            id = id,
            color = image.color
        };

        this.questable = questable;
    }

    /// <summary>
    /// 오브젝트 클릭
    /// </summary>
    public void OnPointerDown()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable.IsClearing() == true)
            {
                return;
            }
        }
        TriggerOnEvent?.Invoke(colorData.id);
    }
}