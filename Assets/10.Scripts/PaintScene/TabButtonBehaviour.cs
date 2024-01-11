using UnityEngine;

public abstract class TabButtonBehaviour : MonoBehaviour
{
    public abstract void InitOn();
    public abstract void InitOff();
    public abstract void TabButtonChange(TabButtonType tabButtonType);
    public abstract void BrushIconChange(ManicureInfoData manicureInfoData);

    private void OnEnable()
    {
        PaintSceneManager.OnTabButtonInited += HandlerInitOn;
        PaintSceneManager.OffTabButtonInited += HandlerInitOff;
        PaintSceneManager.OnTabButtonChanged += HandlerTabButtonChange;
        PaintSceneManager.OnBrushIconChanged += HandlerBrushIconhange;
        QuestSceneManager.OnTabButtonInited += HandlerInitOn;
        QuestSceneManager.OffTabButtonInited += HandlerInitOff;
        QuestSceneManager.OnTabButtonChanged += HandlerTabButtonChange;
        QuestSceneManager.OnBrushIconChanged += HandlerBrushIconhange;
    }

    private void OnDisable()
    {
        PaintSceneManager.OnTabButtonInited -= HandlerInitOn;
        PaintSceneManager.OffTabButtonInited -= HandlerInitOff;
        PaintSceneManager.OnTabButtonChanged -= HandlerTabButtonChange;
        PaintSceneManager.OnBrushIconChanged -= HandlerBrushIconhange;
        QuestSceneManager.OnTabButtonInited -= HandlerInitOn;
        QuestSceneManager.OffTabButtonInited -= HandlerInitOff;
        QuestSceneManager.OnTabButtonChanged -= HandlerTabButtonChange;
        QuestSceneManager.OnBrushIconChanged -= HandlerBrushIconhange;
    }

    private void HandlerInitOn()
    {
        InitOn();
    }

    private void HandlerInitOff()
    {
        InitOff();
    }

    private void HandlerTabButtonChange(TabButtonType tabButtonType)
    {
        TabButtonChange(tabButtonType);
    }

    private void HandlerBrushIconhange(ManicureInfoData manicureInfoData)
    {
        BrushIconChange(manicureInfoData);
    }
}
