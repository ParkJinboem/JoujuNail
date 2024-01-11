using UnityEngine;

public abstract class ManicureCheckBehaviour : MonoBehaviour
{
    public abstract void ManicureCheckTriggerOn(ManicureConverter manicureConverter);

    private void OnEnable()
    {
        ManicureConverter.ManicureCheckEvent += ManicureCheck;
    }

    private void OnDisable()
    {
        ManicureConverter.ManicureCheckEvent -= ManicureCheck;
    }

    private void ManicureCheck(ManicureConverter manicureConverter)
    {
        ManicureCheckTriggerOn(manicureConverter);
    }
}