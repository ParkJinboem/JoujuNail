using UnityEngine;

public abstract class CollectionItemBehaviour : MonoBehaviour
{
    public abstract void TriggerOn(CollectionItem collectionItem);

    private void OnEnable()
    {
        CollectionSpawner.TriggerDownEvent += HandlerTriggerDown;
    }

    private void OnDisable()
    {
        CollectionSpawner.TriggerDownEvent -= HandlerTriggerDown;
    }

    private void HandlerTriggerDown(CollectionItem collectionItem)
    {
        TriggerOn(collectionItem);
    }
}
