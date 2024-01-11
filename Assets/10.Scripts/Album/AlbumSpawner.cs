using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumSpawner : MonoBehaviour
{
    [SerializeField] private Transform albumSlotParent;

    [SerializeField] private AlbumSlotPool albumSlotPool;
    [SerializeField] private AlbumPool albumPool;
    [SerializeField] private GameObject selectedSlot;

    private List<Hand> hands;

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    /// <param name="hands"></param>
    public void Init(List<Hand> hands)
    {
        this.hands = new List<Hand>();
        this.hands = hands;
        CreateAlbumSlot();
    }

    /// <summary>
    /// 앨범슬롯 생성 
    /// </summary>
    /// <param name="hands"></param>
    private void CreateAlbumSlot()
    {
        //저장되어잇는 손,발의 갯수만큼 앨범슬롯을 생성
        for(int i = 0; i < hands.Count; i++)
        {
            AlbumSlotSapwn(hands[i]);
        }
    }

    private void AlbumSlotSapwn(Hand hand)
    {
        GameObject albumSlotObject = albumSlotPool.Pop(albumSlotParent.position).instance;
        AlbumSlot albumSlot = albumSlotObject.GetComponent<AlbumSlot>();
        albumSlot.Init(hand);
        AlbumSceneManager.Instance.Slots.Add(albumSlot);
        albumSlot.transform.SetAsFirstSibling();
    }

    /// <summary>
    /// 앨범 생성 
    /// </summary>
    /// <param name="id"></param>
    public void CreateAlbum(int id)
    {
        Hand loadHand = hands.Find(x => x.saveId == id);
        if(loadHand == null)
        {
            Debug.Log("저장된 아이디가 없습니다.");
        }
        else
        {
            AlbumSpawn(id);
        }
    }

    //private void AlbumSpawn(Hand hand)
    //{
    //    GameObject albumObject = albumPool.Pop(transform.position).instance;
    //    Album album = albumObject.GetComponent<Album>();
    //    album.Init(hand);
    //    AlbumSceneManager.Instance.Album = album;
    //}

    
    private void AlbumSpawn(int saveId)
    {

        List<AlbumSlot> albumSlots = AlbumSceneManager.Instance.Slots;
        AlbumSlot selectSlot = albumSlots.Find(x => x.Id == saveId);

        if(selectSlot.transform.GetChild(0).gameObject.activeSelf)
        {
            selectedSlot = selectSlot.transform.GetChild(0).gameObject;
        }
        else
        {
            selectedSlot = selectSlot.transform.GetChild(1).gameObject;
        }

        GameObject copySlotObj = Instantiate(selectedSlot);
        GameObject albumObject = albumPool.Pop(transform.position).instance;
        Album album = albumObject.GetComponent<Album>();
        album.Init(copySlotObj, saveId);
        AlbumSceneManager.Instance.Album = album;
    }
}