using UnityEngine;

public class PatternScrollCheckBox : MonoBehaviour
{
    public PatternColorScroll patternColorScroll;
    public BoxCollider2D checkCol;

    private GameObject collisiontObject;
    public GameObject CollisionObject
    {
        get { return collisiontObject; }
    }

    private bool isTrigger;
    public bool IsTrigger
    {
        get { return isTrigger; }
        set { isTrigger = value; }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PatternColorConverter patternColorConverter = collision.gameObject.GetComponent<PatternColorConverter>();
        if(patternColorConverter == null)
        {
            return;
        }
        if(patternColorScroll.RotSoundCheck)
        {
            SoundManager.Instance.PlayEffectSound("PatternColor");
        }
        collisiontObject = collision.gameObject;
        patternColorScroll.SelectColor();
        isTrigger = true;
    }
}