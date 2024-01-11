using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundVolume : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Scrollbar volumeScroll;
    private float soundVolume;

    public void OnDrag(PointerEventData eventData)
    {
        soundVolume = volumeScroll.value;
        SoundManager.Instance.ChangeVolume(soundVolume);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        PlayerDataManager.Instance.SaveSoundVolume(soundVolume);
    }
}
