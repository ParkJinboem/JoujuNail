using System;
using UnityEngine;
using UnityEngine.UI;

public class PatternSizeBar : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Text valueText;
    [SerializeField] private PatternScrollHand patternScrollHand;

    private int fingerToesNumber;
    private int alphaNumber;

    private float patternScale;

    private IFreeable freeable;

    private void OnEnable()
    {
        freeable = PaintSceneManager.Instance.Freeables[0];

        fingerToesNumber = freeable.SelectFingerIndex;
        scrollbar.value = freeable.PatternSizes[fingerToesNumber];

        alphaNumber = freeable.PatternAlphaNumbers[fingerToesNumber];

        patternScale = (scrollbar.value / 2) + 0.76f;

        patternScrollHand.animCheck = false;
        ChageAlpha();
        ChangePatternSize();
    }

    private void OnDisable()
    {
        
    }

    public void GuideHandInit()
    {
        patternScrollHand.gameObject.SetActive(true);
        StartCoroutine(patternScrollHand.StartMove());
    }

    public void BtnSoundPlay()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
    }

    /// <summary>
    /// 증가한 값을 체크하고 사이즈 변동 및 스크롤 값 변화
    /// </summary>
    public void ValueChangeCheck()
    {
        patternScale = (scrollbar.value / 2) + 0.76f;
        ChangeScrollValue();
        ChangePatternSize();
        //PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        patternScrollHand.StopHandCoroutine();
    }

    /// <summary>
    /// 값 증가
    /// </summary>
    public void PlusButteon()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        scrollbar.value += 0.125f;
        if (scrollbar.value >= 1f)
        {
            scrollbar.value = 1f;
        }
        patternScale = (scrollbar.value / 2) + 0.76f;
        ChangeScrollValue();
        ChangePatternSize();
        patternScrollHand.StopHandCoroutine();
    }

    /// <summary>
    /// 값 감소
    /// </summary>
    public void MinusButton()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        scrollbar.value -= 0.125f;
        if (scrollbar.value <= 0f)
        {
            scrollbar.value = 0f;
        }
        patternScale = (scrollbar.value / 2) + 0.76f;
        ChangeScrollValue();
        ChangePatternSize();
        patternScrollHand.StopHandCoroutine();
    }

    /// <summary>
    /// 스크롤바의 값에 따른 패턴사이즈리스트 값 변경
    /// </summary>
    private void ChangeScrollValue()
    {
        freeable.PatternSizes[fingerToesNumber] = scrollbar.value;
    }

    /// <summary>
    /// 값에 따른 패턴 사이즈 변경
    /// </summary>
    private void ChangePatternSize()
    {
        freeable.ChageSizePatternObject(patternScale);
    }

    /// <summary>
    /// 투명도 변화
    /// </summary>
    public void ChageAlphaNumber()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        alphaNumber++;
        if(alphaNumber > 4)
        {
            alphaNumber = 0;
        }

        ChageAlpha();
    }

    private void ChageAlpha()
    {
        freeable.ChageAlphaPatternObject(alphaNumber);
        valueText.text = (Mathf.Round(alphaNumber * 25)).ToString() + "%";
    }
}