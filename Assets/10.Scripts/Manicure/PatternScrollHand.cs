using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PatternScrollHand : MonoBehaviour
{
    private RectTransform rect;
    public bool animCheck;
    [SerializeField] private Image scrollHand;
    [SerializeField] private Sprite[] handSprite;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        animCheck = false;
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        rect.anchoredPosition = new Vector2(4, -10);
        StopAllCoroutines();
        animCheck = true;
        gameObject.SetActive(false);
    }

    public IEnumerator StartMove()
    {
        if(animCheck)
        {
            rect.anchoredPosition = new Vector2(4, -10);
            gameObject.SetActive(false);
        }
        else
        {
            rect.anchoredPosition = new Vector2(4, -10);
            animCheck = true;
            while (gameObject.activeSelf)
            {
                scrollHand.sprite = handSprite[0];
                yield return new WaitForSeconds(0.5f);
                scrollHand.sprite = handSprite[1];
                yield return new WaitForSeconds(0.5f);
                scrollHand.sprite = handSprite[0];
                DotweenManager.Instance.DoLocalMoveX(-130, 2.5f, rect);
                yield return new WaitForSeconds(2.5f);
                DotweenManager.Instance.DoLocalMoveX(150, 5, rect);
                yield return new WaitForSeconds(5.0f);
                DotweenManager.Instance.DoLocalMoveX(4, 2.5f, rect);
                yield return new WaitForSeconds(2.5f);
            }
        }
      
    }

    public void StopHandCoroutine()
    {
        gameObject.SetActive(false);
    }
}
