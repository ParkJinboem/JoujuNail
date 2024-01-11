//using UnityEngine;
//using System.Collections;
//using I2.Loc;
//using OnDot.Manager;
//#if UNITY_ANDROID
//using Google.Play.Review;
//#endif

//public class AppReviewManager : MonoBehaviour
//{
//    public static AppReviewManager Instance = null;

//    [SerializeField] int[] checkCounts;

//    private int checkCount;
//    private int maxCheckCount;
//    private bool isWriteReview;

//    void Awake()
//    {
//        if (Instance)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//    }

//    private void Start()
//    {
//        Reset();

//        checkCount = PlayerPrefs.GetInt("checkCount", 0);
//        maxCheckCount = checkCounts[checkCounts.Length - 1];
//        isWriteReview = PlayerPrefs.GetInt("isWriteReview", 0) == 1;
//    }

//    private void Reset()
//    {
//        PlayerPrefs.SetInt("checkCount", 1);
//        PlayerPrefs.SetInt("isWriteReview", 0);
//    }

//    private bool CheckAppReview()
//    {
//        // 카운트 증가
//        checkCount += 1;
//        if (checkCount > maxCheckCount)
//        {
//            checkCount = maxCheckCount;
//        }
//        PlayerPrefs.SetInt("checkCount", checkCount);

//        bool isShow = false;
//        if (!isWriteReview)
//        {
//            if (checkCount == maxCheckCount)
//            {
//                // 최대치
//                isShow = true;
//            }
//            else
//            {
//                // 앱 리뷰 띄울지 확인
//                for (int i = 0; i < checkCounts.Length; i++)
//                {
//                    if (checkCount == checkCounts[i])
//                    {
//                        isShow = true;
//                        break;
//                    }
//                }
//            }
//        }

//        return isShow;
//    }

//    public void ShowAppReview()
//    {
//        // 제한
//        if (!TutorialManager.Instance.IsCompleteMainTutorial)
//        {
//            // 메인 튜토리얼 미완료
//            return;
//        }

//        bool isShow = CheckAppReview();
//        if (!isShow)
//        {
//            return;       
//        }

//        #if UNITY_ANDROID
//            StartCoroutine(ICheckReview());
//        #elif UNITY_IPHONE
//            isWriteReview = true;
//            PlayerPrefs.SetInt("isWriteReview", 1);
//            PopupManager.Instance.Find(EPOPUP.CHOOISEUI);
//            var obj = PopupManager.Instance.GetObject(EPOPUP.CHOOISEUI);
//            obj.GetComponent<ChooiseUI>().Init(LocalizationManager.GetTermTranslation("AppReviewMsg"),
//                () =>
//                {
//                    Application.OpenURL(OnDotManager.Instance.StoreURL);
//                },
//                () =>
//                {

//                });
//        #endif
//    }

//    #if UNITY_ANDROID
//    private IEnumerator ICheckReview()
//    {
//        ReviewManager reviewManager = new ReviewManager();
//        var requestFlowOperation = reviewManager.RequestReviewFlow();
//        yield return requestFlowOperation;
//        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            yield break;
//        }

//        var launchFlowOperation = reviewManager.LaunchReviewFlow(requestFlowOperation.GetResult());
//        yield return launchFlowOperation;
//        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            yield break;
//        }

//        isWriteReview = true;
//        PlayerPrefs.SetInt("isWriteReview", 1);
//    }
//    #endif
//}
