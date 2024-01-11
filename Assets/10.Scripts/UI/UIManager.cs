using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public delegate void PopupClosedHandler();
    public static event PopupClosedHandler OnPopupClosed;

    public ScrollRect scrollRect;

    public Canvas sceneCanvas;

    private Button[] mainSceneButtons;
    private Button[] sceneButtons;

    public GameObject AllClearPopupPrefab; 
    private void Start()
    {
        mainSceneButtons = GetComponentsInChildren<Button>();
    }

    public void CloseAllPopup()
    {
        OnPopupClosed?.Invoke();
    }

    public void ButtonOnOff(bool isOn)
    {
        for(int i = 0; i < mainSceneButtons.Length; i++)
        {
            scrollRect.enabled = isOn;
            mainSceneButtons[i].enabled = isOn;
        }
    }

    public void FreeSceneButtonOnOff(bool isOn)
    {
        sceneButtons = PaintSceneManager.Instance.GetComponentsInChildren<Button>();
        for (int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i].enabled = isOn;
        }
    }

    public void QuestSceneButtonOnOff(bool isOn)
    {
        sceneButtons = QuestSceneManager.Instance.GetComponentsInChildren<Button>();
        for (int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i].enabled = isOn;
        }
    }

    #region             
    long Firsttime = 0;   // 첫번째 클릭시간
    public bool One_Click()
    {
        long CurrentTime = System.DateTime.Now.Ticks;
        if (CurrentTime - Firsttime < 4000000) // 0.4초 ( MS에서는 더블클릭 평균 시간을 0.4초로 보는거 같다.)
        {
            Firsttime = CurrentTime;   // 더블클릭 또는 2회(2회, 3회 4회...)클릭 시 실행되지 않도록 함
            return false;   // 더블클릭 됨
        }
        else
        {
            Firsttime = CurrentTime;   // 1번만 실행되도록 함
            return true;   // 더블클릭 아님
        }
    }
    #endregion
}
