using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectionMoveBehaviour : MonoBehaviour
{
    public abstract void TriggerMove(List<CollectionItem> collectionItems);

    private void OnEnable()
    {
        CollectionManager.TriggerMoveEvent += HandlerTriggerMove;
    }

    private void OnDisable()
    {
        CollectionManager.TriggerMoveEvent -= HandlerTriggerMove;
    }

    private void HandlerTriggerMove(List<CollectionItem> collectionItems)
    {
        TriggerMove(collectionItems);
    }
}
