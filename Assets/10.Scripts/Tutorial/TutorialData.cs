using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialData : MonoBehaviour
{
    public RectTransform maskRect;
    public RectTransform panelRect;
    private GameObject tutorialTargetObj;
    [SerializeField] private TutorialHandAnim tutorialHandAnim;
    private int tutorialId;

    [SerializeField] private AnimationCurve btnActiveCurve;

    public void Init(int tutorialId, GameObject targetObject)
    {
        this.tutorialId = tutorialId;
        tutorialTargetObj = targetObject;
        tutorialHandAnim = GetComponent<TutorialHandAnim>();
        TutorialAction();
    }

    public void TutorialAction()
    {
        switch (tutorialId)
        {
            case 1:
                maskRect.transform.position = new Vector3(tutorialTargetObj.transform.position.x, tutorialTargetObj.transform.position.y, transform.position.z);
                panelRect.sizeDelta = new Vector2(Screen.width + (maskRect.transform.position.x * 2), Screen.height + (maskRect.transform.position.y * 2));                
                StartCoroutine(Tutorial1Action());
                break;
            case 2:
                //maskRect.transform.position = new Vector3(tutorialTargetObj.transform.position.x, tutorialTargetObj.transform.position.y, transform.position.z);
                //panelRect.sizeDelta = new Vector2(Screen.width + (maskRect.transform.position.x * 2), Screen.height + (maskRect.transform.position.y * 2));
                tutorialHandAnim.TutorialAction(false);
                this.transform.SetParent(tutorialTargetObj.transform);
                this.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                this.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                maskRect.anchoredPosition = Vector2.zero;
                maskRect.sizeDelta = new Vector2(640, 360);
                panelRect.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);

                //maskRect.transform.position = new Vector3(tutorialTargetObj.transform.position.x, tutorialTargetObj.transform.position.y, transform.position.z);
                //panelRect.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);
                //maskRect.transform.position = new Vector3(tutorialTargetObj.transform.position.x, tutorialTargetObj.transform.position.y, transform.position.z);
                //panelRect.sizeDelta = new Vector2(Screen.width + (maskRect.transform.position.x * 0.5f), Screen.height + (maskRect.transform.position.y * 0.5f));
                maskRect.GetComponent<Image>().enabled = false;
                StartCoroutine(Tutorial2Action());
                break;
            case 3:
                break;
        }
    }

    IEnumerator Tutorial1Action()
    {
        RectTransform rectTransform = tutorialTargetObj.GetComponent<RectTransform>();
        Image image = panelRect.GetComponent<Image>();
        float sizeTime = 0;
        float colorTime = 0;

        while (sizeTime <= 1)
        {
            sizeTime += Time.deltaTime;
            float scale = btnActiveCurve.Evaluate(sizeTime);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            if (1 < sizeTime)
            {
                rectTransform.localScale = new Vector3(1, 1, 1);
                break;
            }
            yield return null;
        }

        while (colorTime <= 1)
        {
            colorTime += Time.deltaTime * 2.5f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, colorTime);
            if (colorTime > 1)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                break;
            }
            yield return null;
        }
        tutorialTargetObj.GetComponent<Button>().enabled = true;
        tutorialHandAnim.TutorialAction(true);
    }

    IEnumerator Tutorial2Action()
    {
        yield return new WaitForSeconds(2.0f);
        QuestSceneManager.Instance.Tutorial2Action();
        yield return new WaitForSeconds(3.0f);
        maskRect.GetComponent<Image>().enabled = true;
        tutorialHandAnim.TutorialAction(true);
    }
}