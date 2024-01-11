using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStickerItems
{
    public Transform characterStickerItemParent;
    public List<StickerConverter> stickerConverters;
    public List<GuideStickerConverter> guideStickerConverters;
}