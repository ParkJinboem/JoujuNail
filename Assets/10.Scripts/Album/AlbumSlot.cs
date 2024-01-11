using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumSlot : MonoBehaviour
{
    [SerializeField] private AlbumSlotItem albumSlotItem;

    [Header("NailData")]
    [SerializeField] private Image handSprite;
    [SerializeField] private Image footSprite;
    [SerializeField] private Image[] fingerNailMasks;
    [SerializeField] private Image[] toesNailMasks;
    [SerializeField] private RawImage[] fingerNailTextures;
    [SerializeField] private RawImage[] toesNailTextures;
    [SerializeField] private GameObject[] fingerPatternItems;
    [SerializeField] private GameObject[] toesPatternItems;
    [SerializeField] private GameObject stickerItemPrefabs;
    [SerializeField] private Image[] fingerNailCovers;
    [SerializeField] private Image[] toesNailCoves;
    private List<GameObject> stickerItems;
    private GameObject stickerItemObject;

    private Hand hand;
    public Hand Hand
    {
        get { return hand; }
    }

    [Header("Album")]
    [SerializeField] private GameObject AlbumPrefabs;
    [SerializeField] private int id;
    public int Id
    {
        get { return id; }
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    /// <param name="hand"></param>
    public void Init(Hand hand)
    {
        handSprite.gameObject.SetActive(true);
        footSprite.gameObject.SetActive(true);

        stickerItems = new List<GameObject>();
        this.hand = hand;
        id = hand.saveId;
        //id = hand.level;

        //타입이 손일경우
        if (hand.type == 1)
        {
            handSprite.sprite = DataManager.Instance.GetHandSprite(hand.handSpriteString);
            footSprite.gameObject.SetActive(false);

            for (int i = 0; i < hand.fingerdatas.Count; i++)
            {
                Finger finger = hand.fingerdatas[i];
                Pattern pattern = finger.patternData;
                List<Sticker> stickers = finger.stickerDatas;

                SetFingerToes(finger, fingerNailMasks[i], fingerNailCovers[i], fingerNailTextures[i], Statics.fingerScales[i]);
                SetPattern(pattern, fingerPatternItems[i]);
                SetSticker(stickers, fingerNailMasks[i].gameObject.transform);
            }
        }
        //타입이 발일경우
        else if (hand.type == 2)
        {
            handSprite.gameObject.SetActive(false);
            footSprite.sprite = DataManager.Instance.GetFootSprite(hand.handSpriteString);

            for (int i = 0; i < hand.fingerdatas.Count; i++)
            {
                Finger finger = hand.fingerdatas[i];
                Pattern pattern = finger.patternData;
                List<Sticker> stickers = finger.stickerDatas;

                SetFingerToes(finger, toesNailMasks[i], toesNailCoves[i], toesNailTextures[i], Statics.toesScales[i]);
                SetPattern(pattern, toesPatternItems[i]);
                SetSticker(stickers, toesNailMasks[i].gameObject.transform);
            }
        }
    }

    /// <summary>
    /// 손, 발톱 및 매니큐어 셋팅
    /// </summary>
    /// <param name="finger"></param>
    /// <param name="mask"></param>
    /// <param name="cover"></param>
    /// <param name="nailTexture"></param>
    /// <param name="scale"></param>
    private void SetFingerToes(Finger finger, Image mask, Image cover, RawImage nailTexture, Vector2 scale)
    {
        Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
        if (hand.type == 1)
        {
            mask.sprite = DataManager.Instance.GetFingerNailMaskSprite(finger.nailMaskString);
            string[] coverNames = finger.nailMaskString.Split("FingerNailMask");
            cover.sprite = DataManager.Instance.GetFingerNailCoverSprite("FingerNailCover" + coverNames[1]);
        }
        else
        {
            mask.sprite = DataManager.Instance.GetToesNailMaskSprite(finger.nailMaskString);
            string[] coverNames = finger.nailMaskString.Split("ToesNailMask");
            cover.sprite = DataManager.Instance.GetToesNailCoverSprite("ToesNailCover" + coverNames[1]);
        }

        //저장 방식 변경 -> byte를 읽는 형식에서 string을 읽는 방식으로 변경(더이상byte를 저장하지 않음
        //_PaintSceneSaveBehaviour스크립트의 81줄 참고)_231226 박진범
        nailTexture.texture = PaintUtils.TextureFromString(finger.nailTextureString);
        //if (finger.nailTexturePixels == null)
        //{
        //    texture2D = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RGBA32, false);
        //    texture2D.Apply();
        //    nailTexture.texture = texture2D;
        //}
        //else
        //{
        //    texture2D = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RGBA32, false);
        //    texture2D.LoadRawTextureData(finger.nailTexturePixels);
        //    texture2D.Apply();

        //    nailTexture.texture = texture2D;
        //}

    }

    /// <summary>
    /// 패턴 셋팅
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="patternItem"></param>
    private void SetPattern(Pattern pattern, GameObject patternItem)
    {
        if (pattern.itemSpriteString != null)
        {
            patternItem.gameObject.SetActive(true);
            Image image = patternItem.GetComponent<Image>();
            image.sprite = DataManager.Instance.GetManicureItemSprite(pattern.itemSpriteString);
            Color color = image.color;
            ColorUtility.TryParseHtmlString("#" + pattern.patternColorString, out color);
            image.color = color;
            //Vector3 pos = new Vector3(pattern.posX, pattern.posY, pattern.posZ);
            Vector3 pos = Vector3.zero;
            patternItem.transform.localPosition = pos;
            RectTransform rectTransform = patternItem.GetComponent<RectTransform>();
            Vector3 scale = new Vector3(pattern.scaleX, pattern.scaleY, pattern.scaleZ);
            rectTransform.localScale = scale;
        }
        else
        {
            patternItem.SetActive(false);
        }
    }

    /// <summary>
    /// 스티커 셋팅
    /// </summary>
    /// <param name="stickers"></param>
    /// <param name="parent"></param>
    
    private void SetSticker(List<Sticker> stickers, Transform parent)
    {
        for (int j = 0; j < stickers.Count; j++)
        {
            string[] stickerNames = stickers[j].itemSpriteString.Split('_');
            string stickerName = stickerNames[0] + "_" +stickerNames[1];


            if(stickerNames[0] == "AnimationSticker")
            {
                ManicureData manicureData = DataManager.Instance.GetManicureDataByName(stickerName);
                GameObject stickerItemObjectPrefabs = AddressableManager.Instance.GetAssetByKey<GameObject>(manicureData.option);
                stickerItemObject = Instantiate(stickerItemObjectPrefabs);
            }
            else
            {
                stickerItemObject = Instantiate(stickerItemPrefabs);
            }



            stickerItemObject.transform.SetParent(parent.gameObject.transform);
            Image image = stickerItemObject.GetComponent<Image>();
            //image.sprite = DataManager.Instance.GetManicureItemSprite(stickers[j].itemSpriteString);
            image.sprite = DataManager.Instance.GetManicureItemSprite(stickerName);
            RectTransform rectTransform = stickerItemObject.GetComponent<RectTransform>();
            Vector3 pos = new Vector3(stickers[j].posX, stickers[j].posY, stickers[j].posZ);
            rectTransform.localPosition = pos;
            Quaternion rot = Quaternion.Euler(stickers[j].rotX, stickers[j].rotY, stickers[j].rotZ);
            rectTransform.localRotation = rot;
            Vector2 size = new Vector2(stickers[j].width, stickers[j].height);
            rectTransform.sizeDelta = size;
            rectTransform.localScale = Vector3.one;

            stickerItems.Add(stickerItemObject);
        }
    }

    /// <summary>
    /// 앨범 선택
    /// </summary>
    public void SelectAlbum()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        AlbumSceneManager.Instance.MoveToAlbum(id);
    }

    public void Hide()
    {
        for(int i = 0; i < stickerItems.Count; i++)
        {
            Destroy(stickerItems[i]);
        }
        albumSlotItem.Hide();
    }
}