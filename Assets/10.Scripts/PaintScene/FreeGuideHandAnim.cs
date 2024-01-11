using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeGuideHandAnim : MonoBehaviour
{
    public enum HandStatus
    {
        None,
        Scroll,
        Pattern,
        Finger,
        Idle,
    }

    [SerializeField] private GameObject guideHand;
    [SerializeField] private Animator guideHandAnim;
    [SerializeField] private HandStatus handStatus;
    [SerializeField] private HandStatus beforeStatus;
    
    public void SetUp(int actionNum)
    {
        beforeStatus = handStatus;
        
        switch (actionNum)
        {
            case 1:
                handStatus = HandStatus.Scroll;
                break;
            case 2:
                handStatus = HandStatus.Pattern;
                break;
            case 3:
                handStatus = HandStatus.Finger;
                break;
            case 4:
                handStatus = HandStatus.Idle;
                break;
            default:
                handStatus = beforeStatus;
                break;
        }
        StartCoroutine(AnimAction(handStatus));
    }

    public IEnumerator AnimAction(HandStatus status)
    {
        guideHandAnim.SetTrigger("Idle");
        guideHandAnim.Rebind();
        
        yield return new WaitForSeconds(0.5f);
        switch (status)
        {
            case HandStatus.Scroll:
                guideHandAnim.SetTrigger("Scroll");
                break;
            //case HandStatus.Pattern:
            //    guideHandAnim.SetTrigger("Pattern");
                break;
            case HandStatus.Finger:
                guideHandAnim.SetTrigger("Finger");
                break;
            case HandStatus.Idle:
                guideHandAnim.SetTrigger("Idle");
                guideHandAnim.ResetTrigger("Scroll");
                guideHandAnim.ResetTrigger("Pattern");
                guideHandAnim.ResetTrigger("Finger");
                break;
        }
    }

    //AnimationEvent
    public void ResetIdle()
    {
        guideHandAnim.ResetTrigger("Idle");
    }
}
