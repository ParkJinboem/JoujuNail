using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChecker : ManicureCheckBehaviour
{
    [SerializeField] private Animator checkAnimator;
    [SerializeField] private GameObject root;

    private QuestHandData questHandData;
    private IQuestable questable;

    private bool isActive;//오브젝트 활성화 여부
    public bool IsAcive
    {
        get { return isActive; }
        set { isActive = value; }
    }
    private bool isUrge;//선택 재촉 여부
    public bool IsUrge
    {
        set { isUrge = value; }
    }
    private bool isDuplicationClick; //중복클릭 여부

    public float timer = 0;//경과시간 측정 변수
    public float selectInductionTime;//선택 유도 시간

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init(IQuestable questable)
    {
        this.questable = questable;
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(Statics.level);

        isActive = false;
        IsOnObject(false);
    }

    private void Update()
    {
        //클릭후 경과시간 업데이트
        if (isDuplicationClick == true)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 1.8f)
        {
            //중복클릭에 의한 트리거 초기화
            checkAnimator.ResetTrigger("isAnswer");
            checkAnimator.ResetTrigger("isWrongAnswer");
            //애니메이션 실행
            checkAnimator.Play("Idle");
            timer = 0;
            isDuplicationClick = false;
        }
    }

    /// <summary>
    /// 여신 이미지오브젝트 활성화 여부
    /// </summary>
    /// <param name="isActive"></param>
    public void IsOnObject(bool isActive)
    {
        if (this.isActive == false)
        {
            root.SetActive(false);
            return;
        }
        root.SetActive(isActive);
    }

    /// <summary>
    /// 패턴컬러 선택 정답 여부 확인
    /// </summary>
    public void CheckPatternColor(bool correctSoundCheck)
    {
        QuestFingerData questFingerData = questHandData.questFingerDatas[questable.SelectFingerIndex];
        Color questColor = new Color(1, 1, 1, 1);
        UnityEngine.ColorUtility.TryParseHtmlString(questFingerData.questPatternData.patternColorString, out questColor);
        if (questColor == questable.PatternColor)
        {
            //정답일경우 오답애니메이션 종료후 체크 애니메이션 실행
            
            checkAnimator.ResetTrigger("isWrongAnswer");
            AnswerCheckItemAnim(true);
            if(correctSoundCheck)
            {
                SoundPlay(true);
            }
            
        }
        else
        {
            //오답일경우 정답애니메이션 종료후 체크 애니메이션 실행
            checkAnimator.ResetTrigger("isAnswer");
            AnswerCheckItemAnim(false);
            if(correctSoundCheck)
            {
                SoundPlay(false);
            }
        }
    }

    /// <summary>
    /// 아이템 선택 정답 여부 확인
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public override void ManicureCheckTriggerOn(ManicureConverter manicureConverter)
    {
        ManicureInfoData manicureInfoData = manicureConverter.ManicureInfoData;
        QuestFingerData questFingerData = questHandData.questFingerDatas[questable.SelectFingerIndex];
        switch (questable.PaintSteps[questable.SelectFingerIndex])
        {
            //매니큐어
            case 0:
                if (questFingerData.manicureName == manicureInfoData.spriteName)
                {
                    AnswerCheckItemAnim(true);
                    QuestSceneManager.Instance.QuestGuideHandSetUp(3);
                    SoundPlay(true);
                    isUrge = false;
                }
                else
                {
                    AnswerCheckItemAnim(false);
                    QuestSceneManager.Instance.QuestGuideHandSetUp(4);
                    SoundPlay(false);
                    isUrge = true;
                }
                SelectItemToUrgeAnim();
                break;
            //패턴
            case 1:
                QuestPatternData questPatternData = questFingerData.questPatternData;
                if (questPatternData.patternSpriteString == manicureInfoData.spriteName)
                {
                    AnswerCheckItemAnim(true);
                    SoundPlay(true);
                    isUrge = false;
                }
                else
                {
                    AnswerCheckItemAnim(false);
                    SoundPlay(false);
                    isUrge = true;
                }
                SelectItemToUrgeAnim();
                break;
            //캐릭터스티커
            case 2:
                List<QuestStickerData> questCharacterStickerDatas = questFingerData.questStickerDatas.FindAll(x => x.stickerType == StickerType.CharacterSticker);
                QuestStickerData questCharacterStickerData = questCharacterStickerDatas.Find(x => x.stickerSpriteString == manicureInfoData.spriteName);
                if (questCharacterStickerData == null)
                {
                    AnswerCheckItemAnim(false);
                    SoundPlay(false);
                }
                else
                {
                    AnswerCheckItemAnim(true);
                    SoundPlay(true);
                }
                QuestSceneManager.Instance.QuestGuideHandSetUp(4);
                SelectItemToUrgeAnim();
                break;
            //스티커
            case 3:
                List<QuestStickerData> questStickerDatas = questFingerData.questStickerDatas.FindAll(x => x.stickerType == StickerType.Sticker);
                QuestStickerData questStickerData = questStickerDatas.Find(x => x.stickerSpriteString == manicureInfoData.spriteName);
                if (questStickerData == null)
                {
                    AnswerCheckItemAnim(false);
                    SoundPlay(false);
                }
                else
                {
                    AnswerCheckItemAnim(true);
                    SoundPlay(true);
                }
                QuestSceneManager.Instance.QuestGuideHandSetUp(4);
                SelectItemToUrgeAnim();
                break;
            //애니메이션스티커
            case 4:
                List<QuestStickerData> questAnimationStickerDatas = questFingerData.questStickerDatas.FindAll(x => x.stickerType == StickerType.AnimationSticker);
                QuestStickerData questAnimationStickerData = questAnimationStickerDatas.Find(x => x.stickerSpriteString == manicureInfoData.spriteName);
                if (questAnimationStickerData == null)
                {
                    AnswerCheckItemAnim(false);
                    SoundPlay(false);
                }
                else
                {
                    AnswerCheckItemAnim(true);
                    SoundPlay(true);
                }
                QuestSceneManager.Instance.QuestGuideHandSetUp(4);
                SelectItemToUrgeAnim();
                break;
        }
    }

    /// <summary>
    /// 정답 여부 애니메이션 실행
    /// </summary>
    /// <param name="isAnswer"></param>
    private void AnswerCheckItemAnim(bool isAnswer)
    {
        //중복클릭이 아닐경우
        if (UIManager.Instance.One_Click() == true)
        {
            //오답을 천천히 클릭후 정답을 누르면 애니메이션 꼬이는 현상이있어서 시작전 리셋시켜줌
            checkAnimator.ResetTrigger("isWrongAnswer");
            checkAnimator.ResetTrigger("isAnswer");
            //정답일경우
            if (isAnswer == true)
            {
                checkAnimator.SetTrigger("isAnswer");
            }
            //오답일경우
            else
            {
                checkAnimator.SetTrigger("isWrongAnswer");
            }
        }
        //중복클릭일경우
        else
        {
            //정답일경우
            if (isAnswer == true)
            {
                if (!checkAnimator.GetCurrentAnimatorStateInfo(0).IsName("AnswerAnimation"))
                {
                    checkAnimator.ResetTrigger("isWrongAnswer");
                    checkAnimator.SetTrigger("isAnswer");
                }
            }
            //오답일경우
            else
            {
                if (!checkAnimator.GetCurrentAnimatorStateInfo(0).IsName("WrongAnswerAnimation"))
                {
                    checkAnimator.ResetTrigger("isAnswer");
                    checkAnimator.SetTrigger("isWrongAnswer");
                }
            }
        }
        isDuplicationClick = true;
        timer = 0;
    }

    /// <summary>
    /// 정답 매니큐어 선택유도 애니메이션 실행
    /// </summary>
    public void SelectItemToUrgeAnim()
    {
        ManicureConverter manicureConverter = QuestSceneManager.Instance.AnswerManicureConverter;
        StopCoroutine(ISelectItemToUrgeAnim(manicureConverter));
        StartCoroutine(ISelectItemToUrgeAnim(manicureConverter));
    }

    IEnumerator ISelectItemToUrgeAnim(ManicureConverter manicureConverter)
    {
        if (manicureConverter == null)
        {
            yield return new WaitForSeconds(1.65f);
        }
        Animator animator = manicureConverter.GetComponent<Animator>();
        selectInductionTime = 0;
        //하단 리스트의 아이탬 바운스 딜레이 시간이 3초
        while (selectInductionTime < 3f)
        {
            if (manicureConverter != QuestSceneManager.Instance.AnswerManicureConverter)
            {
                break;
            }

            animator.SetTrigger("isBack");
            selectInductionTime += Time.deltaTime;
            if (selectInductionTime >= 3f)
            {
                //매니큐어, 패턴 정답 유도하기
                if (isUrge == true)
                {
                    animator.SetTrigger("Bounce");
                    yield return new WaitForSeconds(3.25f);
                    animator.SetTrigger("isBack");
                    selectInductionTime = 0;
                }
                //매니큐어, 패턴 정답 유도않하기
                else
                {
                    animator.ResetTrigger("Bounce");
                    animator.SetTrigger("isBack");
                    break;
                }
            }
            yield return null;
        }
    }

    void SoundPlay(bool collect)
    {
        if(collect)
        {
            SoundManager.Instance.PlayEffectSound("Collect");
        }
        else if(!collect)
        {
            SoundManager.Instance.PlayEffectSound("Wrong");
        }
    }
}