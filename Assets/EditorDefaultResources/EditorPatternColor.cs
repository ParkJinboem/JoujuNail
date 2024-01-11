using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorPatternColor : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private int id;

    public void Start()
    {
        color = GetComponent<Image>().color;
    }

    public void ClickedColor()
    {
        EditorPaintSceneManager.Instance.SetUpPatternColor(id,color);
    }
}
