using System;
using System.Collections;
using UnityEngine;

public class HandFootPositioner : MonoBehaviour
{
    [SerializeField] private AnimationCurve outPositionCurve;
    [SerializeField] private AnimationCurve inScaleCureve;

    [Serializable]
    public class InfoHand
    {
        public GameObject hand;
        public RectTransform handRect;
        public Vector3 handPosition;
        public Vector3 handScale;
        public AnimationCurve inXpositionCurve;
        public AnimationCurve inYpositionCurve;
    }
    public InfoHand infoHand;

    [Serializable]
    public class InfoFoot
    {
        public GameObject foot;
        public RectTransform footRect;
        public Vector3 footPosition;
        public Vector3 footScale;
        public AnimationCurve inXpositionCurve;
        public AnimationCurve inYpositionCurve;
    }
    public InfoFoot infoFoot;

    private float time;

    void OnEnable()
    {
        if (infoHand.hand.activeSelf == false)
        {
            infoHand.hand.SetActive(true);
        }
        if (infoFoot.foot.activeSelf == false)
        {
            infoFoot.foot.SetActive(true);
        }

        infoHand.handRect.anchoredPosition = infoHand.handPosition;
        infoHand.handRect.localScale = infoHand.handScale;
        infoFoot.footRect.anchoredPosition = infoFoot.footPosition;
        infoFoot.footRect.localScale = infoFoot.footScale;
    }

    public void Init()
    {
        time = 0;
    }

    /// <summary>
    /// 프리씬 손발 이동 및 스케일 값이동 애니메이션
    /// </summary>
    /// <param name="isHand"></param>
    /// <param name="isFoot"></param>
    public void ScalePosChanger(bool isHand, bool isFoot)
    {
        StopCoroutine(IScalePosChanger(isHand, isFoot));
        StartCoroutine(IScalePosChanger(isHand, isFoot));
    }

    IEnumerator IScalePosChanger(bool isHand, bool isFoot)
    {
        Vector2 handPos = infoHand.handPosition;
        Vector2 footPos = infoFoot.footPosition;

        if (isHand == true && isFoot == false)
        {
            while (time <= 0.75f)
            {
                time += Time.deltaTime * 1.5f;
                infoHand.handRect.anchoredPosition = new Vector2(infoHand.inXpositionCurve.Evaluate(time), infoHand.inYpositionCurve.Evaluate(time));
                float scale = inScaleCureve.Evaluate(time);
                infoHand.handRect.localScale = new Vector3(scale, scale, scale);
                infoFoot.footRect.anchoredPosition = new Vector2(footPos.x * outPositionCurve.Evaluate(time), footPos.y);
                yield return null;
            }
            infoFoot.foot.SetActive(false);
        }
        else if (isHand == false && isFoot == true)
        {
            while (time <= 0.75f)
            {
                time += Time.deltaTime * 1.5f;
                infoFoot.footRect.anchoredPosition = new Vector2(infoFoot.inXpositionCurve.Evaluate(time), infoFoot.inYpositionCurve.Evaluate(time));
                float scale = inScaleCureve.Evaluate(time);
                infoFoot.footRect.localScale = new Vector3(scale, scale, scale);
                infoHand.handRect.anchoredPosition = new Vector2(handPos.x * outPositionCurve.Evaluate(time), handPos.y);
                yield return null;
            }
            infoHand.hand.SetActive(false);
        }
    }
}