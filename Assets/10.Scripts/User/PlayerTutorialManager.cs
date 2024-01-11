using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    public delegate void PlayerTutorialAddedHandler(int tutorialId);
    public static event PlayerTutorialAddedHandler OnPlayerTutorialAdded;

    public delegate void PlayerTutorialChangedHandler(int tutorialId);
    public static event PlayerTutorialChangedHandler OnPlayerTutorialChanged;

    /// <summary>
    /// 튜토리얼 정보 반환
    /// </summary>
    /// <param name="tutorialId"></param>
    /// <returns></returns>
    public Tutorial GetTutorialById(int tutorialId)
    {
        Tutorial tutorial = sl.tutorials.Find(x => x.id == tutorialId);
        return tutorial;
    }

    /// <summary>
    /// 튜토리얼 완료 여부
    /// </summary>
    /// <param name="tutorialId"></param>
    /// <returns></returns>
    public bool IsCompleteTutorial(int tutorialId)
    {
        Tutorial tutorial = GetTutorialById(tutorialId);
        if (tutorial != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 클리어한 튜토리얼 정보 추가
    /// </summary>
    /// <param name="tutorial"></param>
    public void AddTutorial(Tutorial tutorial)
    {
        if (IsHaveHand(tutorial.id))
        {
            return;
        }

        SaveTutorialData(tutorial);

        OnPlayerTutorialAdded?.Invoke(tutorial.id);
    }

    /// <summary>
    /// 튜토리얼 정보 저장
    /// </summary>
    /// <param name="tutorial"></param>
    public void SaveTutorialData(Tutorial tutorial)
    {
        int index = sl.tutorials.FindIndex(x => x.id == tutorial.id);
        if (index == -1)
        {
            sl.tutorials.Add(tutorial);
        }
        else
        {
            sl.tutorials[index] = tutorial;
        }

        isTutorialSave = true;
        SaveData();

        OnPlayerTutorialChanged?.Invoke(tutorial.id);
    }

  
}