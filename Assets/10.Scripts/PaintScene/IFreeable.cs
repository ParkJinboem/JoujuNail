using System.Collections.Generic;
using UnityEngine;

public interface IFreeable
{
    public void MoveToMainScene(bool isInit);
    public void AllReset();
    public void SetVectorBrush(string manicureName, DrawMode drawMode, int brushSize, Color brushColor, Texture2D pattern, bool isAditiveBrush, bool brushCanDrawOnBlack, bool usesLockMasks, bool drawEnable);
    public void SetDrawMode();
    public void SetDrawBrushOnOff(bool isOn);
    public void PatternItem(Sprite sprite, Vector3 scale, ManicureInfoData manicureInfoData);
    public void OnOffPatternSlider();
    public void CreateStickerItem(ManicureInfoData manicureInfoData);
    public void StickerItemDeselect(StickerConverter stickerObject);
    public void ChangeEidtParent(StickerConverter stickerObject);
    public void ChageSizePatternObject(float value);
    public void ChageAlphaPatternObject(int alphaNumber);
    public void ChageColorPatternObject(int id, Color color);
    public void SelectBeautyItem();
    public void SelectFinger(int fingerNum);
    public void CancleFinger();
    public void OnOffAllInterectable(bool isOn);
    public void NailMaskSetUp();
    public void ChangeBrushSize(int size);
    public void SetUpEraser();
    public void ChangeManicureItemList(string sceneName);
    public void ChangeStickerObjectStatus(bool value);
    public void FingerStepSetUp(int selectFinger, int fingerStep);
    public void PatternSlideSetActive();
    public void PaintSelectFinger(int fingerNum);
    public void paintEngineObjectSetActive();
    public void NailSetting();
    public void StickerItemRayCastOnOff(string sceneName);
    public string GetSceneName();
    public void PatternGuideAnim();
    public AdvancedMobilePaint[] PaintEngines
    {
        get;
    }
    public AdvancedMobilePaint SelectPaintEngine
    {
        get;
    }
    public List<int> SelectFingerCount
    {
        get;
    }
    public int SelectFingerIndex
    {
        get;
    }
    public List<PatternItems> PatternItems
    {
        get;set;
    }
    public GameObject PatternClone
    {
        set;
    }
    public float[] PatternSizes
    {
        get;set;
    }
    public int[] PatternAlphaNumbers
    {
        get;set;
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
    public int[] CharacterStickerCounts
    {
        get;set;
    }
    public int[] StickerCounts
    {
        get;set;
    }
    public int[] AnimationStickerCounts
    {
        get;set;
    }
    public GameObject patternSlider();
    public bool IsFirtst
    {
        get; set;
    }
    public bool IsPaint
    {
        get;set;
    }
    public ManicureInfoData ManicureInfoData
    {
        get;set;
    }
}
