using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSceneSaveBehaviour : MonoBehaviour
{
    [SerializeField] private NailPhoto nailPhoto;

    private byte[] pixels;

    /// <summary>
    /// 데이터 저장
    /// </summary>
    /// <param name="questHandData"></param>
    /// <param name="handSpriteName"></param>
    /// <param name="questable"></param>
    public void Save(QuestHandData questHandData, string handSpriteName, IQuestable questable)
    {
        AdvancedMobilePaint[] paintEngines = questable.PaintEngines;
        List<PatternItems> patternItems = questable.PatternItems;
        List<CharacterStickerItems> characterStickerItems = questable.CharacterStickerItems;
        List<StickerItems> stickerItems = questable.StickerItems;
        List<AnimationStickerItems> animationStickerItems = questable.AnimationStickerItems;
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
                hand.saveId = id;
                break;
            }
        }

        string[] spriteSplitStrings = handSpriteName.Split('('); 
        hand.handSpriteString = spriteSplitStrings[0];
        hand.bgSpriteString = questHandData.bgSpriteString;

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

        //레벨 +1
        PlayerDataManager.Instance.CompleteMission(questHandData.level);
        questable.IsClear = true;
        nailPhoto.TImeLineStart(hand, questable, null, soundCheck);
    }

    /// <summary>
    /// 손가락 저장
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
    /// 패턴 저장
    /// </summary>
    /// <param name="patternItem"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private Pattern SavePattern(PatternItems patternItem, int index)
    {
        Pattern pattern = new Pattern();
        //패턴컨버터가 null이 아닐경우
        if (patternItem.patternConverter != null)
        {
            pattern.id = index + 1;
            Transform tr = patternItem.patternConverter.gameObject.GetComponent<Transform>();
            pattern.posX = tr.localPosition.x;
            pattern.posY = tr.localPosition.y;
            pattern.posZ = tr.localPosition.z;

            RectTransform rectTransform = patternItem.patternConverter.gameObject.GetComponent<RectTransform>();
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
        return pattern;
    }

    /// <summary>
    /// 스티커 저장
    /// </summary>
    /// <param name="characterStickerItem"></param>
    /// <param name="stickerItem"></param>
    /// <param name="animationStickerItem"></param>
    /// <returns></returns>
    private List<Sticker> SaveStickers(CharacterStickerItems characterStickerItem, StickerItems stickerItem, AnimationStickerItems animationStickerItem)
    {
        List<Sticker> stickers = new List<Sticker>();

        //캐릭터 스티커 저장
        for (int j = 0; j < characterStickerItem.guideStickerConverters.Count; j++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = characterStickerItem.guideStickerConverters[j].RectTransform;

            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            GuideStickerConverter guideStickerConverter = characterStickerItem.guideStickerConverters[j];
            Image image = guideStickerConverter.Image;
            if (image != null)
            {
                string[] characterStickerItemNames = image.sprite.name.Split('(');
                sticker.itemSpriteString = characterStickerItemNames[0];
                stickers.Add(sticker);
            }
        }
        //일반스티커 저장
        for (int j = 0; j < stickerItem.guideStickerConverters.Count; j++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = stickerItem.guideStickerConverters[j].RectTransform;

            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            GuideStickerConverter guideStickerConverter = stickerItem.guideStickerConverters[j];
            Image image = guideStickerConverter.Image;
            if (image != null)
            {
                string[] stickerItemNames = image.sprite.name.Split('(');
                sticker.itemSpriteString = stickerItemNames[0];
                stickers.Add(sticker);
            }
        }
        //애니메이션스티커 저장
        for (int j = 0; j < animationStickerItem.guideStickerConverters.Count; j++)
        {
            Sticker sticker = new Sticker();

            RectTransform rectTransform = animationStickerItem.guideStickerConverters[j].RectTransform;

            sticker.posX = rectTransform.localPosition.x;
            sticker.posY = rectTransform.localPosition.y;
            sticker.posZ = rectTransform.localPosition.z;
            sticker.rotX = rectTransform.localRotation.eulerAngles.x;
            sticker.rotY = rectTransform.localRotation.eulerAngles.y;
            sticker.rotZ = rectTransform.localRotation.eulerAngles.z;
            sticker.width = rectTransform.sizeDelta.x;
            sticker.height = rectTransform.sizeDelta.y;

            GuideStickerConverter guideStickerConverter = animationStickerItem.guideStickerConverters[j];
            Image image = guideStickerConverter.Image;
            if (image != null)
            {
                string[] animationStickerItemNames = image.sprite.name.Split('(');
                sticker.itemSpriteString = animationStickerItemNames[0];
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
        Color[] colors = paintEngine.tex.GetPixels();

        pixels = new byte[paintEngine.tex.height * paintEngine.tex.width * 4];
        int pix = 0;
        for (int i = 0; i < paintEngine.tex.height; i++)
        {
            for (int j = 0; j < paintEngine.tex.width; j++)
            {
                pixels[pix] = (byte)(colors[i * paintEngine.tex.width + j].r * 255);//R
                pixels[pix + 1] = (byte)(colors[i * paintEngine.tex.width + j].g * 255);//G
                pixels[pix + 2] = (byte)(colors[i * paintEngine.tex.width + j].b * 255);//B
                pixels[pix + 3] = (byte)(colors[i * paintEngine.tex.width + j].a * 255);//A
                pix += 4;
            }
        }
        return pixels;
    }
}