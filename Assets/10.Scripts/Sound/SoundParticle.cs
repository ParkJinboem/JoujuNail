using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParticle : MonoBehaviour
{
    [SerializeField] private AudioClip born;
    [SerializeField] private AudioSource audioSource;

    public void SoundEvent()
    {
        if (PlayerDataManager.Instance.GetOptionData().isSound)
        {
            audioSource.PlayOneShot(born);
        }
    }

}
