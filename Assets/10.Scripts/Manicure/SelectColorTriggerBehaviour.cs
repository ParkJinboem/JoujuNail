using UnityEngine;

public abstract class SelectColorTriggerBehaviour : MonoBehaviour
{
    public abstract void TriggerOn(int index);

    private void OnEnable()
    {
        PatternColorConverter.TriggerOnEvent += HandlerTriggerOn;
    }

    private void OnDisable()
    {
        PatternColorConverter.TriggerOnEvent -= HandlerTriggerOn;
    }

    private void HandlerTriggerOn(int index)
    {
        TriggerOn(index);
    }
}