using UnityEngine;
using UnityEngine.UI;

public class TapButton : TabButtonBehaviour
{
    [SerializeField] private TabButtonType tabButtonType;

    [Header("Setting")]
    public Sprite clickSprite;
    public Sprite unClickSprite;
    public Image tapButtonimage;
    public Image brushIcon;

    private ManicureInfoData manicureInfoData;

    private IQuestable questable;
    private IFreeable freeable;

    private bool isInit = false;

    public void Init(bool isInit)
    {
        this.isInit = isInit;
        if (isInit == false)
        {
            return;
        }

        if (Statics.gameMode == GameMode.Quest)
        {
            questable = QuestSceneManager.Instance.Questables[0];
        }
        else if (Statics.gameMode == GameMode.Free)
        {
            freeable = PaintSceneManager.Instance.Freeables[0];
        }
    }

    public override void InitOn()
    {
        Init(true);
    }

    public override void InitOff()
    {
        Init(false);
    }

    /// <summary>
    /// 탭버튼 이미지 변경
    /// </summary>
    /// <param name="tabButtonType"></param>
    public override void TabButtonChange(TabButtonType tabButtonType)
    {
        if (isInit == false)
        {
            return;
        }

        //현재 탭버튼타입이 같을 경우
        if (tabButtonType == this.tabButtonType)
        {
            tapButtonimage.sprite = clickSprite;
            if (Statics.gameMode == GameMode.Quest)
            {
                //탭버튼타입이 지우개가 아니고 매니큐어데이터가 존재할때 
                if (tabButtonType != TabButtonType.Eraser && manicureInfoData != null)
                {
                    questable.SetVectorBrush(manicureInfoData.spriteName, manicureInfoData.drawMode, Statics.brushSize, manicureInfoData.baseColor, null, false, true, true, true);
                }
            }
            else if (Statics.gameMode == GameMode.Free)
            {
                //탭버튼타입이 지우개가 아니고 매니큐어데이터가 존재할때
                if (tabButtonType != TabButtonType.Eraser && manicureInfoData != null)
                {
                    freeable.SetVectorBrush(manicureInfoData.spriteName, manicureInfoData.drawMode, Statics.brushSize, manicureInfoData.baseColor, null, false, true, true, true);
                }
            }
        }
        else
        {
            tapButtonimage.sprite = unClickSprite;
        }
    }

    /// <summary>
    /// 브러쉬 아이콘 변경
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public override void BrushIconChange(ManicureInfoData manicureInfoData)
    {
        //빅브러쉬를 선택중일때
        if (tabButtonType == TabButtonType.BigBrush)
        {
            this.manicureInfoData = manicureInfoData;
            brushIcon.sprite = DataManager.Instance.GetBigBrushIconItemSprite(manicureInfoData.bigBrushSpriteName);
            if (Statics.brushSize == 30)
            {
                tapButtonimage.sprite = clickSprite;
            }
            else
            {
                tapButtonimage.sprite = unClickSprite;
            }
        }
        //스몰브러쉬를 선택중일떄
        else if (tabButtonType == TabButtonType.SamllBrush)
        {
            this.manicureInfoData = manicureInfoData;
            brushIcon.sprite = DataManager.Instance.GetSmallBrushIconItemSprite(manicureInfoData.smallBrushSpriteName);
            if (Statics.brushSize == 15)
            {
                tapButtonimage.sprite = clickSprite;
            }
            else
            {
                tapButtonimage.sprite = unClickSprite;
            }
        }
        //지우개를 선택중일때
        else if (tabButtonType == TabButtonType.Eraser)
        {
            tapButtonimage.sprite = unClickSprite;
            return;
        }
    }
}