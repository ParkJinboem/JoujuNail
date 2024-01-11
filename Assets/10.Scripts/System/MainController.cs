using System.Collections;
using OnDot.System;
using UnityEngine;
using UnityEngine.Playables;

public class MainController : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Transform manager;

    public bool isTimeLine;
    [Header("Tutorial")]
    public Transform tutorial1Parent;
    public GameObject tutorial1Target;

    void Start()
    {
        AddressableManager.Instance.HandlerAssetLoadCompleted(AddressableManager.Instance.FadeInSelectScene);

        DataManager.Instance.Init();
        CollectionManager.Instance.Init();
        HandFootTextureMaker.Instance.Init();
        MainSceneController.Instance.Set();
        TutorialManager.Instance.Init();
        SoundManager.Instance.MainInit(manager);

        isTimeLine = true;
    }

    void Update()
    {
        if (isTimeLine == false)
        {
            return;
        }

        //로딩이 끝나면 타임라인 재생
        if (ScreenFaderManager.Instance.canvasAlphaNumber() > 0.8f)
        {
            return;
        }

        //타임라인은 한번만 재생
        if (isTimeLine == true)
        {
            IntroAnimation();
            isTimeLine = false;
        }
    }

    /// <summary>
    /// 타임라인 재생
    /// </summary>
    private void IntroAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(IIntroAnimation());
    }

    IEnumerator IIntroAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        playableDirector.Play();
        yield return new WaitForSeconds(2f);

        //튜토리얼1을 진행했는지 체크
        TutorialManager.Instance.CheckClearTutorial(1, tutorial1Parent, tutorial1Target);
    }
}