using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintSceneSaveBehaviour : MonoBehaviour
{
    [SerializeField] private NailPhoto nailPhoto;

    /// <summary>
    /// 데이터 저장
    /// </summary>
    /// <param name="handBg"></param>
    /// <param name="handSpriteName"></param>
    /// <param name="freeable"></param>
    public void Save(SpriteRenderer handBg, string handSpriteName, IFreeable freeable)
    {
        AdvancedMobilePaint[] paintEngines = freeable.PaintEngines;
        List<PatternItems> patternItems = freeable.PatternItems;
        List<CharacterStickerItems> characterStickerItems = freeable.CharacterStickerItems;
        List<StickerItems> stickerItems = freeable.StickerItems;
        List<AnimationStickerItems> animationStickerItems = freeable.AnimationStickerItems;
        bool soundCheck = PlayerDataManager.Instance.GetOptionData().isSound;

        Hand hand = new Hand();

        if (Statics.selectType == SelectType.Hand)
        {
            hand.type = 1;
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            hand.type = 2;
        }

        int id = 0;
        while (true)
        {
            bool isHave = PlayerDataManager.Instance.IsHaveHand(id);
            //해당 아이디를 가지고 있으면
            if (isHave)
            {
                id += 1;
            }
            //해당 아이디를 가지고 있지 않을때
            else
            {
                //hand.level = id;
                hand.saveId = id;
                break;
            }
        }

        string[] spriteSplitStrings = handSpriteName.Split('(');
        hand.handSpriteString = spriteSplitStrings[0];
        string[] bgSpriteSplitStrings = handBg.sprite.name.Split('(');
        hand.bgSpriteString = bgSpriteSplitStrings[0];

        List<Finger> fingers = new List<Finger>();
        for (int i = 0; i < 5; i++)
        {
            Finger finger = new Finger();

            finger = SaveFinger(paintEngines[i]);
            finger.patternData = SavePattern(patternItems[i], i);
            finger.stickerDatas = SaveStickers(characterStickerItems[i], stickerItems[i], animationStickerItems[i]);
            fingers.Add(finger);
        }
        hand.fingerdatas = fingers;
        nailPhoto.TImeLineStart(hand, null, freeable, soundCheck);
    }

    /// <summary>
    /// 손가락 정보저장
    /// </summary>
    /// <param name="paintEngine"></param>
    /// <returns></returns>
    private Finger SaveFinger(AdvancedMobilePaint paintEngine)
    {
        Finger finger = new Finger();
        //더이상 byte를 저장하지 않음_231226 박진범
        //finger.nailTexturePixels = GetBytes(paintEngine);
        finger.nailTextureString = PaintUtils.StringFromTexture(paintEngine.tex);
        string[] spriteName = paintEngine.maskSprite.sprite.name.Split('(');
        finger.nailMaskString = spriteName[0];
        return finger;
    }

    /// <summary>
    /// 패턴 정보저장
    /// </summary>
    /// <param name="patternItem"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private Pattern SavePattern(PatternItems patternItem, int index)
    {
        Pattern pattern = new Pattern();
        //패턴아이템이 null이 아닐떄
        if (patternItem.patternConverter != null)
        {
            if (patternItem.patternConverter.gameObject.activeInHierarchy)
            {
                pattern.id = index + 1;
                pattern.posX = patternItem.patternConverter.transform.localPosition.x;
                pattern.posY = patternItem.patternConverter.transform.localPosition.y;
                pattern.posZ = patternItem.patternConverter.transform.localPosition.z;

                RectTransform rectTransform = patternItem.patternConverter.GetComponent<RectTransform>();
                pattern.scaleX = rectTransform.localScale.x;
                pattern.scaleY = rectTransform.localScale.y;
                pattern.scaleZ = rectTransform.localScale.z;

                Image image = patternItem.patternConverter.Image;
                if (image != null)
                {
                    string[] itemSpriteSplitString = image.sprite.name.Split('(');
                    pattern.itemSpriteString = itemSpriteSplitString[0];
                    pattern.patternColorString = ColorUtility.ToHtmlStringRGBA(image.color);
                }
            }
        }
        return pattern;
    }

    /// <summary>
    /// 스티커 정보저장
    /// </summary>
    /// <param name="characterStickerItem"></param>
    /// <param name="stickerItem"></param>
    /// <param name="animationStickerItem"></param>
    /// <returns></returns>
    private List<Sticker> SaveStickers(CharacterStickerItems characterStickerItem, StickerItems stickerItem, AnimationStickerItems animationStickerItem)
    {
        List<Sticker> stickers = new List<Sticker>();

        //캐릭터 스티커 저장
        for (int i = 0; i < characterStickerItem.stickerConverters.Count; i++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = characterStickerItem.stickerConverters[i].gameObject.GetComponent<RectTransform>();
            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            StickerConverter stickerConverter = characterStickerItem.stickerConverters[i];
            Image image = stickerConverter.StickerImage;
            if (image != null)
            {
                string[] characterStickerItemNames = image.sprite.name.Split('(');
                sticker.itemSpriteString = characterStickerItemNames[0];
                stickers.Add(sticker);
            }
        }
        //일반스티커 저장
        for (int i = 0; i < stickerItem.stickerConverters.Count; i++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = stickerItem.stickerConverters[i].gameObject.GetComponent<RectTransform>();
            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            StickerConverter stickerConverter = stickerItem.stickerConverters[i];
            Image image = stickerConverter.StickerImage;
            if (image != null)
            {
                string[] stickerItemNames = image.sprite.name.Split('(');
                sticker.itemSpriteString = stickerItemNames[0];
                stickers.Add(sticker);
            }
        }
        //애니메이션 스티커 저장
        for (int i = 0; i < animationStickerItem.stickerConverters.Count; i++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = animationStickerItem.stickerConverters[i].gameObject.GetComponent<RectTransform>();
            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            StickerConverter stickerConverter = animationStickerItem.stickerConverters[i];
            Image image = stickerConverter.AnimationStickerObject.GetComponent<Image>();
            if (image != null)
            {
                string[] animationStickerItemNames = image.sprite.name.Split('_');
                sticker.itemSpriteString = animationStickerItemNames[0] + "_" + animationStickerItemNames[1];
                stickers.Add(sticker);
            }
        }
        return stickers;
    }

    /// <summary>
    /// 페인트엔진의 텍스쳐를 픽셀화
    /// </summary>
    /// <param name="paintEngine"></param>
    /// <returns></returns>
    private byte[] GetBytes(AdvancedMobilePaint paintEngine)
    {
        byte[] pixels = paintEngine.pixels;
        return pixels;
    }
}