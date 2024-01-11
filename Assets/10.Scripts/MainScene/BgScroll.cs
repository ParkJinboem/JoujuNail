using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OnDot.System;

public class BgScroll : CollectionMoveBehaviour
{
    [SerializeField] private List<GameObject> bgList; //페이지 갯수
    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Animator homeButtonAnim;
    [SerializeField] private UIButtonAnimationController uiButtonAnimationController;

    private List<CollectionController> collectionControllers; //동적 생성한 아이템 리스트  
    public List<Vector2> collectionPos;

    private ScrollRect scroll; //스크롤뷰 오브젝트

    private float itemWidth; //아이템 너비
    private float itemHeight; //아이템 너비
    private int centerBgCount = 6;

    [SerializeField] private Vector2 itemPos;
    [SerializeField] private float initPos;
    [SerializeField] private float centerPos;
    [SerializeField] private float offsetPos;
    [SerializeField] private bool soundCheck;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
        collectionControllers = new List<CollectionController>();
    }

    void Start()
    {
        itemWidth = 1920;
        itemHeight = 1080;
        BgSetUp();

        collectionPos = new List<Vector2>();
        for (int i = 0; i < collectionControllers.Count; i++)
        {
            collectionPos.Add(collectionControllers[i].transform.position);
        }
        initPos = Mathf.Abs(bgList[4].transform.position.x);
        offsetPos = initPos / 2;
    }

    /// <summary>
    /// 배경 위치 및 크기 셋팅
    /// </summary>
    private void BgSetUp()
    {
        for (int i = 0; i < bgList.Count; i++)
        {
            RectTransform rectTransform = bgList[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(itemWidth, itemHeight);
            //rectTransform.localPosition = new Vector3(((i - 8) * rectTransform.sizeDelta.x) + 212, 0, 0);
            rectTransform.localPosition = new Vector3(((i - 7) * rectTransform.sizeDelta.x), 0, 0);
        }

        for (int i = 0; i < bgList.Count; i++)
        {
            List<CollectionController> childs = GetChildrens(bgList[i]);
            for (int k = 0; k < childs.Count; k++)
            {
                collectionControllers.Add(childs[k]);
            }
        }
    }

    /// <summary>
    /// 메인씬 스클로러가 움직일시 들어오는 함수(자동, 수동 포함)
    /// </summary>
    public void MoveBgScroll()
    {
        // 로딩씬 재생중에는 리턴
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }

        if(soundCheck)
        {
            SoundManager.Instance.PlayEffectSound("CollectionMove");
            soundCheck = false;
        }
        float contentX = scroll.content.anchoredPosition.x; //스크롤뷰의 콘텐츠오브젝트의 x좌표

        //배경 옮겨주는 포문
        foreach (GameObject item in bgList)
        {
            RelocationItem(item, contentX);
        }

        centerPos = bgList[6].transform.position.x;
        if(centerPos > initPos - offsetPos && centerPos < initPos + offsetPos)
        {
            HomeButtonAnimation(false);
        }
        else
        {
            HomeButtonAnimation(true);
        }
    }

    /// <summary>
    /// 홈 메인으로 이동버튼
    /// </summary>
    public void GoHome()
    {
        StopAllCoroutines();
        StartCoroutine(IReturnMove(false));
    }

    //계산에서 옮겨줌_ +212는 배경 이미지사이즈가 1920이 아니기때문에 보정해줌
    IEnumerator IReturnMove(bool isClear)
    {
        bool isMove = true;
        scroll.decelerationRate = 0;
        while (isMove)
        {
            Vector3 targetPos = scrollContent.transform.position - (bgList[centerBgCount].transform.position - canvasRect.transform.position);
            //targetPos = new Vector3(targetPos.x + 212, scrollContent.transform.position.y, 0);
            targetPos = new Vector3(targetPos.x, scrollContent.transform.position.y, 0);
            scrollContent.transform.position = Vector3.MoveTowards(scrollContent.transform.position, targetPos, Time.deltaTime * 10000);
            float distance = (scrollContent.transform.position - targetPos).magnitude;
            if (distance < 0.1f)
            {
                scroll.decelerationRate = 0.135f;
                isMove = false;
            }
            yield return null;
        }

        //홈버튼 이동끝나면 애니메이션 재생
        if(!PlayerDataManager.Instance.IsCompleteTutorial(3) && isClear)
        {
            uiButtonAnimationController.TutorialAnimPlay();
        }
    }

    /// <summary>
    /// 홈버튼 애니메이션
    /// </summary>
    /// <param name="isShow"></param>
    private void HomeButtonAnimation(bool isShow)
    {
        homeButtonAnim.SetBool("isShow", isShow);
    }

    /// <summary>
    /// 콜렉션아이템이 들어갈곳으로 이동
    /// </summary>
    /// <param name="collectionItems"></param>
    public override void TriggerMove(List<CollectionItem> collectionItems)
    {
        StopAllCoroutines();
        StartCoroutine(ITriggerMove(collectionItems));
    }

    IEnumerator ITriggerMove(List<CollectionItem> collectionItems)
    {
        int time = 0;
        int count = 0;
        SoundManager.Instance.PlayEffectSound("CollectionMove");
        while (count < collectionItems.Count)
        {
            for (int i = 0; i < collectionControllers.Count; i++)
            {
                if (collectionItems[count].ManicureData.collectionNumber == collectionControllers[i].collectionNumber)
                {
                    yield return new WaitForSeconds(time);
                    Vector3 targetPos = scrollContent.transform.position - (collectionControllers[i].transform.position - canvasRect.transform.position);
                    targetPos = new Vector3(targetPos.x, scrollContent.transform.position.y, 0);
                    Vector3 startPos = scrollContent.transform.position;

                    scrollContent.transform.position = Vector3.MoveTowards(startPos, targetPos, Time.deltaTime * 20000);

                    MoveBgScroll();

                    float distance = (scrollContent.transform.position - targetPos).magnitude;
                    time = 0;
                    if (distance < 0.25f)
                    {
                        for (int k = 0; k < collectionControllers[i].CollectionBehaviours.Count; k++)
                        {
                            if (collectionItems[count].ManicureData.id == collectionControllers[i].CollectionBehaviours[k].ManicureData.id)
                            {
                                Vector3 targetVec = collectionControllers[i].CollectionBehaviours[k].transform.position;
                                yield return null;
                                CollectionManager.Instance.CollectionItemAnimate(collectionItems[count], targetVec);
                                yield return new WaitForSeconds(CollectionManager.Instance.Duration);
                                collectionControllers[i].CollectionBehaviours[k].Init(true, collectionItems[count].ManicureData.baseColor);
                                collectionControllers[i].CollectionBehaviours[k].ParticleOn();
                                soundCheck = true;
                                time = 1;
                                
                            }
                        }
                        count++;
                        if (count == collectionItems.Count)
                        {
                            SoundManager.Instance.PlayEffectSound("CollectionMove");
                            UIManager.Instance.ButtonOnOff(true);
                            StopCoroutine(ITriggerMove(collectionItems));
                            yield return new WaitForSeconds(1.0f);
                            StartCoroutine(IReturnMove(true));
                            break;
                        }
                        yield return null;
                    }
                }
            }
            yield return null;
        }
    }

    private int RelocationItem(GameObject item, float contentX)
    {
        // 왼쪽에 붙이기
        if (item.transform.localPosition.x + contentX > itemWidth * 8 + itemWidth / 2)
        {
            item.transform.localPosition -= new Vector3((bgList.Count * itemWidth), 0);
            return 1;
        }
        // 오른쪽에 붙이기
        else if (item.transform.localPosition.x + contentX < -(itemWidth * 7 + itemWidth / 2))
        {
            item.transform.localPosition += new Vector3((bgList.Count * itemWidth), 0);
            return 2;
        }
        return 0;
    }

    private List<CollectionController> GetChildrens(GameObject root)
    {
        List<CollectionController> childs = new List<CollectionController>();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).GetComponent<CollectionController>() != null)
            {
                childs.Add(root.transform.GetChild(i).GetComponent<CollectionController>());
            }
        }
        return childs;
    }
}