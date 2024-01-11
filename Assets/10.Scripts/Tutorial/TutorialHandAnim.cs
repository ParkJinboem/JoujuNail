using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandAnim : MonoBehaviour
{
    public Animator TutorialAnim;

    public void TutorialAction(bool isAction)
    {
        TutorialAnim.SetBool("Action", isAction);
    }

    public void TutorialAction3()
    {
        TutorialAnim.SetTrigger("Action03");
    }
}
