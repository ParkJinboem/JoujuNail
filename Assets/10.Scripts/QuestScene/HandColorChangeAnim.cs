using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColorChangeAnim : MonoBehaviour
{
    [SerializeField] private Animator bounceAnim;
    public void StartAnim()
    {
        bounceAnim.SetTrigger("Bounce");
    }
}
