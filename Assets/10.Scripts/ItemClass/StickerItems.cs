using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StickerItems
{
    public Transform stickerItemParent;
    public List<StickerConverter> stickerConverters;
    public List<GuideStickerConverter> guideStickerConverters;
}