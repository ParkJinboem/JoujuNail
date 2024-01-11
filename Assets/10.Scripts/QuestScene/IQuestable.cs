using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IQuestable
{
    public void Init(int level, QuestHandData questHandData, Image handImage, Image[] fingerNailMasks);
    public void MoveToMainScene();
    public void AllReset();
    public void SetVectorBrush(string manicureName, DrawMode drawMode, int brushSize, Color brushColor, Texture2D pattern, bool isAditiveBrush, bool brushCanDrawOnBlack, bool usesLockMasks, bool drawEnable);
    public void SetDrawMode();
    public void SetDrawBrushOnOff(bool isOn);
    public void PatternItem(Sprite sprite, Vector3 scale, ManicureInfoData manicureInfoData);
    public void CreateStickerItem(ManicureInfoData manicureInfoData);
    public void StickerItemDeselect(StickerConverter stickerObject);
    public void ChangeEidtParent(StickerConverter stickerObject);
    public void ChageColorPatternObject(Color color);
    public void SelectBeautyItem();
    public void SelectFinger(int index);
    public void CancleFinger();
    public void ChangeBrushSize(int size);
    public void SetUpEraser();
    public void ChangeManicureItemList(string sceneName);
    public void ChangeStickerObjectStatus(bool value);
    public void QuestConfirm();
    public void ClearQuest(int finger, string handSpriteName);
    public void QuestSelectFinger(int fingerNum);
    public void paintEngineObjectSetActive();
    public void QuestCancleFinger();
    public void OnOffAllInterectable(bool isOn);
    public void CreateGuideSticker(string stickerTypeName);
    public void ClicekdQuestButtonWithUIAnim();
    public void ClickedQuestSceneBackButton(bool isClear);
    public void PreviewStep();
    public void ShowClearMark(int clearfinger, bool isClear);
    public int GetSceneStep();
    public bool IsClearing();
    public void SetClearing(bool isClear);
    public void Hide();
    public Image HandfootImage
    {
        get;
    }
    public AdvancedMobilePaint[] PaintEngines
    {
        get;
    }
    public AdvancedMobilePaint SelectPaintEngine
    {
        get;
    }
    public int[] PaintSteps
    {
        get;
    }
    public List<int> ClearFingers
    {
        get;
    }
    public List<PatternItems> PatternItems
    {
        get; set;
    }
    public Color PatternColor
    {
        get; set;
    }
    public GameObject PatternClone
    {
        set;
    }
    public List<CharacterStickerItems> CharacterStickerItems
    {
        get;set;
    }
    public List<StickerItems> StickerItems
    {
        get;set;
    }
    public List<AnimationStickerItems> AnimationStickerItems
    {
        get;set;
    }
    public List<GameObject> GuideStickerObjects
    {
        get;
    }
    public int Level
    {
        get;
    }
    public bool IsClear
    {
        get;set;
    }
    public int SelectFingerIndex
    {
        get;set;
    }
    public string SceneName
    {
        get;
    }
    public bool IsPaint
    {
        get;set;
    }
    public bool IsPatternScroll
    {
        get;set;
    }
    public bool IsChecking
    {
        get;set;
    }
}