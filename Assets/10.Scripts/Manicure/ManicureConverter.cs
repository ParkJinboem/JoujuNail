using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

public class ManicureConverter : MonoBehaviour, ICell
{
    public delegate void ManicureTriggerDownHandler(int manicureId);
    public static event ManicureTriggerDownHandler ManicureTriggerDownEvent;

    public delegate void ManicureCheckHandler(ManicureConverter manicureConverter);
    public static event ManicureCheckHandler ManicureCheckEvent;

    [SerializeField] private ManicureItem manicureItem;

    [Header("ManicureItemInfo")]
    public int id;
    private RectTransform rectTransform;
    private Image image;
    private Sprite sprite;
    private ManicureInfoData manicureInfoData;
    public ManicureInfoData ManicureInfoData
    {
        get { return manicureInfoData; }
    }
    private DrawMode mode;
    private Texture2D tempTex;

    private bool isClick;//클릭했는지 여부
    private bool isSelect;//선택되어있는지 여부
    public bool IsSelect
    {
        get { return isSelect; }
        set { isSelect = value; }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 하단 리스크 아이템 초기값 설정
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public void Init(ManicureInfoData manicureInfoData)
    {
        this.manicureInfoData = manicureInfoData;
        id = manicureInfoData.id;
        mode = manicureInfoData.drawMode;
        image.color = new Color(1, 1, 1, 1);

        sprite = DataManager.Instance.GetManicureItemSprite(manicureInfoData.spriteName);

        if (manicureInfoData.type == "Manicure")
        {
            rectTransform.sizeDelta = Statics.manicureSize;
        }
        else if (manicureInfoData.type == "Pattern")
        {
            rectTransform.sizeDelta = Statics.patternSize;
        }
        else
        {
            rectTransform.sizeDelta = Statics.stickerSize;
        }

        //현재 선택한 매니큐어 아이디와 매니큐어데이터의 아이디가 같을경우
        if (Statics.selectManicureId == manicureInfoData.id)
        {
            image.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureInfoData.useOnSpriteName);
            rectTransform.localRotation = Quaternion.Euler(0, 0, -17.5f);
            isSelect = true;
        }
        //아닐경우
        else
        {
            image.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureInfoData.useOffSpriteName);
            rectTransform.localRotation = Quaternion.identity;
            isSelect = false;
        }
    }

    /// <summary>
    /// 매니큐어 선택을 위한 클릭여부(수동클릭 여부 확인)
    /// </summary>
    public void CheckManicure()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            isClick = true;
        }
    }

    /// <summary>
    /// 선택한 매니큐어 셋팅
    /// </summary>
    public void SetUpPaint()
    {
        IQuestable questable = null;
        IFreeable freeable = null;
        if (Statics.gameMode == GameMode.Quest)
        {
            questable = QuestSceneManager.Instance.Questables[0];

            if (questable.IsClearing() == true)
            {
                return;
            }
        }
        else
        {
            if (PaintSceneManager.Instance.Freeables.Count == 0)
            {
                return;
            }
            SoundManager.Instance.PlayEffectSound("Collect");
            freeable = PaintSceneManager.Instance.Freeables[0];
        }


        //Statics.selectManicureId = manicureInfoData.id;
        //마지막 매니큐어를 무조건 저장하도록 변경_231123 박진범
        if (manicureInfoData.type == "Manicure")
        {
            PlayerDataManager.Instance.SetCurManicreId(manicureInfoData.id);
            //Statics.selectManicureId = PlayerDataManager.Instance.GetInfo().curSelectedManicureId;
            Statics.selectManicureId = manicureInfoData.id;
        }

        ManicureTriggerDownEvent?.Invoke(manicureInfoData.id);

        if (Statics.gameMode == GameMode.Free)
        {
            FreeSwitchDrawMode(freeable);
        }
        else
        {
            if (isClick == true)
            {
                //프리뷰 캐릭터 애니메이션
                ManicureCheckEvent?.Invoke(this);
                isClick = false;
            }
            QuestSwitchDrawMode(questable);
        }
    }

    /// <summary>
    /// 선택한 매니큐어 타입에 따른 모드 설정 
    /// </summary>
    /// <param name="questable"></param>
    private void QuestSwitchDrawMode(IQuestable questable)
    {
        if (questable.SelectPaintEngine == null)
        {
            return;
        }
        switch (mode)
        {
            case DrawMode.Cream:
                questable.SelectPaintEngine.useAdditiveColors = true;
                questable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, null, false, true, true, true);
                QuestSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                break;
            case DrawMode.Glittery:
                questable.SelectPaintEngine.useAdditiveColors = true;
                tempTex = PaintUtils.ConvertSpriteToTexture2D(sprite);
                questable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, tempTex, false, true, true, true);
                QuestSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                break;
            case DrawMode.PearlRainbow:
                questable.SelectPaintEngine.useAdditiveColors = true;
                tempTex = PaintUtils.ConvertSpriteToTexture2D(sprite);
                questable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, tempTex, false, true, true, true);
                QuestSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                break;
            case DrawMode.Pattern:
                QuestHandData questHandData = DataManager.Instance.GetQuestHandDataWithLevel(Statics.level);
                QuestPatternData questPatternData = questHandData.questFingerDatas[questable.SelectFingerIndex].questPatternData;
                if (questPatternData.patternSpriteString == manicureInfoData.spriteName)
                {
                    Vector3 scale = new Vector3(questPatternData.patternScaleX, questPatternData.patternScaleY, questPatternData.patternScaleZ);
                    questable.PatternItem(sprite, scale, manicureInfoData);
                }
                else
                {
                    questable.PatternItem(sprite, Vector3.one, manicureInfoData);
                }
                break;
            case DrawMode.Sticker:
                questable.CreateStickerItem(manicureInfoData);
                break;
        }
    }

    /// <summary>
    /// 선택한 매니큐어 타입에 따른 모드 설정 
    /// </summary>
    /// <param name="freeable"></param>
    private void FreeSwitchDrawMode(IFreeable freeable)
    {
        if (freeable.SelectPaintEngine == null)
        {
            return;
        }
        switch (mode)
        {
            case DrawMode.Cream:
                freeable.SelectPaintEngine.useAdditiveColors = true;
                freeable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, null, false, true, true, true);
                PaintSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                PaintSceneManager.Instance.FreeGuideHandSetUp(3);
                break;
            case DrawMode.Glittery:
                freeable.SelectPaintEngine.useAdditiveColors = true;
                tempTex = PaintUtils.ConvertSpriteToTexture2D(sprite);
                freeable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, tempTex, false, true, true, true);
                PaintSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                PaintSceneManager.Instance.FreeGuideHandSetUp(3);
                break;
            case DrawMode.PearlRainbow:
                freeable.SelectPaintEngine.useAdditiveColors = true;
                tempTex = PaintUtils.ConvertSpriteToTexture2D(sprite);
                freeable.SetVectorBrush(manicureInfoData.spriteName, mode, Statics.brushSize, ManicureInfoData.baseColor, tempTex, false, true, true, true);
                PaintSceneManager.Instance.ChangeBrushIcon(manicureInfoData);
                PaintSceneManager.Instance.FreeGuideHandSetUp(3);
                break;
            case DrawMode.Pattern:
                freeable.PatternItem(sprite, Vector3.one, manicureInfoData);
                //패턴가이드손가락 수정_240104 박진범
                if(id != 113)
                {
                    PaintSceneManager.Instance.PatternHandAnim();
                }
                break;
            case DrawMode.Sticker:
                freeable.CreateStickerItem(manicureInfoData);
                PaintSceneManager.Instance.FreeGuideHandSetUp(4);
                break;
        }
        freeable.ManicureInfoData = manicureInfoData;
    }

    public void Hide()
    {
        manicureItem.Hide();
    }
}