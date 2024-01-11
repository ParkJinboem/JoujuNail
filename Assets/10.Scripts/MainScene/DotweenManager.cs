using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DotweenManager : MonoSingleton<DotweenManager>
{
    public bool isTweening;

    public void DoPopupOpen(float startVal, float endVal, float time, Transform targetObj)
    {
        isTweening = true;
        Vector3 startScale = new Vector3(startVal, startVal, 1);
        targetObj.localScale = startScale;
        targetObj.DOScale(endVal, time).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public IEnumerator DoFade(float endVal, float time, GameObject targetObj)
    {
        targetObj.SetActive(true);
        isTweening = true;
        Image popupImage = targetObj.GetComponent<Image>();
        popupImage.color = new Color(.3f, .3f, .3f, 0);
        popupImage.DOFade(endVal, time);
        yield return new WaitForSeconds((time) + 1f);
        popupImage.DOFade(0f, time).OnComplete(() =>
        {
            targetObj.SetActive(false);
            isTweening = false;
        });
    }

    public void DoFadeImage(float startAlpha, float endAlpha, float time, GameObject targetObj)
    {
        targetObj.SetActive(true);
        isTweening = true;
        Image popupImage = targetObj.GetComponent<Image>();
        popupImage.color = new Color(popupImage.color.r, popupImage.color.g, popupImage.color.b, startAlpha);
        popupImage.DOFade(endAlpha, time).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoFadeImage(float startAlpha, float endAlpha, float time, RawImage targetObj)
    {
        targetObj.gameObject.SetActive(true);
        isTweening = true;
        RawImage popupImage = targetObj.GetComponent<RawImage>();
        popupImage.color = new Color(popupImage.color.r, popupImage.color.g, popupImage.color.b, startAlpha);
        popupImage.DOFade(endAlpha, time).OnComplete(() =>
        {
            isTweening = false;
        });
    }


    public void DoSizeImage(float startScale, float endScale, float time, GameObject targetObj)
    {
        targetObj.SetActive(true);
        isTweening = true;
        targetObj.transform.localScale = new Vector3(startScale, startScale, 1);
        targetObj.transform.DOScale(endScale, time).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public IEnumerator DoTextFade(float endVal, float time, GameObject targetObj)
    {
        targetObj.SetActive(true);
        isTweening = true;
        Text popupImage = targetObj.GetComponent<Text>();
        popupImage.DOFade(endVal, time);
        yield return new WaitForSeconds((time) + .8f);
        popupImage.DOFade(0f, time).OnComplete(() =>
        {
            targetObj.SetActive(false);
            isTweening = false;
        });
    }

    public void DoLocalMoveX(float x, float time, Transform moveObj)
    {
        isTweening = true;

        moveObj.DOLocalMoveX(x, time).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoLocalMoveX(float x, float time, RectTransform moveObj)
    {
        isTweening = true;

        moveObj.DOAnchorPosX(x, time).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoLocalMoveY(float y, float time, Transform moveObj)
    {
        isTweening = true;

        moveObj.DOLocalMoveY(y, time).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoLocalMoveY(float y, float time, RectTransform moveObj)
    {
        isTweening = true;

        moveObj.DOAnchorPosY(y, time).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoLocalMove(float x, float y, float time, Transform moveObj)
    {
        isTweening = true;

        moveObj.DOLocalMove(new Vector3(x, y, 1), time).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DoLocalMove(float x, float y, float time, RectTransform moveObj)
    {
        isTweening = true;

        moveObj.DOAnchorPos(new Vector3(x, y, 1), time).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DORotate(float z, float time, Transform moveObj)
    {
        isTweening = true;

        moveObj.DORotate(new Vector3(0, 0, z), time).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void DORotate(float z, float time, RectTransform moveObj)
    {
        isTweening = true;

        moveObj.DORotate(new Vector3(0, 0, z), time).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isTweening = false;
        });
    }
}
