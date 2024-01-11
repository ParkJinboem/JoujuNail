using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class ManicureItemScrollView : ManicureTriggerBehaviour, IBeginDragHandler
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private ManicureItemSpawner manicureItemSpawner;
    [SerializeField] private AnimationCurve contentBoxAnimation;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scroll;
    public Transform itemParent;
    private float itemWidth = 150.0f;
    private float itemPadding;

    [Header("ManicureItem")]
    private List<ManicureConverter> manicureConverters;
    public List<ManicureConverter> ManicureConverters
    {
        get { return manicureConverters; }
        set { manicureConverters = value; }
    }
    private List<ManicureInfoData> manicureInfoDatas;
    public GameObject manicureItemPrefabs;

    [SerializeField] private GameObject ReturnParent;

    private List<GameObject> returnObjects;

    private bool isScroll;
    public bool IsScroll
    {
        get { return isScroll; }
    }
    private bool rupe = false;
    private bool isInit;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        boxCollider2D.size = new Vector2(Screen.width, rectTransform.rect.size.y);
    }

    /// <summary>
    /// 하단 리스트 초기값 설정
    /// </summary>
    public void Init()
    {
        manicureInfoDatas = new List<ManicureInfoData>();
        manicureInfoDatas = DataManager.Instance.GetManicureInfoDataByType("Manicure");

        manicureItemSpawner.Init(itemParent, manicureInfoDatas);
        manicureConverters = new List<ManicureConverter>();
        manicureConverters = manicureItemSpawner.ManicureConverters;

        returnObjects = new List<GameObject>();

        ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
        HorizontalLayoutGroup horizontalLayoutGroup = scrollRect.content.GetComponent<HorizontalLayoutGroup>();
        itemPadding = horizontalLayoutGroup.spacing;

        RectTransform rectTransform = manicureConverters[0].GetComponent<RectTransform>();
        itemWidth = rectTransform.sizeDelta.x;

        isInit = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작하면 가이드핸드 애니메이션 종료
        if (Statics.gameMode == GameMode.Free && !rupe)
        {
            PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        }
        else if (Statics.gameMode == GameMode.Quest && !rupe)
        {
            QuestSceneManager.Instance.QuestGuideHandSetUp(4);
        }
    }

    public void moveScroll()
    {
        if(itemParent.childCount < 8)
        {
            scroll.horizontal = false;
            scroll.content.anchoredPosition = new Vector2(0, 0);
            return;
        }

        scroll.horizontal = true;
        float contentX = scroll.content.anchoredPosition.x;

        foreach (var item in manicureConverters)
        {
            RelocationItem(item, contentX);
        }
    }

    private void RelocationItem(ManicureConverter manicureConverter, float contentX)
    {
        // 왼쪽에 붙이기
        if (manicureConverter.transform.localPosition.x + contentX > itemWidth * itemParent.childCount + itemWidth / 2)
        {
            manicureConverter.transform.localPosition -= new Vector3(manicureConverters.Count * (itemWidth + itemPadding), 0);
        }
        // 오른쪽에 붙이기
        else if (manicureConverter.transform.localPosition.x + contentX < -(itemWidth * (itemParent.childCount - 8) + itemWidth / 2))
        {
            manicureConverter.transform.localPosition += new Vector3(manicureConverters.Count * (itemWidth + itemPadding), 0);
        }
        //else if (manicureConverter.transform.localPosition.x + contentX < -(itemWidth * (itemParent.childCount - 7) + itemWidth / 2))

    }

    /// <summary>
    /// 아이템 리스트를 교체
    /// </summary>
    /// <param name="itemType"></param>
    public void ResetContent(string itemType)
    {
        Resets();

        RectTransform rect = scroll.content.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        manicureInfoDatas = DataManager.Instance.GetManicureInfoDataByType(itemType);
        manicureItemSpawner.Init(itemParent, GetManicureInfoDatas(itemType));
        manicureConverters = manicureItemSpawner.ManicureConverters;

        RectTransform rectTransform = manicureConverters[0].GetComponent<RectTransform>();
        itemWidth = rectTransform.sizeDelta.x;

        int index = itemParent.childCount;
        for (int i = 0; i < index; i++)
        {
            if(itemParent.GetChild(i).gameObject.activeSelf == false)
            {
                returnObjects.Add(itemParent.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < returnObjects.Count; i++)
        {
            returnObjects[i].transform.SetParent(ReturnParent.transform);
        }
        ItemArraySort();
        if(itemType == "Manicure" && Statics.gameMode == GameMode.Quest)
        {
            ChangePosition();
        }
    }

    /// <summary>
    /// 아이템 리스트 리셋
    /// </summary>
    public void Resets()
    {
        if (isInit == false)
        {
            return;
        }

        if (manicureConverters != null)
        {
            for (int i = 0; i < manicureConverters.Count; i++)
            {
                manicureConverters[i].Hide();
            }
            manicureConverters = new List<ManicureConverter>();
            manicureInfoDatas = new List<ManicureInfoData>();
        }

        for (int i = 0; i < returnObjects.Count; i++)
        {
            returnObjects[i].transform.SetParent(itemParent);
        }
        returnObjects = new List<GameObject>();
        isScroll = false;
    }

    /// <summary>
    /// 아이템리스트 콘텐츠박스 애니메이션 실행
    /// </summary>
    public void PlayMoveContentBox()
    {
        rupe = false;
        MoveContentBox();
    }

    private void MoveContentBox()
    {
        StopCoroutine(IMoveContentBox());
        StartCoroutine(IMoveContentBox());
    }

    IEnumerator IMoveContentBox()
    {
        isScroll = true;
        float time = 0;
        rupe = true;
        yield return new WaitForSeconds(0.25f);
        while (rupe)
        {
            time += Time.deltaTime;
            scroll.content.anchoredPosition = new Vector2(scroll.content.anchoredPosition.x + contentBoxAnimation.Evaluate(time), 0);
            if (time > 1.25f)
            {
                isScroll = false;
                rupe = false;
            }
            yield return null;
        }
    }

    public void IsRupeStop()
    {
        rupe = false;
        isScroll = false;

        if (Statics.gameMode == GameMode.Quest)
        {
            IQuestable questable = QuestSceneManager.Instance.Questables[0];
            int sceneStep = questable.GetSceneStep();
            if (sceneStep == 0)
            {
                questable.SetDrawBrushOnOff(true);
            }
        }
        else if (Statics.gameMode == GameMode.Free)
        {
            IFreeable freeable = PaintSceneManager.Instance.Freeables[0];
            string sceneName = freeable.GetSceneName();
            if (sceneName == "pattern")
            {
                freeable.SetDrawBrushOnOff(true);
            }
        }
    }

    /// <summary>
    /// 선택한 매니큐어아이템에 대한 트리거
    /// </summary>
    /// <param name="manicureid"></param>
    public override void ManicureTriggerTriggerOn(int manicureid)
    {
        for (int i = 0; i < manicureConverters.Count; i++)
        {
            Image image = manicureConverters[i].GetComponent<Image>();
            RectTransform rectTransform = manicureConverters[i].GetComponent<RectTransform>();
            ManicureInfoData manicureInfoData = manicureConverters[i].ManicureInfoData;
            if (manicureInfoData.id == manicureid && manicureInfoData.type == "Manicure")
            {
                image.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureInfoData.useOnSpriteName);
                rectTransform.localRotation = Quaternion.Euler(0, 0, -17.5f);
                manicureConverters[i].IsSelect = true;
            }
            else
            {
                image.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureInfoData.useOffSpriteName);
                rectTransform.localRotation = Quaternion.identity;
                manicureConverters[i].IsSelect = false;
            }
        }
    }

    /// <summary>
    /// 필요한 매니큐어 데이터를 콜렉션 데이터와 비교 및 반환
    /// </summary>
    /// <returns></returns>
    public List<ManicureInfoData> GetManicureInfoDatas(string itemType)
    {
        //타입에 맞는 매니큐어 데이터 반환
        List<ManicureInfoData> manicureInfoDatas = DataManager.Instance.GetManicureInfoDataByType(itemType);
        //획득한 콜렉션 데이터 반환
        List<Collection> collections = PlayerDataManager.Instance.sl.collections;
        //필요한 매니큐어 데이터리스트
        List<ManicureInfoData> getManicureInfoDatas = new List<ManicureInfoData>();
        for (int i = 0; i < manicureInfoDatas.Count; i++)
        {
            for (int j = 0; j < collections.Count; j++)
            {
                if (manicureInfoDatas[i].id == collections[j].id)
                {
                    getManicureInfoDatas.Add(manicureInfoDatas[i]);
                }
            }
        }
        //각 타입별 획득한 콜렉션 및 퀘스트에 사용되는 아이템리스트 반환
        if (Statics.gameMode == GameMode.Quest)
        {
            QuestHandData questHandData = DataManager.Instance.GetQuestHandDataWithLevel(Statics.level);
            List<QuestFingerData> questFingerDatas = questHandData.questFingerDatas;
            //매니큐어
            if (itemType == "Manicure")
            {
                for (int i = 0; i < questFingerDatas.Count; i++)
                {
                    QuestFingerData questFingerData = questFingerDatas[i];
                    ManicureInfoData manicureInfoData = DataManager.Instance.GetManicureInfoDataByName(questFingerData.manicureName);
                    if (!getManicureInfoDatas.Contains(manicureInfoData))
                    {
                        getManicureInfoDatas.Add(manicureInfoData);
                    }
                }
                getManicureInfoDatas = getManicureInfoDatas.OrderBy(x => x.id).ToList();
            }
            //패턴
            else if (itemType == "Pattern")
            {
                List<QuestPatternData> questPatternDatas = new List<QuestPatternData>();
                for (int i = 0; i < questFingerDatas.Count; i++)
                {
                    questPatternDatas.Add(questFingerDatas[i].questPatternData);
                }
                for (int i = 0; i < questPatternDatas.Count; i++)
                {
                    QuestPatternData questPatternData = questPatternDatas[i];
                    ManicureInfoData manicureInfoData = DataManager.Instance.GetManicureInfoDataByName(questPatternData.patternSpriteString);
                    if (!getManicureInfoDatas.Contains(manicureInfoData))
                    {
                        getManicureInfoDatas.Add(manicureInfoData);
                    }
                }
                getManicureInfoDatas = getManicureInfoDatas.OrderBy(x => x.id).ToList();
            }
            //캐릭터스티커
            else if (itemType == "CharacterSticker")
            {
                List<QuestStickerData> questCharacterStickerDatas = new List<QuestStickerData>();
                for (int i = 0; i < questFingerDatas.Count; i++)
                {
                    for (int j = 0; j < questFingerDatas[i].questStickerDatas.Count; j++)
                    {
                        QuestStickerData questStickerData = questFingerDatas[i].questStickerDatas[j];
                        if (questStickerData.type == itemType)
                        {
                            questCharacterStickerDatas.Add(questStickerData);
                        }
                    }
                }
                for (int i = 0; i < questCharacterStickerDatas.Count; i++)
                {
                    QuestStickerData questStickerData = questCharacterStickerDatas[i];
                    ManicureInfoData manicureInfoData = DataManager.Instance.GetManicureInfoDataByName(questStickerData.stickerSpriteString);
                    if (!getManicureInfoDatas.Contains(manicureInfoData))
                    {
                        getManicureInfoDatas.Add(manicureInfoData);
                    }
                }
                getManicureInfoDatas = getManicureInfoDatas.OrderBy(x => x.id).ToList();
            }
            //스티커
            else if (itemType == "Sticker")
            {
                List<QuestStickerData> questStickerDatas = new List<QuestStickerData>();
                for (int i = 0; i < questFingerDatas.Count; i++)
                {
                    for (int j = 0; j < questFingerDatas[i].questStickerDatas.Count; j++)
                    {
                        QuestStickerData questStickerData = questFingerDatas[i].questStickerDatas[j];
                        if (questStickerData.type == itemType)
                        {
                            questStickerDatas.Add(questStickerData);
                        }
                    }
                }
                for (int i = 0; i < questStickerDatas.Count; i++)
                {
                    QuestStickerData questStickerData = questStickerDatas[i];
                    ManicureInfoData manicureInfoData = DataManager.Instance.GetManicureInfoDataByName(questStickerData.stickerSpriteString);
                    if (!getManicureInfoDatas.Contains(manicureInfoData))
                    {
                        getManicureInfoDatas.Add(manicureInfoData);
                    }
                }
                getManicureInfoDatas = getManicureInfoDatas.OrderBy(x => x.id).ToList();
            }
            //애니메이션스티커
            else
            {
                List<QuestStickerData> questAnimationStickerDatas = new List<QuestStickerData>();
                for (int i = 0; i < questFingerDatas.Count; i++)
                {
                    for (int j = 0; j < questFingerDatas[i].questStickerDatas.Count; j++)
                    {
                        QuestStickerData questStickerData = questFingerDatas[i].questStickerDatas[j];
                        if (questStickerData.type == itemType)
                        {
                            questAnimationStickerDatas.Add(questStickerData);
                        }
                    }
                }
                for (int i = 0; i < questAnimationStickerDatas.Count; i++)
                {
                    QuestStickerData questStickerData = questAnimationStickerDatas[i];
                    ManicureInfoData manicureInfoData = DataManager.Instance.GetManicureInfoDataByName(questStickerData.stickerSpriteString);
                    if (!getManicureInfoDatas.Contains(manicureInfoData))
                    {
                        getManicureInfoDatas.Add(manicureInfoData);
                    }
                }
                getManicureInfoDatas = getManicureInfoDatas.OrderBy(x => x.id).ToList();
            }
            return getManicureInfoDatas;
        }
        ////획득한 콜렉션 아이템리스트 반환
        else
        {
            return getManicureInfoDatas;
        }

        //꾸미기 제작용으로 현재 모든 아이탬이 프리씬에서는 등장함 테스트 종료시 위의 else문으로 대체_수정
        //else
        //{
        //    return manicureInfoDatas;
        //}

    }

    public void ItemArraySort()
    {
        List<int> itemId = new List<int>();
        List<GameObject> manicureItem = GetChildrens(itemParent.gameObject);
        for (int i = 0; i < manicureItem.Count; i++)
        {
            itemId.Add(manicureItem[i].GetComponent<ManicureConverter>().id);
        }
        itemId.Sort();

        for (int i = 0; i < manicureItem.Count; i++)
        {
            GameObject item = manicureItem.Find(x => x.GetComponent<ManicureConverter>().id == itemId[i]);
            item.transform.SetAsLastSibling();
        }
        
    }

    public void ChangePosition()
    {
        int childCount = itemParent.childCount;
        List<GameObject> manicureItem = GetChildrens(itemParent.gameObject);
        for (int i = 0; i < childCount; i++)
        {
            int count = Random.Range(0, childCount - 1);
            manicureItem[count].transform.SetAsFirstSibling();
        }

    }

    private List<GameObject> GetChildrens(GameObject root)
    {
        List<GameObject> childs = new List<GameObject>();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).GetComponent<ManicureItem>() != null)
            {
                childs.Add(root.transform.GetChild(i).gameObject);
            }
        }
        return childs;
    }

}