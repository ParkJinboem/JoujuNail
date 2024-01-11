using System.Collections;
using System.Collections.Generic;
using OnDot.System;
using UnityEngine;

public class QuestAnimController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private HandFootSetting handFootSetting;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject foot;
    [SerializeField] private List<GameObject> fingers;
    [SerializeField] private List<GameObject> toess;
    [SerializeField] private GameObject handImage;
    [SerializeField] private GameObject footImage;
    [SerializeField] private PatternColorScroll patternColorScroll;
    [SerializeField] private GameObject homeBtn;
    [SerializeField] private GameObject changeColorHandBtn;

    private HandFootData data;
    private RectTransform rectTransform;

    public Animator uiAnimator;

    private float time;

    private int selectFinger;
    private int fingerStep;

    private List<GameObject> selectObjects;
    private GameObject selectImage;

    private bool isClick;

    private IQuestable questable;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="isQuest"></param>
    public void Init(IQuestable questable)
    {
        this.questable = questable;
        homeBtn.SetActive(true);
        changeColorHandBtn.SetActive(true);

        data = new HandFootData();

        Quest quest = PlayerDataManager.Instance.GetQuest();
        QuestHandData questHandData = DataManager.Instance.GetQuestHandDataWithLevel(quest.level);
        if (questHandData.selectType == SelectType.Hand)
        {
            data = handFootSetting.GetData(HandFootType.Hand, SceneType.Quest);
            rectTransform = hand.GetComponent<RectTransform>();
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            data = handFootSetting.GetData(HandFootType.Foot, SceneType.Quest);
            rectTransform = foot.GetComponent<RectTransform>();
        }

        //튜토리얼 2를 클리어하지않았으면 애니메이션 실행을 안함
        if(!PlayerDataManager.Instance.IsCompleteTutorial(2))
        { 
            homeBtn.SetActive(false);
            changeColorHandBtn.SetActive(false);
        }
        if (!PlayerDataManager.Instance.IsCompleteTutorial(3))
        {
            homeBtn.SetActive(false);
        }


        uiAnimator.SetTrigger("QuestScene");
        uiAnimator.SetBool("isQuestIdle", true);
        QuestSceneManager.Instance.SetTabButton(true);
    }

    public void MoveMainScene()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        StopCoroutine(IMoveMainScene());
        StartCoroutine(IMoveMainScene());
    }

    IEnumerator IMoveMainScene()
    {
        uiAnimator.SetTrigger("isMainScene");
        yield return new WaitForSeconds(0.75f);
        QuestSceneManager.Instance.MoveToMainScene();
    }

    /// <summary>
    /// 퀘스트 손가락 클릭시 해당 손가락에대한 애니메이션 실행
    /// </summary>
    /// <param name="index"></param>
    public void ClickedQuestSceneFinger(int index)
    {
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        if (isClick == true)
        {
            return;
        }
        //튜토리얼을 클리어하지않았으면 4번째 손가락만 클릭되도록 수정
        if (!PlayerDataManager.Instance.IsCompleteTutorial(2) && index != 4)
        {
            return;
        }

        SoundManager.Instance.PlayEffectSound("Finger");
        selectFinger = index;
        ChangeFingerToes(true, selectFinger, 0);
        questable.SelectFinger(selectFinger);
        isClick = true;
    }

    /// <summary>
    /// 퀘스트 손가락 꾸미기에서 뒤로가기 버튼 클릭시 애니메이션 실행
    /// </summary>
    /// <param name="isClear"></param>
    public void ClickedQuestSceneBackButton(bool isClear)
    {
        QuestSceneManager.Instance.QuestGuideHandSetUp(4);
        SoundManager.Instance.StopDrawSound();
        if (!changeColorHandBtn.activeSelf)
        {
            changeColorHandBtn.SetActive(true);
            changeColorHandBtn.GetComponent<HandColorChangeAnim>().StartAnim();
        }
        isClick = false;
        ChangeFingerToes(false, selectFinger, 0);
        BackQuestUIAnim(isClear, fingerStep);
        questable.SetDrawBrushOnOff(false);
        questable.CancleFinger();
    }

    public void BackBtnSoundPlay()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
    }

    /// <summary>
    /// 각 스텝에 맞는 UI의 뒤로가기 애니메이션 실행
    /// </summary>
    /// <param name="isClear"></param>
    /// <param name="step"></param>
    private void BackQuestUIAnim(bool isClear, int step)
    {
        StopCoroutine(IBackQuestUIAnim(isClear, step));
        StartCoroutine(IBackQuestUIAnim(isClear, step));
    }

    IEnumerator IBackQuestUIAnim(bool isClear, int step)
    {
        if (isClear == false)
        {
            uiAnimator.SetBool("isQuestIdle", true);
            yield return new WaitForSeconds(0.75f);
            QuestSceneManager.Instance.SetImageOff();
            yield return new WaitForSeconds(0.75f);
            uiAnimator.SetBool("isQuestPaint", false);
            uiAnimator.SetBool("isQuestPattern", false);
            uiAnimator.SetBool("isQuestCharacterSticker", false);
            uiAnimator.SetBool("isQuestSticker", false);
            uiAnimator.SetBool("isQuestAnimationSticker", false);
            string listName = "";
            switch (step)
            {
                case 0:
                    listName = "ManicureScene";
                    break;
                case 1:
                    listName = "PatternScene";
                    break;
                case 2:
                    listName = "characterStickerScene";
                    break;
                case 3:
                    listName = "stickerScene";
                    break;
                case 4:
                    listName = "animationStickerScene";
                    break;
            }
            if (questable.ClearFingers.Contains(questable.SelectFingerIndex))
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            questable.ChangeManicureItemList(listName);
            questable.OnOffAllInterectable(true);
        }
        else
        {
            uiAnimator.SetTrigger("isClearAnim");
        }
    }

    /// <summary>
    /// 각 스텝에 맞는 UI애니메이션 실행
    /// </summary>
    /// <param name="fingerNum"></param>
    /// <param name="step"></param>
    /// <param name="beforeStep"></param>
    public void ClickQuestUIAnim(int fingerNum, int step, int beforeStep)
    {
        switch (step)
        {
            case 0:
                QuestSceneManager.Instance.SetupGuidePaint(fingerNum, questable.SceneName);
                ClickQuestUIAnim("isQuestPaint", "ManicureScene", "isQuestIdle", 1.65f, 0);
                break;
            case 1:
                ClickQuestUIAnim("isQuestPattern", "PatternScene", "isQuestPaint", 0.75f, 1);
                patternColorScroll.IsInit = false;
                patternColorScroll.Init();
                break;
            case 2:
                if (beforeStep == 0)
                {
                    ClickQuestUIAnim("isQuestCharacterSticker", "characterStickerScene", "isQuestPaint", 0.75f, 2);
                }
                else
                {
                    ClickQuestUIAnim("isQuestCharacterSticker", "characterStickerScene", "isQuestPattern", 0.75f, 2);
                }
                break;
            case 3:
                if (beforeStep == 0)
                {
                    ClickQuestUIAnim("isQuestSticker", "stickerScene", "isQuestPaint", 0.75f, 3);
                }
                else if (beforeStep == 1)
                {
                    ClickQuestUIAnim("isQuestSticker", "stickerScene", "isQuestPattern", 0.75f, 3);
                }
                else
                {
                    ClickQuestUIAnim("isQuestSticker", "stickerScene", "isQuestCharacterSticker", 0.75f, 3);
                }
                break;
            case 4:
                if (beforeStep == 0)
                {
                    ClickQuestUIAnim("isQuestAnimationSticker", "animationStickerScene", "isQuestPaint", 0.75f, 4);
                }
                else if (beforeStep == 1)
                {
                    ClickQuestUIAnim("isQuestAnimationSticker", "animationStickerScene", "isQuestPattern", 0.75f, 4);
                }
                else if (beforeStep == 2)
                {
                    ClickQuestUIAnim("isQuestAnimationSticker", "animationStickerScene", "isQuestCharacterSticker", 0.75f, 4);
                }
                else
                {
                    ClickQuestUIAnim("isQuestAnimationSticker", "animationStickerScene", "isQuestSticker", 0.75f, 4);
                }
                break;
        }
        if (step != 0)
        {
            questable.IsPaint = false;
            questable.SetDrawBrushOnOff(false);
        }
        QuestSceneManager.Instance.QuestGuideHandSetUp(4);
    }

    private void ClickQuestUIAnim(string boolName, string listname, string returnboolName, float time, int step)
    {
        StopCoroutine(IClickQuestUIAnim(boolName, listname, returnboolName, time, step));
        StartCoroutine(IClickQuestUIAnim(boolName, listname, returnboolName, time, step));
    }

    IEnumerator IClickQuestUIAnim(string boolName, string listname, string returnboolName, float time, int step)
    {
        uiAnimator.SetBool(boolName, true);
        questable.SetClearing(false);
        questable.SelectBeautyItem();
        yield return new WaitForSeconds(time);
        questable.ChangeManicureItemList(listname);
        uiAnimator.SetBool(returnboolName, false);
        uiAnimator.SetBool("isQuestIdle", false);
        yield return new WaitForSeconds(0.45f);
        if (step != 0)
        {
            questable.IsPaint = false;
            questable.SetDrawBrushOnOff(false);
        }
        else
        {
            questable.IsPaint = true;
            questable.SetDrawBrushOnOff(true);
        }
        questable.paintEngineObjectSetActive();
        questable.ChangeStickerObjectStatus(true);
        yield return new WaitForSeconds(1.0f);
        QuestSceneManager.Instance.QuestGuideHandSetUp(1);

    }

    /// <summary>
    /// 선택한 손가락의 위치 및 크기 변경
    /// </summary>
    /// <param name="changeScale"></param>
    /// <param name="fingerNumber"></param>
    /// <param name="nailNumber"></param>
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

        //true면 손가락 확대 false면 손바닥 등장
        if (isbool == true)
        {
            while (time <= 0.75f)
            {
                time += Time.deltaTime;

                float scale = data.scaleAnimationCurve[fingerNumber].Evaluate(time);
                rectTransform.localScale = new Vector3(scale, scale, scale);
                float basef = data.baseAnimationCurve.Evaluate(time);
                rectTransform.localRotation = Quaternion.Euler(0, 0, data.rots[fingerNumber] * basef);
                rectTransform.anchoredPosition = new Vector2(data.pos[fingerNumber].x * basef, data.pos[fingerNumber].y * basef);
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
                rectTransform.anchoredPosition = new Vector2(data.pos[fingerNumber].x * basef, data.pos[fingerNumber].y * basef);
                yield return null;
            }
            time = 0;
        }
    }
}