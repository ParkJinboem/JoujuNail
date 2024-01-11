using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationStickerItems
{
    public Transform AnimationStickerItemParent;
    public List<StickerConverter> stickerConverters;
    public List<GuideStickerConverter> guideStickerConverters;
}