using UnityEngine;

public class UICanvasAwakeDisableBehaviour : MonoBehaviour
{
    public GameObject[] objectsToDisable;

    void Awake()
    {
        DisableObjects();
    }

    private void DisableObjects()
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }
}
