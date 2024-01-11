using System.Collections.Generic;
using OnDot.System;
using UnityEngine;
using System.Linq;

public class AlbumSceneManager : MonoSingleton<AlbumSceneManager>
{
    [SerializeField] private AlbumSpawner albumSpawner;
    [SerializeField] private GameObject albumSlotPrefab;
    [SerializeField] private Transform albumSlotParent;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private AlbumSceneItem albumSceneItem;
    [SerializeField] private ScreenShot screenShot;

    private List<Hand> hands;

    public List<AlbumSlot> slots;
    public List<AlbumSlot> Slots
    {
        get { return slots; }
        set { slots = value; }
    }

    private Album album;
    public Album Album
    {
        get { return album; }
        set { album = value; }
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    public void Init()
    {
        hands = new List<Hand>();
        hands = PlayerDataManager.Instance.GetHandData();

        slots = new List<AlbumSlot>();
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.sizeDelta = Vector2.zero;

        if (hands != null)
        {
            albumSpawner.Init(hands);
        }
    }

    /// <summary>
    /// 앨범슬롯에서 앨범으로 이동
    /// </summary>
    /// <param name="id"></param>
    public void MoveToAlbum(int id)
    {
        //로딩완료 전까지는 실행을 못하게함
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        albumSpawner.CreateAlbum(id);
    }

    /// <summary>
    /// 메인씬 이동
    /// </summary>
    public void MoveToMainScene()
    {
        //로딩완료 전까지는 실행을 못하게함
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        MainSceneController.Instance.MainSceneOn(false);

        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].Hide();
        }

        albumSceneItem.Hide();
    }

    /// <summary>
    /// 앨범 데이터 삭제
    /// </summary>
    /// <param name="id"></param>
    public void DeleteAlbum(int id)
    {
        if(album != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Id == id)
                {
                    slots[i].Hide();
                    slots.Remove(slots[i]);
                    album.Hide();
                    PlayerDataManager.Instance.DeleteHand(id);
                }
            }
        }
    }

    /// <summary>
    /// 스크린샷
    /// </summary>
    public void ScreenShot()
    {
        screenShot.ClickScreenShot();
    }
}