using UnityEngine;

public class Statics
{
    // const
    public const int handfootSpriteCount = 16;
    public const int handType = 1;
    public const int footType = 2;
    public const int fingerType = 3;
    public const int toesType = 4;
    public const int fingerNailType = 5;
    public const int toesNailType = 6;
    public const int fingerNailCoverType = 7;
    public const int toesNailCoverType = 8;
    public const int bgType = 11;

    // static
    public static int level = 0;
    public static int maxLevel = 140;
    public static GameMode gameMode;
    public static SelectType selectType;
    public static int selectManicureId = 2;
    public static int brushSize = 30;
    public static Vector2[] fingerScales = { new Vector2(173, 316), new Vector2(155, 297), new Vector2(166, 315), new Vector2(155, 292), new Vector2(136, 257) };//손톱 텍스쳐 실제크기
    public static Vector2[] toesScales = { new Vector2(379, 423), new Vector2(216, 244), new Vector2(177, 200), new Vector2(151, 171), new Vector2(134, 151) };//발톱 텍스쳐 실제크기
    public static int[] fingerCreamCutline = { 33700, 27700, 31500, 27200, 21400};//손톱 일반 매니큐어 커트라인
    public static int[] fingerGlitteryCutline = { 16690, 14500, 16500, 15120, 10660 };//손톱 글리터리 커트라인
    public static int[] fingerPearlRainbowCutline = { 150, 2450, 2900 , 2490 , 1915};//손톱 펄레인보우 커트라인
    public static int[] toesCreamCutline = { 87860, 33900, 22800, 16700, 13100 };//발톱 일반 매니큐어 커트라인 
    public static int[] toesGlitteryCutline = { 46495, 14404, 10409, 6800, 6610 };//발톱 글리터리 커트라인
    public static int[] toesPearlRainbowCutline = { 657, 130, 90, 180, 97 };//발톱 펄레인보우 커트라인
    public static int[] fingerAutoDrawingCount = { 26, 26, 26, 26, 26 };
    public static float[] fingerAutoDrawingRatio = { 0.04f, 0.04f, 0.04f, 0.04f, 0.04f };
    public static int[] toesAutoDrawingCount = { 41, 26, 11, 11, 11 };
    public static float[] toesAutoDrawingRatio = { 0.025f, 0.04f, 0.1f, 0.1f, 0.1f };
    public static Vector2 manicureSize = new Vector2(180, 260);// 매니큐어 아이템리스트상 크기
    public static Vector2 patternSize = new Vector2(228, 308);//패턴 아이템리스트상 크기
    public static Vector2 stickerSize = new Vector2(256, 256);//스티커 아이템리스트상 크기
}
