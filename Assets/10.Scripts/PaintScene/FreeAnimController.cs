using System.Collections;
using System.Collections.Generic;
using OnDot.System;
using UnityEngine;

public class FreeAnimController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private HandFootSetting handFootSetting;
    [SerializeField] private List<GameObject> fingers;
    [SerializeField] private List<GameObject> toess;
    [SerializeField] private GameObject handImage;
    [SerializeField] private GameObject footImage;
    [SerializeField] private PatternColorScroll patternColorScroll;

    private HandFootData data;
    private HandFootPositioner handFootPositioner;
    private RectTransform rectTransform;

    private float time;

    public Animator uiAnimator;

    private GameObject bgObject;
    public GameObject BgObject
    {
        get { return bgObject; }
    }

    private string sceneName;
    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }

    private int selectFinger;
    private int selectToes;
    private int fingerStep;

    private List<GameObject> selectObjects;
    private GameObject selectImage;

    private IFreeable freeable;
    public IFreeable Freeable
    {
        set { freeable = value; }
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="handFootPositioner"></param>
    public void Init(HandFootPositioner handFootPositioner)
    {
        data = new HandFootData();
        data.enabled = false;

        this.handFootPositioner = handFootPositioner;

        sceneName = "pattern";

        if (bgObject != null)
        {
            ResetBg();
        }
    }

    public void MoveMainScene()
    {
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        StopCoroutine(IMoveMainScene());
        StartCoroutine(IMoveMainScene());
    }

    IEnumerator IMoveMainScene()
    {
        if (freeable == null)
        {
            yield return null;
            PaintSceneManager.Instance.MoveToMainScene();
        }
        else
        {
            uiAnimator.SetTrigger("isMainScene");
            yield return new WaitForSeconds(0.75f);
            PaintSceneManager.Instance.MoveToMainScene();
        }
    }

    /// <summary>
    /// 배경이미지 위치 초기화
    /// </summary>
    public void ResetBg()
    {
        Transform[] childTrs = bgObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < childTrs.Length; i++)
        {
            childTrs[i].position = Vector3.zero;
        }
    }

    /// <summary>
    /// name에 따른 애니메이션 실행
    /// </summary>
    /// <param name="name"></param>
    /// <param name="bgObject"></param>
    public void ClickBg(string name, GameObject bgObject)
    {
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }

        if (!UIManager.Instance.One_Click())
        {
            return;
        }

        this.bgObject = bgObject;

        if (name == "HandBg")
        {
            data = handFootSetting.GetData(HandFootType.Hand, SceneType.Free);
            data.enabled = true;
            rectTransform = handFootPositioner.infoHand.handRect;
            Statics.selectType = SelectType.Hand;
            handFootPositioner.ScalePosChanger(true, false);
        }
        else if (name == "FootBg")
        {
            data = handFootSetting.GetData(HandFootType.Foot, SceneType.Free);
            data.enabled = true;
            rectTransform = handFootPositioner.infoFoot.footRect;
            Statics.selectType = SelectType.Foot;
            handFootPositioner.ScalePosChanger(false, true);
        }

        PaintSceneManager.Instance.SetDataWithInterface();
        PaintSceneManager.Instance.SetTabButton(true);

        uiAnimator.SetTrigger("FreeScene");
        uiAnimator.SetBool("isFreeIdle", true);

        freeable.NailMaskSetUp();
    }

    /// <summary>
    /// 손가락 클릭시 애니메이션 실행
    /// </summary>
    /// <param name="clickedFinger"></param>    
    public void ClickedFreeSceneFinger(int clickedFinger)
    {
        if (!UIManager.Instance.One_Click())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("Finger");
        PaintSceneManager.Instance.FreeGuideHandSetUp(1);
        selectFinger = clickedFinger;
        ChangeFingerToes(true, selectFinger, 0);
        freeable.SelectFinger(selectFinger);
        PaintSceneManager.Instance.SaveButtonSetActive(false);
    }

    /// <summary>
    /// 발가락 클릭시 애니메이션 실행
    /// </summary>
    /// <param name="clickedToes"></param>
    public void ClickedFreeSceneFoot(int clickedToes)
    {
        if (!UIManager.Instance.One_Click())
        {
            return;
        }

        if(data.enabled == false)
        {
            Debug.LogError("광클 뚫림");
            return;
        }
        PaintSceneManager.Instance.FreeGuideHandSetUp(1);
        selectToes = clickedToes;
        ChangeFingerToes(true, selectToes, 0);
        freeable.SelectFinger(selectToes);
        PaintSceneManager.Instance.SaveButtonSetActive(false);
    }

    public void ClickedFreeSceneBackButton()
    {
        if (!UIManager.Instance.One_Click())
        {
            return;
        }

        PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        SoundManager.Instance.StopDrawSound();
        StopAllCoroutines();

        if (Statics.selectType == SelectType.Hand)
        {
            ChangeFingerToes(false, selectFinger, 0);
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            ChangeFingerToes(false, selectToes, 0);
        }

        BackFreeUIAnim();

        freeable.SetDrawBrushOnOff(false);
        freeable.PatternSlideSetActive();
        PaintSceneManager.Instance.SaveButtonSetActive(true);
        freeable.CancleFinger();
    }

    public void BackBtnSoundPlay()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
    }

    private void BackFreeUIAnim()
    {
        StopCoroutine(IBackFreeUIAnim());
        StartCoroutine(IBackFreeUIAnim());
    }

    IEnumerator IBackFreeUIAnim()
    {
        uiAnimator.SetBool("isFreeIdle", true);
        yield return new WaitForSeconds(1.65f);
        uiAnimator.SetBool("isFreePaint", false);
        uiAnimator.SetBool("isFreePattern", false);
        uiAnimator.SetBool("isFreeCharacterSticker", false);
        uiAnimator.SetBool("isFreeSticker", false);
        uiAnimator.SetBool("isFreeAnimationSticker", false);
        
        freeable.ChangeManicureItemList("ManicureScene");
        yield return new WaitForSeconds(0.75f);
        freeable.OnOffAllInterectable(true);
    }

    public void ClickFreeUIAnim()
    {
        StopCoroutine(IClickFreeUIAnim());
        StartCoroutine(IClickFreeUIAnim());
    }

    IEnumerator IClickFreeUIAnim()
    {
        uiAnimator.SetBool("isFreePaint", true);
        freeable.SelectBeautyItem();
        freeable.IsFirtst = true;
        yield return new WaitForSeconds(1.65f);
        freeable.ChangeManicureItemList("ManicureScene");
        uiAnimator.SetBool("isFreeIdle", false);
        yield return new WaitForSeconds(0.45f);
        freeable.paintEngineObjectSetActive();
        freeable.ChangeStickerObjectStatus(true);
        freeable.SetDrawBrushOnOff(true);
    }

    public void NextStep()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        if (StickerConverter.currentStickerConverter != null)
        {
            freeable.StickerItemDeselect(StickerConverter.currentStickerConverter);
            StickerConverter.currentStickerConverter.DeselectItem();
        }

        StopAllCoroutines();

        switch (sceneName)
        {
            case "pattern":
                fingerStep = 1;
                if (freeable.SelectPaintEngine.drawEnabled == false)
                {
                    ClickNextFreeUIAnim("isFreePattern", "PatternScene", "isFreePaint");
                }
                else if (freeable.SelectPaintEngine.drawEnabled == true)
                {
                    ClickNextFreeUIAnim("isFreePattern", "PatternScene", "isFreePaint");
                }
                patternColorScroll.IsInit = false;
                patternColorScroll.Init();
                sceneName = "characterSticker";
                break;
            case "characterSticker":
                fingerStep = 2;
                freeable.patternSlider().SetActive(false);
                ClickNextFreeUIAnim("isFreeCharacterSticker", "characterStickerScene", "isFreePattern");
                sceneName = "sticker";
                break;
            case "sticker":
                fingerStep = 3;
                ClickNextFreeUIAnim("isFreeSticker", "stickerScene", "isFreeCharacterSticker");
                sceneName = "animationSticker";
                break;
            case "animationSticker":
                fingerStep = 4;
                ClickNextFreeUIAnim("isFreeAnimationSticker", "animationStickerScene", "isFreeSticker");
                sceneName = "selectFinger";
                break;
            case "selectFinger":
                fingerStep = 0;
                ClickNextFreeUIAnim("isFreeIdle", "ManicureScene", "isFreeAnimationSticker");
                sceneName = "pattern";
                ClickedFreeSceneBackButton();
                break;
        }

        if (sceneName != "pattern")
        {
            freeable.IsPaint = false;
            freeable.SetDrawBrushOnOff(false);
        }

        freeable.FingerStepSetUp(selectFinger, fingerStep);
        
    }

    private void ClickNextFreeUIAnim(string boolName, string listname, string returnboolName)
    {
        StopCoroutine(IClickNextFreeUIAnim(boolName, listname, returnboolName));
        StartCoroutine(IClickNextFreeUIAnim(boolName, listname, returnboolName));
    }

    IEnumerator IClickNextFreeUIAnim(string boolName, string listname, string returnboolName)
    {
        uiAnimator.SetBool(boolName, true);
        yield return new WaitForSeconds(0.75f);
        freeable.ChangeManicureItemList(listname);
        uiAnimator.SetBool(returnboolName, false);
        if (sceneName == "characterSticker")
        {
            yield return new WaitForSeconds(0.3f);
            freeable.OnOffPatternSlider();
        }
        yield return new WaitForSeconds(0.5f);
        PaintSceneManager.Instance.FreeGuideHandSetUp(1);
    }

    public void PrevStep()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        StopAllCoroutines();
        switch (sceneName)
        {
            //패턴씬
            case "characterSticker":
                fingerStep = 0;
                ClickPrevFreeUIAnim("isFreePattern", "ManicureScene", "isFreePaint");
                freeable.SetDrawBrushOnOff(true);
                sceneName = "pattern";
                break;
            //캐릭터 스티커씬
            case "sticker":
                fingerStep = 1;
                ClickPrevFreeUIAnim("isFreeCharacterSticker", "PatternScene", "isFreePattern");
                patternColorScroll.IsInit = false;
                patternColorScroll.Init();
                sceneName = "characterSticker";
                break;
            //스티커씬
            case "animationSticker":
                fingerStep = 2;
                ClickPrevFreeUIAnim("isFreeSticker", "characterStickerScene", "isFreeCharacterSticker");
                sceneName = "sticker";
                break;
            //애니메이션 스티커씬
            case "selectFinger":
                fingerStep = 3;
                ClickPrevFreeUIAnim("isFreeAnimationSticker", "stickerScene", "isFreeSticker");
                sceneName = "animationSticker";
                break;
        }

        freeable.FingerStepSetUp(selectFinger, fingerStep);
        //PaintSceneManager.Instance.FreeGuideHandSetUp(1);
    }

    private void ClickPrevFreeUIAnim(string boolName, string listname, string returnboolName)
    {
        StopCoroutine(IClickPrevFreeUIAnim(boolName, listname, returnboolName));
        StartCoroutine(IClickPrevFreeUIAnim(boolName, listname, returnboolName));
    }

    IEnumerator IClickPrevFreeUIAnim(string boolName, string listname, string returnboolName)
    {
        uiAnimator.SetBool(returnboolName, true);
        yield return new WaitForSeconds(0.75f);
        freeable.ChangeManicureItemList(listname);
        uiAnimator.SetBool(boolName, false);
        if (sceneName == "characterSticker")
        {
            yield return new WaitForSeconds(0.3f);
            freeable.OnOffPatternSlider();
        }
        yield return new WaitForSeconds(0.5f);
        PaintSceneManager.Instance.FreeGuideHandSetUp(1);
    }

    public void ChangeFingerToes(bool changeScale, int fingerNumber, int nailNumber)
    {
        ChangeHandFootScale(changeScale, fingerNumber, nailNumber);
    }

    private void ChangeHandFootScale(bool changeScale, int fingerNumber, int nailNumber)
    {
        StopCoroutine(IChangeHandFootScale(changeScale, fingerNumber, nailNumber));
        StartCoroutine(IChangeHandFootScale(changeScale, fingerNumber, nailNumber));
    }

    IEnumerator IChangeHandFootScale(bool isbool, int fingerNumber, int nailNumber)
    {
        if (Statics.selectType == SelectType.Hand)
        {
            selectObjects = fingers;
            selectImage = handImage;
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            selectObjects = toess;
            selectImage = footImage;
        }

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 5; i++)
        {
            selectObjects[i].SetActive(true);
            selectImage.SetActive(true);
        }

        if (isbool == true)
        {
            while (time <= 0.75f)
            {
                time += Time.deltaTime;

                float scale = data.scaleAnimationCurve[fingerNumber].Evaluate(time);
                rectTransform.localScale = new Vector3(scale, scale, scale);
                float basef = data.baseAnimationCurve.Evaluate(time);
                rectTransform.localRotation = Quaternion.Euler(0, 0, data.rots[fingerNumber] * basef);
                rectTransform.anchoredPosition = new Vector2(data.basePosition.x + data.pos[fingerNumber].x * basef, data.basePosition.y + data.pos[fingerNumber].y * basef);
                yield return null;
            }

            for (int i = 0; i < 5; i++)
            {
                selectObjects[i].SetActive(false);
                selectImage.SetActive(false);
            }
            selectObjects[fingerNumber].SetActive(true);
            time = 0.75f;
        }
        else if (isbool == false)
        {
            while (time >= 0)
            {
                time -= Time.deltaTime;

                float scale = data.scaleAnimationCurve[fingerNumber].Evaluate(time);
                rectTransform.localScale = new Vector3(scale, scale, scale);
                float basef = data.baseAnimationCurve.Evaluate(time);
                rectTransform.localRotation = Quaternion.Euler(0, 0, data.rots[fingerNumber] * basef);
                rectTransform.anchoredPosition = new Vector2(data.basePosition.x + data.pos[fingerNumber].x * basef, data.basePosition.y + data.pos[fingerNumber].y * basef);
                yield return null;
            }
            time = 0;
        }
    }
}