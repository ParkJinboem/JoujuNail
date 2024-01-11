using System;
using System.Collections.Generic;

[Serializable]
public class Hand
{
    public int type;
    //public int level;
    public int saveId;
    public string bgSpriteString;
    public string handSpriteString;
    public List<Finger> fingerdatas;
}