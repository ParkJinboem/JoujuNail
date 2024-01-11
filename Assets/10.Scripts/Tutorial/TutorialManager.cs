using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using OnDot.Util;

public class TutorialManager : PersistentSingleton<TutorialManager>
{
    [SerializeField] private TutorialScriptable tutorialTable;
    [SerializeField] private GameObject questBtn;
    public ScrollRect bgScroll;
    private GameObject presentTutorialObj;

    /// <summary>
    /// 튜토리얼 진행여부 확인 및 오브젝트 크기 조절 여부
    /// </summary>
    public void Init()
    {
        RectTransform rectTransform = questBtn.GetComponent<RectTransform>();
        if(PlayerDataManager.Instance.IsCompleteTutorial(1))
        {
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            questBtn.GetComponent<Button>().enabled = false;
            rectTransform.localScale = new Vector3(0, 0, 0);
            bgScroll.horizontal = false;
        }
    }

    /// <summary>
    /// 튜토리얼 클리어 여부 확인후 튜토리얼 진행
    /// </summary>
    /// <param name="tutorialID"></param>
    public void CheckClearTutorial(int tutorialID, Transform parentPos, GameObject targetObj)
    {
        bool isComplete = PlayerDataManager.Instance.IsCompleteTutorial(tutorialID);
        if (isComplete == true)
        {
            //Debug.Log("튜토리얼 " + tutorialID + "완료");
        }
        else
        {
            StartTutorial(tutorialID, parentPos, targetObj);
        }
    }

    public void StartTutorial(int tutorialID, Transform parentPos, GameObject targetObj)
    {
        TutorialLinker tutorialLinker = tutorialTable.GetTutorial(tutorialID);
        PlayTutorialStep(tutorialLinker, parentPos, targetObj);
    }

    private void PlayTutorialStep(TutorialLinker tutorialLinker, Transform parentPos, GameObject targetObj)
    {
        GameObject tutorialObject = Instantiate(tutorialLinker.tutorialObject, parentPos);
        tutorialObject.transform.SetAsFirstSibling();
        TutorialData tutorialData = tutorialObject.GetComponent<TutorialData>();
        tutorialData.Init(tutorialLinker.tutorialId, targetObj);
        presentTutorialObj = tutorialObject;
    }

    public void ClearTutorial(int index)
    {
        Tutorial tutorial = new Tutorial()
        {
            id = index
        };
        PlayerDataManager.Instance.AddTutorial(tutorial);
        if(index == 1)
        {
            bgScroll.horizontal = true;
        }
    }

    public void TutorialHide()
    {
        presentTutorialObj.SetActive(false);
    }
}