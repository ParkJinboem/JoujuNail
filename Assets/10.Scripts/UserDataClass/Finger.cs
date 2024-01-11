using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Finger
{
    public byte[] nailTexturePixels;
    public string nailTextureString;
    public string nailMaskString;
    //테스트_박진범
    //public Texture nailTexture;
    //public RawImage nailRawImage;
    public Pattern patternData;
    public List<Sticker> stickerDatas;
}