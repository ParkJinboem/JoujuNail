using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class CollectionManager : MonoSingleton<CollectionManager>
{
    public delegate void TriggerMoveHandler(List<CollectionItem> collectionItems);
    public static event TriggerMoveHandler TriggerMoveEvent;

    [SerializeField] private CollectionSpawner collectionSpawner;
    [SerializeField] private List<CollectionController> collectionControllers;

    private List<int> indexs;
    
    private List<ManicureData> manicureDatas;
    private List<CollectionItem> collectionItems;
    public List<CollectionItem> CollectionItems
    {
        get { return collectionItems; }
        set { collectionItems = value; }
    }

    [SerializeField] private GameObject particle;
    private GameObject particleObject;

    [Header("Animation Setting")]
    private float duration;
    public float Duration
    {
        get { return duration; }
    }
    private Button giftBoxBtn;
    public Button GiftBoxBtn
    {
        set { giftBoxBtn = value; }
    }
    private int count;
    private int collectionCount;
    private int collectionId;
    private int currentPageNumber;

    [Header("CollectionCreatePos")]
    public List<CollectionPos> collectionPos;
    [Serializable]
    public class CollectionPos
    {
        public int collectionCount;
        public List<Vector3> pos;
    }

    private bool isCollection;
    public bool IsCollection
    {
        get { return isCollection; }
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    public void Init()
    {
        manicureDatas = new List<ManicureData>();
        manicureDatas = DataManager.Instance.ManicureDatas;
        for (int i = 0; i < manicureDatas.Count; i++)
        {
            //초기 매니큐어 데이터의 isHave가 true인경우 콜렉션에 추가
            if (manicureDatas[i].isHave == true)
            {
                Collection collection = new Collection()
                {
                    id = manicureDatas[i].id
                };
                PlayerDataManager.Instance.AddCollection(collection);
            }
        }
        for (int i = 0; i < collectionControllers.Count; i++)
        {
            collectionControllers[i].Init();
        }

        //컬렉션 테스트시 게임 시작하자마자 콜렉션 스타트 해줌
        //GetCollectionIndexs();
    }

    /// <summary>
    /// 획득할 컬렉션의 갯수 및 아이디 반환 
    /// </summary>
    //테스트용_테스트 완료후 아래 함수로 사용할 것_231127 박진범
    //public void GetCollectionIndexs()
    //{
    //    //테스트용 데이터 수집
    //    List<ManicureData> rewardDatas = DataManager.Instance.ManicureDatas;

    //    collectionCount = 3;
    //    indexs = new List<int>();
    //    for (int i = 0; i < collectionCount; i++)
    //    {
    //        int randomReward = UnityEngine.Random.Range(0, 400);
    //        int index = rewardDatas[randomReward].id;
    //        //if (indexs.Contains(index) || PlayerDataManager.Instance.IsHaveCollection(index) == true)
    //        //{
    //        //    Debug.Log(rewardDatas[i].spriteName + "은중복된 보상 입니다.");
    //        //}
    //        //else
    //        {
    //            indexs.Add(index);
    //            Collection collection = new Collection()
    //            {
    //                id = index
    //            };
    //            PlayerDataManager.Instance.AddCollection(collection);
    //        }
    //    }

    //    //모든 보상을 받았다면 팝업창을 생성
    //    if (indexs.Count == 0)
    //    {
    //        UIManager.Instance.AllClear();
    //    }
    //    else
    //    {
    //        GetCollectionItem();
    //        isCollection = true;
    //    }
    //}

    public void GetCollectionIndexs()
    {
        List<RewardData> rewardDatas = DataManager.Instance.GetRewardInfoDatabyLevel(Statics.level);
        collectionCount = rewardDatas.Count;
        indexs = new List<int>();
        for (int i = 0; i < collectionCount; i++)
        {
            int index = rewardDatas[i].id;
            if (indexs.Contains(index) || PlayerDataManager.Instance.IsHaveCollection(index) == true)
            {
                Debug.Log("Error :" + rewardDatas[i].spriteName + "은중복된 보상 입니다.");
            }
            else
            {
                indexs.Add(index);
                Collection collection = new Collection()
                {
                    id = index
                };
                PlayerDataManager.Instance.AddCollection(collection);
            }
        }

        //모든 보상을 받았다면 팝업창을 생성
        if (indexs.Count == 0)
        {
            PopupManager.Instance.ShowAlarm("CollectionAllClear");
        }
        else
        {
            GetCollectionItem();
            isCollection = true;
        }
    }

    /// <summary>
    /// 획득한 갯수의 맞춰 컬렉션 선별 및 버튼 셋, 파티클 셋팅
    /// </summary>
    private void GetCollectionItem()
    {
        collectionItems = new List<CollectionItem>();
        collectionSpawner.CreateCollection(indexs);
        for (int i = 0; i < collectionItems.Count; i++)
        {
            collectionItems[i].Hide();
        }

        UIManager.Instance.ButtonOnOff(false);
        collectionSpawner.CreateGiftBox();
        giftBoxBtn.onClick.AddListener(() => OnClicBoxAnimation());

        particleObject = Instantiate(particle, giftBoxBtn.transform.parent);
        particleObject.transform.SetAsFirstSibling();
        RectTransform rectTransform = particleObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;

        isCollection = false;
    }

    /// <summary>
    /// 콜렉션 박스 클릭 애니메이션 
    /// </summary>
    private void OnClicBoxAnimation()
    {
        SoundManager.Instance.PlayEffectSound("CollectionClick");
        Animator animator = giftBoxBtn.GetComponent<Animator>();
        animator.SetTrigger("isBouce");
        giftBoxBtn.GetComponent<GiftBoxItem>().ParticlePlay();
        count++;
        if (count == 3)
        {
            SoundManager.Instance.PlayEffectSound("CollectionBoom");
            CollectionBoxAnimate();
            count = 0;
        }
    }

    /// <summary>
    /// 콜렉션 아이템 이동 애니메이션
    /// </summary>
    private void CollectionBoxAnimate()
    {
        StopCoroutine(ICollectionBoxAnimate());
        StartCoroutine(ICollectionBoxAnimate());
    }

    IEnumerator ICollectionBoxAnimate()
    {
        for (int i = 0; i < collectionItems.Count; i++)
        {
            collectionItems[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < collectionItems.Count; i++)
        {
            for (int k = 0; k < collectionPos.Count; k++)
            {
                if (collectionPos[k].collectionCount == collectionItems.Count)
                {
                    duration = 0.5f;
                    float xIncrease;
                    float yIncrease;
                    if (Screen.width / 1920 < 1)
                    {
                        xIncrease = 1;
                    }
                    else
                    {
                        xIncrease = Screen.width / 1920;
                    }
                    if (Screen.height / 1080 < 1)
                    {
                        yIncrease = 1;
                    }
                    else
                    {
                        yIncrease = Screen.height / 1080;
                    }

                    Vector3 pos = new Vector3(Screen.width / 2, Screen.height / 2, 0) + new Vector3(collectionPos[k].pos[i].x * xIncrease, collectionPos[k].pos[i].y * yIncrease, 0);
                    collectionItems[i].transform.DOMove(pos, duration);
                }
            }
        }
        yield return new WaitForSeconds(0.25f);
        Destroy(particleObject);
        yield return new WaitForSeconds(1.25f);
        if(giftBoxBtn != null)
        {
            Destroy(giftBoxBtn.gameObject);
        }

        if (collectionItems.Count == collectionCount)
        {
            TriggerMoveEvent?.Invoke(collectionItems);
        }
    }

    /// <summary>
    /// 콜렉션 아이템 애니메이션 
    /// </summary>
    /// <param name="collectionItem"></param>
    /// <param name="targetPos"></param>
    public void CollectionItemAnimate(CollectionItem collectionItem, Vector3 targetPos)
    {
        duration = 0.5f;
        collectionItem.transform.DOMove(targetPos, duration);
        collectionItem.transform.DOScale(1, duration);
        DOScaleAnim(collectionItem);
    }

    /// <summary>
    /// 콜렉션 아이템 스케일 변동 애니메이션
    /// </summary>
    /// <param name="collectionItem"></param>
    private void DOScaleAnim(CollectionItem collectionItem)
    {
        StopCoroutine(IDOScaleAnim(collectionItem));
        StartCoroutine(IDOScaleAnim(collectionItem));
    }

    IEnumerator IDOScaleAnim(CollectionItem collectionItem)
    {
        yield return new WaitForSeconds(duration);
        collectionId = collectionItem.ManicureData.id;
        currentPageNumber = collectionItem.ManicureData.collectionNumber;
        CollectionItemActive(false);
        collectionItem.transform.DOScale(1.5f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        collectionItem.transform.DOScale(1, 0.25f).OnComplete(() =>
        {
            collectionItem.Hide();
        }) ;
        yield return new WaitForSeconds(0.25f);
        CollectionItemActive(true);
    }

    /// <summary>
    /// 콜렉션 아이템 활성화여부
    /// </summary>
    /// <param name="isOn"></param>
    private void CollectionItemActive(bool isOn)
    {
        for (int i = 0; i < collectionControllers.Count; i++)
        {
            if(currentPageNumber == collectionControllers[i].collectionNumber)
            {
                CollectionBehaviour collectionBehaviour = collectionControllers[i].GetCollectionBehaviour(collectionId);
                collectionBehaviour.gameObject.SetActive(isOn);
                break;
            }
        }

        if (isOn == true)
        {
            StopAllCoroutines();
        }
    }
}