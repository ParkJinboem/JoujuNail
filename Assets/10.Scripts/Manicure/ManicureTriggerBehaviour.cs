using UnityEngine;

public abstract class ManicureTriggerBehaviour : MonoBehaviour
{
    public abstract void ManicureTriggerTriggerOn(int manicureid);

    private void OnEnable()
    {
        ManicureConverter.ManicureTriggerDownEvent += ManicureTriggerDown;
    }

    private void OnDisable()
    {
        ManicureConverter.ManicureTriggerDownEvent -= ManicureTriggerDown;
    }

    private void ManicureTriggerDown(int manicureid)
    {
        ManicureTriggerTriggerOn(manicureid);
    }
}
