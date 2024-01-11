using UnityEngine;
using UnityEngine.UI;

public class PaintBrushCreater : MonoBehaviour
{
    [SerializeField] private RawImage eraserRawImage;
    [SerializeField] private RawImage brushRawImage;

    private Texture2D bigBrush;
    private Texture2D smallBrush;

    private RawImage brushImage;
    public RawImage BrushImage
    {
        get { return brushImage; }
    }

    public DrawMode drawMode = DrawMode.Cream;
    public LayerMask paintLayerMask;
    private RaycastHit2D hit;

    private bool isInit;
    public bool IsInit
    {
        set { isInit = value; }
    }

    private IQuestable questable;
    public IQuestable Questable
    {
        set { questable = value; }
    }
    private IFreeable freeable;
    public IFreeable Freeable
    {
        set { freeable = value; }
    }

    public Vector2 brushOffset;

    private void Update()
    {
        if (isInit == false)
        {
            if (brushImage != null)
            {
                Destroy(brushImage.gameObject);
            }
            return;
        }

        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable.IsClearing() == true)
            {
                return;
            }

            questable.SetDrawMode();
        }
        else if (Statics.gameMode == GameMode.Free)
        {
            freeable.SetDrawMode();
        }

        if (Input.GetMouseButtonDown(0) && Input.touchCount <= 1)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, paintLayerMask);
            if (Statics.gameMode == GameMode.Quest)
            {
                if (hit == true)
                {
                    SetBrushImage(drawMode, false);
                    questable.IsPaint = false;
                }
                else
                {
                    SetBrushImage(drawMode, true);
                    DrawSoundPlay(drawMode, true);
                    questable.IsPaint = true;
                }
            }
            else if (Statics.gameMode == GameMode.Free)
            {
                if (hit == true)
                {
                    SetBrushImage(drawMode, false);
                    freeable.IsPaint = false;
                }
                else
                {
                    SetBrushImage(drawMode, true);
                    DrawSoundPlay(drawMode, true);
                    freeable.IsPaint = true;
                }
            }
        }

        if (Input.GetMouseButton(0) && Input.touchCount <= 1)
        {
            if (brushImage != null)
            {
                Vector2 offset = new Vector2();
                if (drawMode == DrawMode.Eraser)
                {
                    offset = Vector2.zero;
                }
                else
                {
                    offset = brushOffset;
                }
                brushImage.gameObject.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y));
            }
        }

        if (Input.GetMouseButtonUp(0) && Input.touchCount <= 1)
        {
            DrawSoundPlay(drawMode, false);
            SetBrushImage(drawMode, false);
        }
    }

    /// <summary>
    /// 드로우 모드 설정
    /// </summary>
    /// <param name="drawMode"></param>
    public void SetDrawMode(DrawMode drawMode)
    {
        this.drawMode = drawMode;
    }

    /// <summary>
    /// 브러쉬 이미지 크게이 따른 이미지 설정
    /// </summary>
    /// <param name="drawMode"></param>
    /// <param name="isDraw"></param>
    public void SetBrushImage(DrawMode drawMode, bool isDraw)
    {
        if (isDraw == true)
        {
            if (brushImage == null)
            {
                if (drawMode != DrawMode.Eraser)
                {
                    if (Statics.gameMode == GameMode.Quest)
                    {
                        RawImage rawImage = Instantiate(brushRawImage, QuestSceneManager.Instance.transform);
                        brushImage = rawImage;
                    }
                    else if (Statics.gameMode == GameMode.Free)
                    {
                        RawImage rawImage = Instantiate(brushRawImage, PaintSceneManager.Instance.transform);
                        brushImage = rawImage;
                    }
                }
                else
                {
                    if (Statics.gameMode == GameMode.Quest)
                    {
                        RawImage rawImage = Instantiate(eraserRawImage, QuestSceneManager.Instance.transform);
                        brushImage = rawImage;
                    }
                    else if (Statics.gameMode == GameMode.Free)
                    {
                        RawImage rawImage = Instantiate(eraserRawImage, PaintSceneManager.Instance.transform);
                        brushImage = rawImage;
                    }
                }
                
            }

            if (drawMode != DrawMode.Eraser)
            {
                string manicureId = PlayerDataManager.Instance.GetInfo().curSelectedManicureId.ToString("D3");
                if (Statics.brushSize == 30)
                {
                    if (Statics.gameMode == GameMode.Quest)
                    {
                        //string[] manicureNames = questable.SelectPaintEngine.manicureName.Split("_");
                        //Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("BigBrush_" + manicureNames[1]);
                        Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("BigBrush_" + manicureId);
                        bigBrush = texture;
                    }
                    else if (Statics.gameMode == GameMode.Free)
                    {
                        //string[] manicureNames = freeable.SelectPaintEngine.manicureName.Split("_");
                        //Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("BigBrush_" + manicureNames[1]);
                        Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("BigBrush_" + manicureId);
                        bigBrush = texture;
                    }
                    brushImage.texture = bigBrush;
                }
                else if (Statics.brushSize == 15)
                {
                    if (Statics.gameMode == GameMode.Quest)
                    {
                        //string[] manicureNames = questable.SelectPaintEngine.manicureName.Split("_");
                        //Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("SmallBrush_" + manicureNames[1]);
                        Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("SmallBrush_" + manicureId);
                        smallBrush = texture;
                    }
                    else if (Statics.gameMode == GameMode.Free)
                    {
                        //string[] manicureNames = freeable.SelectPaintEngine.manicureName.Split("_");
                        //Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("SmallBrush_" + manicureNames[1]);
                        Texture2D texture = AddressableManager.Instance.GetAssetByKey<Texture2D>("SmallBrush_" + manicureId);
                        smallBrush = texture;
                    }
                    brushImage.texture = smallBrush;
                }
            }
        }
        else if (isDraw == false)
        {
            if (brushImage != null)
            {
                Destroy(brushImage.gameObject);
            }
        }
    }

    private void DrawSoundPlay(DrawMode drawMode,bool play)
    {
        if (play)
        {
            if (drawMode == DrawMode.Cream)
            {
                brushRawImage.transform.GetChild(0).gameObject.SetActive(false);
                SoundManager.Instance.PlayDrawSound("DrawManicure");
            }
            else if (drawMode == DrawMode.Eraser)
            {
                SoundManager.Instance.PlayDrawSound("DrawEraser");
            }
            else
            {
                SoundManager.Instance.PlayDrawSound("DrawPealManicure");
            }
        }
        else if (!play)
        {   
            SoundManager.Instance.StopDrawSound();
        }
    }
}