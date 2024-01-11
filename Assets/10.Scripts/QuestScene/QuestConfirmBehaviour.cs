using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestConfirmBehaviour : MonoBehaviour
{
    public delegate void ChangeParentHandler();
    public static event ChangeParentHandler OnChangeParentHandler;

    private QuestHandData questHandData;

    private int beforeStep;
    public int BeforeStep
    {
        get { return beforeStep; }
    }

    private bool isQuestClear = false;//퀘스트 전체 클리어 여부
    public bool IsQuestClear
    {
        get { return isQuestClear; }
        set { isQuestClear = value; }
    }
    private bool isClear = false;//각 단계 클리어 여부s
    public bool IsClear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    private IQuestable questable;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="questHandData"></param>
    /// <param name="questable"></param>
    public void Init(QuestHandData questHandData, IQuestable questable)
    {
        this.questHandData = questHandData;
        this.questable = questable;
    }

    /// <summary>
    /// 퀘스트 검사
    /// </summary>
    /// <param name="paintStep"></param>
    /// <param name="selectFinger"></param>
    /// <param name="paintEngine"></param>
    /// <param name="patternItems"></param>
    /// <param name="guideStickerObjects"></param>
    /// <param name="characterStickerItems"></param>
    /// <param name="stickerItems"></param>
    /// <param name="animationStickerItems"></param>
    /// <param name="clearFinger"></param>
    public void QuestConfirm(int[] paintStep, int selectFinger, AdvancedMobilePaint paintEngine, List<PatternItems> patternItems, List<GameObject> guideStickerObjects, List<CharacterStickerItems> characterStickerItems, List<StickerItems> stickerItems, List<AnimationStickerItems> animationStickerItems,List<int> clearFinger)
    {
        List<QuestFingerData> questFingerDatas = questHandData.questFingerDatas;
        QuestPatternData questPatternData = questFingerDatas[selectFinger].questPatternData;
        List<QuestStickerData> questCharacterStickerDatas = questFingerDatas[selectFinger].questStickerDatas.FindAll(x => x.stickerType == StickerType.CharacterSticker);
        List<QuestStickerData> questStickerDatas = questFingerDatas[selectFinger].questStickerDatas.FindAll(x => x.stickerType == StickerType.Sticker);
        List<QuestStickerData> questAnimationStickerDatas = questFingerDatas[selectFinger].questStickerDatas.FindAll(x => x.stickerType == StickerType.AnimationSticker);

        switch (paintStep[selectFinger])
        {
            case 0: // 그리기
                int confirmCount = ConfirmFunctioner.PaintConfirm(paintEngine, selectFinger, questFingerDatas[selectFinger]);
                if (confirmCount == 2)
                {
                    paintEngine.drawEnabled = false;

                    beforeStep = 0;
                    //패턴 퀘스트를 가지고 있을경우
                    if (questPatternData.isHave == true)
                    {
                        paintStep[selectFinger] = 1;
                    }
                    //아닐경우
                    else
                    {
                        int trueCharacterStickerCount = 0;
                        for (int i = 0; i < questCharacterStickerDatas.Count; i++)
                        {
                            if (questCharacterStickerDatas[i].isHave == true)
                            {
                                trueCharacterStickerCount++;
                            }
                        }
                        //캐릭터스티커 퀘스트를 가지고 있을경우
                        if (trueCharacterStickerCount == questCharacterStickerDatas.Count)
                        {
                            paintStep[selectFinger] = 2;
                        }
                        //아닐경우
                        else
                        {
                            int trueStickerCount = 0;
                            for (int i = 0; i < questStickerDatas.Count; i++)
                            {
                                if (questStickerDatas[i].isHave == true)
                                {
                                    trueStickerCount++;
                                }
                            }
                            //일반스티커 퀘스트를 가지고 있을경우
                            if (trueStickerCount == questCharacterStickerDatas.Count)
                            {
                                paintStep[selectFinger] = 3;
                            }
                            //아닐경우
                            else
                            {
                                int trueAnimationStickerCount = 0;
                                for (int i = 0; i < questAnimationStickerDatas.Count; i++)
                                {
                                    if (questAnimationStickerDatas[i].isHave == true)
                                    {
                                        trueAnimationStickerCount++;
                                    }
                                }
                                //애니메이션스티커를 가지고 있을경우
                                if (trueAnimationStickerCount == questAnimationStickerDatas.Count)
                                {
                                    paintStep[selectFinger] = 4;
                                }
                                //아닐경우
                                else
                                {
                                    paintStep[selectFinger] = 5;
                                }
                            }
                        }
                    }

                    if (questFingerDatas[selectFinger].mode == "Cream")
                    {
                        paintEngine.tex = HandFootTextureMaker.Instance.GetTexture2D(questFingerDatas[selectFinger].level, Statics.selectType, selectFinger);
                    }
                    else if (questFingerDatas[selectFinger].mode == "PearlRainbow")
                    {
                        paintEngine.tex = HandFootTextureMaker.Instance.GetTexture2D(questFingerDatas[selectFinger].level, Statics.selectType, selectFinger);
                    }
                    else if (questFingerDatas[selectFinger].mode == "Glittery")
                    {
                        paintEngine.tex = HandFootTextureMaker.Instance.GetTexture2D(questFingerDatas[selectFinger].level, Statics.selectType, selectFinger);
                    }
                    RawImage rawImage = paintEngine.GetComponent<RawImage>();
                    rawImage.texture = paintEngine.tex;
                    //스텝을 전부 완료않했을경우
                    if (paintStep[selectFinger] != 5)
                    {
                        ClearQuestPart(paintStep[selectFinger], null, selectFinger, paintEngine.transform, "ClearQuestPart");
                    }
                    //했을경우
                    else
                    {
                        ClearQuestPart(paintStep[selectFinger], clearFinger, selectFinger, paintEngine.transform, "ClearQuestPart");
                    }
                    isClear = true;
                }
                break;
            case 1:// 패턴
                if (patternItems[selectFinger].patternConverter != null)
                {
                    Image patternImage = patternItems[selectFinger].patternConverter.Image;
                    if (questPatternData != null && patternImage.sprite.name == questFingerDatas[selectFinger].questPatternData.patternSpriteString + "(Clone)" && ConfirmFunctioner.patternColorConfirm(questFingerDatas[selectFinger].questPatternData, patternItems[selectFinger].patternConverter, questable))
                    {
                        beforeStep = 1;
                        int trueCharacterStickerCount = 0;
                        for (int i = 0; i < questCharacterStickerDatas.Count; i++)
                        {
                            if (questCharacterStickerDatas[i].isHave == true)
                            {
                                trueCharacterStickerCount++;
                            }
                        }
                        //캐릭터스티커 퀘스트를 가지고 있을경우
                        if (trueCharacterStickerCount == questCharacterStickerDatas.Count)
                        {
                            paintStep[selectFinger] = 2;
                        }
                        //아닐경우
                        else
                        {
                            int trueStickerCount = 0;
                            for (int i = 0; i < questStickerDatas.Count; i++)
                            {
                                if (questStickerDatas[i].isHave == true)
                                {
                                    trueStickerCount++;
                                }
                            }
                            //일반스티커 퀘스트를 가지고 있을경우
                            if (trueStickerCount == questCharacterStickerDatas.Count)
                            {
                                paintStep[selectFinger] = 3;
                            }
                            //아닐경우 
                            else
                            {
                                int trueAnimationStickerCount = 0;
                                for (int i = 0; i < questAnimationStickerDatas.Count; i++)
                                {
                                    if (questAnimationStickerDatas[i].isHave == true)
                                    {
                                        trueAnimationStickerCount++;
                                    }
                                }
                                //애니메이션스티커 퀘스트를 가지고 있을경우
                                if (trueAnimationStickerCount == questAnimationStickerDatas.Count)
                                {
                                    paintStep[selectFinger] = 4;
                                }
                                //아닐경우
                                else
                                {
                                    paintStep[selectFinger] = 5;
                                }
                            }
                        }

                        //스텝을 전부 완료않했을경우
                        if (paintStep[selectFinger] != 5)
                        {
                            ClearQuestPart(paintStep[selectFinger], null, selectFinger, patternItems[selectFinger].patternItemParent, "ClearQuestPart");
                        }
                        //완료했을경우
                        else
                        {
                            ClearQuestPart(paintStep[selectFinger], clearFinger, selectFinger, patternItems[selectFinger].patternItemParent, "ClearQuestPart");
                        }

                        isClear = true;

                        //패턴 컬러색이 안맞았는데 맞아다고 처리되는 경우가 있어서 추가_231221 박진범
                        //Color correctColor;
                        //ColorUtility.TryParseHtmlString(questPatternData.patternColorString + "FF", out correctColor);
                        //patternImage.color = correctColor;
                    }
                }
                break;
            case 2:// 캐릭터 스티커
                GameObject clearCharacterStickerObject = ConfirmFunctioner.StickerConfirm(StickerConverter.currentStickerConverter, guideStickerObjects);
                if (clearCharacterStickerObject != null && guideStickerObjects.Count != 0)
                {
                    Image image = clearCharacterStickerObject.GetComponent<Image>();
                    image.color = new Color(1, 1, 1, 1);
                    GuideStickerConverter guideStickerConverter = clearCharacterStickerObject.GetComponent<GuideStickerConverter>();
                    clearCharacterStickerObject.transform.SetParent(characterStickerItems[selectFinger].characterStickerItemParent);
                    guideStickerObjects.Remove(clearCharacterStickerObject);
                    StickerConverter.currentStickerConverter.gameObject.transform.SetParent(transform);
                    characterStickerItems[selectFinger].stickerConverters.Remove(StickerConverter.currentStickerConverter);
                    characterStickerItems[selectFinger].guideStickerConverters.Add(guideStickerConverter);
                    Destroy(StickerConverter.currentStickerConverter.gameObject);
                }
                else if(clearCharacterStickerObject == null)
                {
                    OnChangeParentHandler();
                }

                int characterStickercount = 0;
                for (int i = 0; i < characterStickerItems[selectFinger].guideStickerConverters.Count; i++)
                {
                    if (characterStickerItems[selectFinger].guideStickerConverters[i] != null)
                    {
                        characterStickercount++;
                    }
                }

                if (guideStickerObjects.Count == 0 && characterStickercount == questCharacterStickerDatas.Count)
                {
                    beforeStep = 2;
                    int trueStickerCount = 0;
                    for (int i = 0; i < questStickerDatas.Count; i++)
                    {
                        if (questStickerDatas[i].isHave == true)
                        {
                            trueStickerCount++;
                        }
                    }
                    //일반스티커 퀘스트를 가지고 있을경우
                    if (trueStickerCount == questCharacterStickerDatas.Count)
                    {
                        paintStep[selectFinger] = 3;
                    }
                    //아닐경우
                    else
                    {
                        int trueAnimationStickerCount = 0;
                        for (int i = 0; i < questAnimationStickerDatas.Count; i++)
                        {
                            if (questAnimationStickerDatas[i].isHave == true)
                            {
                                trueAnimationStickerCount++;
                            }
                        }
                        //애니메이션스티커를 가지고 있을경우
                        if (trueAnimationStickerCount == questAnimationStickerDatas.Count)
                        {
                            paintStep[selectFinger] = 4;
                        }
                        //아닐경우
                        else
                        {
                            paintStep[selectFinger] = 5;
                        }
                    }
                    for (int i = 0; i < characterStickerItems[selectFinger].guideStickerConverters.Count; i++)
                    {
                        if (characterStickerItems[selectFinger].guideStickerConverters[i] != null)
                        {
                            GuideStickerConverter guideStickerConverter = characterStickerItems[selectFinger].guideStickerConverters[i];
                        }
                    }

                    //스텝을 전부 완료않했을경우
                    if (paintStep[selectFinger] != 5)
                    {
                        ClearQuestPart(paintStep[selectFinger], null, selectFinger, characterStickerItems[selectFinger].characterStickerItemParent, "ClearQuestPart");
                    }
                    //완료했을경우
                    else
                    {
                        ClearQuestPart(paintStep[selectFinger], clearFinger, selectFinger, characterStickerItems[selectFinger].characterStickerItemParent, "ClearQuestPart");
                    }
                    isClear = true;
                }
                break;
            case 3:// 스티커
                GameObject clearStickerObject = ConfirmFunctioner.StickerConfirm(StickerConverter.currentStickerConverter, guideStickerObjects);
                if (clearStickerObject != null)
                {
                    Image image = clearStickerObject.GetComponent<Image>();
                    image.color = new Color(1, 1, 1, 1);
                    GuideStickerConverter guideStickerConverter = clearStickerObject.GetComponent<GuideStickerConverter>();
                    clearStickerObject.transform.SetParent(stickerItems[selectFinger].stickerItemParent);
                    guideStickerObjects.Remove(clearStickerObject);
                    StickerConverter.currentStickerConverter.gameObject.transform.SetParent(transform);
                    stickerItems[selectFinger].stickerConverters.Remove(StickerConverter.currentStickerConverter);
                    stickerItems[selectFinger].guideStickerConverters.Add(guideStickerConverter);
                    Destroy(StickerConverter.currentStickerConverter.gameObject);
                }
                else if (clearStickerObject == null)
                {
                    OnChangeParentHandler();
                }

                int stickerCount = 0;
                for (int i = 0; i < stickerItems[selectFinger].guideStickerConverters.Count; i++)
                {
                    if (stickerItems[selectFinger].guideStickerConverters[i] != null)
                    {
                        stickerCount++;
                    }
                }

                if (guideStickerObjects.Count == 0 && stickerCount == questStickerDatas.Count)
                {
                    beforeStep = 3;
                    int trueAnimationStickerCount = 0;
                    for (int i = 0; i < questAnimationStickerDatas.Count; i++)
                    {
                        if (questAnimationStickerDatas[i].isHave == true)
                        {
                            trueAnimationStickerCount++;
                        }
                    }
                    //애니메이션스티커 퀘스트를 가지고 있을경우
                    if (trueAnimationStickerCount == questAnimationStickerDatas.Count)
                    {
                        paintStep[selectFinger] = 4;
                    }
                    //아닐경우
                    else
                    {
                        paintStep[selectFinger] = 5;
                    }
                    for (int i = 0; i < stickerItems[selectFinger].guideStickerConverters.Count; i++)
                    {
                        if (stickerItems[selectFinger].guideStickerConverters[i] != null)
                        {
                            GuideStickerConverter guideStickerConverter = stickerItems[selectFinger].guideStickerConverters[i];
                        }
                    }

                    //스텝을 전부 완료않했을경우
                    if (paintStep[selectFinger] != 5)
                    {
                        ClearQuestPart(paintStep[selectFinger], null, selectFinger, stickerItems[selectFinger].stickerItemParent, "ClearQuestPart");
                    }
                    //완료했을경우
                    else
                    {
                        ClearQuestPart(paintStep[selectFinger], clearFinger, selectFinger, stickerItems[selectFinger].stickerItemParent, "ClearQuestPart");
                    }
                    isClear = true;
                }
                break;
            case 4:// 애니메이션스티커
                GameObject clearAnimationStickerObject = ConfirmFunctioner.StickerConfirm(StickerConverter.currentStickerConverter, guideStickerObjects);
                if (clearAnimationStickerObject != null)
                {
                    Image image = clearAnimationStickerObject.GetComponent<Image>();
                    image.color = new Color(1, 1, 1, 1);
                    GuideStickerConverter guideStickerConverter = clearAnimationStickerObject.GetComponent<GuideStickerConverter>();
                    clearAnimationStickerObject.transform.SetParent(animationStickerItems[selectFinger].AnimationStickerItemParent);
                    guideStickerConverter.StartAnim();
                    guideStickerObjects.Remove(clearAnimationStickerObject);
                    StickerConverter.currentStickerConverter.gameObject.transform.SetParent(transform);
                    animationStickerItems[selectFinger].stickerConverters.Remove(StickerConverter.currentStickerConverter);
                    animationStickerItems[selectFinger].guideStickerConverters.Add(guideStickerConverter);
                    Destroy(StickerConverter.currentStickerConverter.gameObject);
                }
                else if (clearAnimationStickerObject == null)
                {
                    OnChangeParentHandler();
                }

                int animationStickerCount = 0;
                for (int i = 0; i < animationStickerItems[selectFinger].guideStickerConverters.Count; i++)
                {
                    if (animationStickerItems[selectFinger].guideStickerConverters[i] != null)
                    {
                        animationStickerCount++;
                    }
                }

                if (guideStickerObjects.Count == 0 && animationStickerCount == questStickerDatas.Count)
                {
                    paintStep[selectFinger] = 5;
                    ClearQuestPart(paintStep[selectFinger], clearFinger, selectFinger, animationStickerItems[selectFinger].AnimationStickerItemParent, "ClearQuestPart");
                    paintStep[selectFinger] = 0;
                    isClear = true;
                }
                break;
        }
    }

    /// <summary>
    /// 퀘스트를 클리어한후 애니메이션 실행 및 다음스텝 준비
    /// </summary>
    /// <param name="step"></param>
    /// <param name="clearFinger"></param>
    /// <param name="selectFinger"></param>
    /// <param name="position"></param>
    /// <param name="effectName"></param>
    private void ClearQuestPart(int step, List<int> clearFinger, int selectFinger, Transform position, string effectName)
    {
        StopCoroutine(IClearQuestPart(step, clearFinger, selectFinger, position, effectName));
        StartCoroutine(IClearQuestPart(step, clearFinger, selectFinger, position, effectName));
    }

    IEnumerator IClearQuestPart(int step, List<int> clearFinger, int selectFinger, Transform position, string effectName)
    {
        if (isClear == false)
        {
            UIManager.Instance.QuestSceneButtonOnOff(false);
            yield return new WaitForSeconds(0.25f);
            CreateParticle(position, effectName);
            SoundManager.Instance.PlayEffectSound("Clear");
            yield return new WaitForSeconds(1f);
            switch (step)
            {
                case 1:
                    QuestSceneManager.Instance.HideGuidePaint();
                    questable.ClicekdQuestButtonWithUIAnim();
                    questable.PreviewStep();
                    UIManager.Instance.QuestSceneButtonOnOff(true);
                    break;
                case 2:
                    questable.ClicekdQuestButtonWithUIAnim();
                    questable.PreviewStep();
                    questable.CreateGuideSticker("CharacterSticker");
                    UIManager.Instance.QuestSceneButtonOnOff(true);
                    break;
                case 3:
                    questable.ClicekdQuestButtonWithUIAnim();
                    questable.PreviewStep();
                    questable.CreateGuideSticker("Sticker");
                    UIManager.Instance.QuestSceneButtonOnOff(true);
                    break;
                case 4:
                    questable.ClicekdQuestButtonWithUIAnim();
                    questable.PreviewStep();
                    questable.CreateGuideSticker("AnimationSticker");
                    UIManager.Instance.QuestSceneButtonOnOff(true);
                    break;
                case 5:
                    CreateParticle(position, "HeartExplosion");
                    yield return new WaitForSeconds(1f);
                    if (clearFinger.Contains(selectFinger) == false)
                    {
                        questable.ClearFingers.Add(selectFinger);
                    }

                    //클리어한 손가락이 5개가 아닐경우
                    if (clearFinger.Count != 5)
                    {
                        questable.ClickedQuestSceneBackButton(false);
                        yield return new WaitForSeconds(2.25f);
                        questable.ShowClearMark(selectFinger, false);
                    }
                    //5개일경우
                    else if (clearFinger.Count == 5)
                    {
                        isQuestClear = true;
                        questable.ShowClearMark(selectFinger, isQuestClear);
                        yield return new WaitForSeconds(2f);
                        questable.ClickedQuestSceneBackButton(true);
                        yield return new WaitForSeconds(2.5f);
                        questable.ClearFingers.Clear();
                        questable.ClearQuest(selectFinger, questable.HandfootImage.sprite.name);
                    }
                    UIManager.Instance.QuestSceneButtonOnOff(true);
                    break;
            }
        }
    }

    /// <summary>
    /// 손가락 퀘스트 완료시 파티클 생성
    /// </summary>
    /// <param name="position"></param>
    /// <param name="effectName"></param>
    private void CreateParticle(Transform position, string effectName)
    {
        StopCoroutine(ICreateParticle(position, effectName));
        StartCoroutine(ICreateParticle(position, effectName));
    }

    IEnumerator ICreateParticle(Transform position, string effectName)
    {
        yield return null;
        GameObject particleEffect = ParticleManager.Instance.CreateParticle(effectName);
        particleEffect.transform.SetParent(position);
        particleEffect.transform.position = new Vector3(position.position.x, position.position.y, position.position.z);
        particleEffect.transform.localScale = new Vector3(70, 70, 1);
    }
}