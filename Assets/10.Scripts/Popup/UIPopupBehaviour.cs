using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopupBehaviour : MonoBehaviour
{
    [Header("UI Popup")]
    public Transform root;

    /// <summary>
    /// 항시 Init 호출 여부
    /// </summary>
    public bool useAlwaysInit = false;

    [Space]
    public UnityEvent initEvent;

    /// <summary>
    /// 초기화 여부 (최초 Show)
    /// </summary>
    private bool isInit = false;

    /// <summary>
    /// Pool 상태 (true일 경우 사용 가능)
    /// </summary>
    private bool inPool;
    public bool InPool
    {
        get { return inPool; }
    }

    private void OnEnable()
    {
        UIManager.OnPopupClosed += Hide;
    }

    private void OnDisable()
    {
        UIManager.OnPopupClosed -= Hide;
    }

    public void Show()
    {
        inPool = false;
        root.gameObject.SetActive(true);

        if (useAlwaysInit ||
            !isInit)
        {
            isInit = true;
            initEvent?.Invoke();
        }
    }

    public void Hide()
    {
        inPool = true;
        root.gameObject.SetActive(false);
    }
}
