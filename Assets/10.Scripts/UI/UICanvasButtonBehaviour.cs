using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class UICanvasButtonEvent
{
    public Button button;
    public UnityEvent onClick;
}

public class UICanvasButtonBehaviour : MonoBehaviour
{
    public UICanvasButtonEvent[] buttonEvents;

    private void Awake()
    {
        for (int i = 0; i < buttonEvents.Length; i++)
        {
            UICanvasButtonEvent uiCanvasButtonEvent = buttonEvents[i];
            buttonEvents[i].button.onClick.RemoveAllListeners();
            buttonEvents[i].button.onClick.AddListener(() => Click(uiCanvasButtonEvent));
        }
    }

    private void Click(UICanvasButtonEvent uiCancasButtonEvent)
    {
        uiCancasButtonEvent.onClick?.Invoke();
    }

    public void CreateButton()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttonEvents = new UICanvasButtonEvent[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonEvents[i] = new UICanvasButtonEvent()
            {
                button = buttons[i]
            };
        }
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(UICanvasButtonBehaviour))]
public class UICanvasButtonBehaviourEditor : Editor
{
    UICanvasButtonBehaviour value;

    private void OnEnable()
    {
        value = (UICanvasButtonBehaviour)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Button"))
        {
            value.CreateButton();
            EditorUtility.SetDirty(value);
        }
    }
}
#endif
