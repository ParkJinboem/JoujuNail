using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialLinker
{
    public int tutorialId;
    public GameObject tutorialObject;
    public bool isClear;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TutorialTable")]
public class TutorialScriptable : ScriptableObject
{
    [SerializeField] private List<TutorialLinker> KeyByLinkTutorials;

    public TutorialLinker GetTutorial(int tutorialId)
    {
        return KeyByLinkTutorials.Find(x => x.tutorialId == tutorialId);
    }

    public List<TutorialLinker> GetTutorialLinkers()
    {
        return KeyByLinkTutorials;
    }
}