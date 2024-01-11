using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedMobilePaint : MonoBehaviour
{
    public Dictionary<int, byte[]> textureByteTest = new Dictionary<int, byte[]>();

    [Header("Inital")]
    [SerializeField, ReadOnly] private bool isInit = false;
    public bool IsInit
    {
        set { isInit = value; }
    }

    public bool drawEnabled = false;
    public bool createCanvasMesh = false;
    //브러시 사이즈
    public int brushSize;
    //브러시 알파값
    public float brushAlphaStrength = 1f;
    //페인트 컬러 색상
    public Color32 paintColor = new Color32(255, 0, 0, 255);
    public Color32 clearColor = new Color32(255, 255, 255, 255);
    //
    private bool usingClearingImage = false;
    public bool useLockArea;
    public bool useAdditiveColors;
    public bool canDrawOnBlack;
    public bool multitouchEnabled = false;
    public bool isLinePaint = false;
    public bool textureNeedsUpdate = false;
    private bool wentOutside = false;
    public bool connectBrushStokes = true;

    public LayerMask paintLayerMask;
    public DrawMode drawMode = DrawMode.Cream;
    public BrushProperties brushMode = BrushProperties.Default;
    public FilterMode filterMode = FilterMode.Point;

    [Header("UseThreshold")] //한계점
    public bool useMaskLayerOnly = false;
    public bool useThreshold = false;
    public byte paintThreshold = 128;

    [Header("Pixel")]
    [SerializeField] private Vector2 pixelUV;    //손톱이미지 크기의 위치 = 155 x 504
    public Vector2 PicelUV
    {
        get { return pixelUV; }
    }
    [SerializeField] private Vector2 pixelUVOld;
    public Vector2 PixelUVOld
    {
        get { return pixelUVOld; }
    }
    [HideInInspector]
    public byte[] lockMaskPixels;
    [HideInInspector]
    public byte[] pixels;
    [HideInInspector]
    public byte[] confirmPixels;
    [HideInInspector]
    public byte[] maskPixels;
    private byte[] clearPixels;

    [Header("Texture")]
    public Texture2D tex;
    public Texture2D confirmTex;
    public int texWidth;
    public int texHeight;
    public Sprite emptyNail;
    public string manicureName;
    public string targetTexture = "_MainTex";

    [Header("Pattern")]
    public Texture2D pattenTexture;
    public Texture2D customPattern;
    public int customPatternWidth;
    public int customPatternHeight;
    [HideInInspector]
    public byte[] patternBrushBytes;

    [Header("CusmtomBrush")]
    public Texture2D customBrush;
    public int customBrushWidth;
    public int customBrushHeight;
    public int customBrushWidthHalf;
    public Vector2 pixelUV_t;
    public bool useCustomBrushAlpha = true;
    [HideInInspector]
    public byte[] customBrushBytes;

    [Header("RayCast")]
    public Transform raySource;
    public bool useAlternativeRay = false;
    private RaycastHit hit;

    [Header("PaintSetUP")]
    public Sprite mainTexture;
    public Image maskSprite;
    Texture2D tempTex;

    [Header("StickerItem")]
    public bool movable;

    public List<Texture2D> rotateTexture2D = new List<Texture2D>();
    //회전시킨 글리터리 매니큐어값을 byte로 담은 리스트 => 0~360를 45도로 8개 담음
    public List<byte[]> patternBrushByteList = new List<byte[]>();

    [SerializeField] private Texture2D customBigBrush;
    [SerializeField] private Texture2D customSmallBrush;

    private Vector3[] texCoords = new Vector3[4];
    public GameObject nailObj;

    private bool isDrag;
    public bool IsDrag
    {
        get { return isDrag; }
    }

    private Vector3 currentPosition;

    private IQuestable questable;

    private void OnEnable()
    {
        if (isInit == false)
        {
            questable = null;
            InitializeEverything();
            InitializePaint();
            isInit = true;
        }
        ReadCurrentCustomBrush();

        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable == null)
            {
                questable = QuestSceneManager.Instance.Questables[0];
            }
        }
    }

    void FixedUpdate()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable.IsClearing() == true)
            {
                return;
            }
        }
        useAlternativeRay = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit, Mathf.Infinity, paintLayerMask);
    }

    void Update()
    {
        if (isInit == false)
        {
            return;
        }

        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable.IsClearing() == true)
            {
                return;
            }

            if (questable.IsPaint == true)
            {
                PaintingTexture();
            }
        }
        else if (Statics.gameMode == GameMode.Free)
        {
            for(int i = 0; i < PaintSceneManager.Instance.Freeables.Count; i++)
            {
                if (PaintSceneManager.Instance.Freeables[i].IsPaint == true)
                {
                    PaintingTexture();
                }
            }
        }
    }

    private void PaintingTexture()
    {
        if (!multitouchEnabled)
        {
            MousePaint();
        }
        if (drawEnabled)
        {
            UpdateTexture();
        }
    }

    public void InitializeEverything()
    {
        if (createCanvasMesh)
        {
            CreateCanvasQuad();
        }

        if (tex != null)
        {
            texWidth = tex.width;
            texHeight = tex.height;
        }
        else
        {
            texWidth = 0;
            texHeight = 0;
        }
        usingClearingImage = true;

        texWidth = GetComponent<Renderer>().material.GetTexture(targetTexture).width;
        texHeight = GetComponent<Renderer>().material.GetTexture(targetTexture).height;
        pixels = new byte[texWidth * texHeight * 4];
        tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        confirmTex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        ReadClearingImage();
        GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
        ClearImage();
        tex.filterMode = filterMode;
        tex.wrapMode = TextureWrapMode.Clamp;
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void InitializePaint()
    {
        tempTex = PaintUtils.ConvertSpriteToTexture2D(mainTexture);
        SetDrawingTexture(tempTex);
        useAdditiveColors = true;
        ReadCurrentCustomBrush();
        tempTex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        useMaskLayerOnly = true;
        SetDrawingMask(tempTex);
        CreateAreaLockMask(50, 50);
        drawEnabled = false;
    }

    public void SetVectorBrush(string manicureName, DrawMode drawMode, int brushSize, Color brushColor, Texture2D pattern, bool isAditiveBrush, bool brushCanDrawOnBlack, bool usesLockMasks, bool drawEnable)
    {
        this.manicureName = manicureName;
        string currentManicureName = null;
        if (Statics.brushSize == 30)
        {
            currentManicureName = manicureName;
        }
        else if (Statics.brushSize == 15)
        {
            currentManicureName = manicureName + "_S";
        }
        this.drawMode = drawMode;
        drawEnabled = drawEnable;
        switch (drawMode)
        {
            case DrawMode.Cream:
                brushMode = BrushProperties.Default;
                if (questable != null)
                {
                    questable.IsChecking = false;
                }
                break;
            case DrawMode.Glittery:
                if (patternBrushByteList != null)
                {
                    patternBrushByteList.Clear();
                }
                ReadGlitteryPattern(currentManicureName);
                if (questable != null)
                {
                    questable.IsChecking = true;
                }
                brushMode = BrushProperties.Glittery;
                break;
            case DrawMode.PearlRainbow:
                ReadCurrentCustomPattern(pattern);
                if (questable != null)
                {
                    questable.IsChecking = false;
                }
                brushMode = BrushProperties.Pattern;
                break;
            case DrawMode.Eraser:
                brushMode = BrushProperties.Clear;
                if (questable != null)
                {
                    questable.IsChecking = true;
                }
                break;
        }
        this.brushSize = brushSize;
        paintColor = brushColor;
        canDrawOnBlack = brushCanDrawOnBlack;
    }

    /// <summary>
    /// 스크립트 활성화시 기본 손톱의 texture를 가져와서 그려줌
    /// </summary>
    /// <param name="texture"></param>
    public void SetDrawingTexture(Texture2D texture)
    {
        texWidth = texture.width;
        texHeight = texture.height;
        tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        confirmTex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        pixels = new byte[texture.width * texture.height * 4];
        confirmPixels = new byte[texture.width * texture.height * 4];
        Color[] texturePixels = texture.GetPixels();
        int pix = 0;
        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                pixels[pix] = (byte)(texturePixels[i * texture.width + j].r * 255);//R
                pixels[pix + 1] = (byte)(texturePixels[i * texture.width + j].g * 255);//G
                pixels[pix + 2] = (byte)(texturePixels[i * texture.width + j].b * 255);//B
                pixels[pix + 3] = (byte)(texturePixels[i * texture.width + j].a * 0);//A
                pix += 4;
            }
        }
        tex.LoadRawTextureData(pixels);
        tex.Apply(false);
        confirmTex.LoadRawTextureData(pixels);
        confirmTex.Apply(false);
        GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
        if (createCanvasMesh)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<RawImage>().texture = tex;
            gameObject.GetComponent<RawImage>().enabled = true;
        }
    }

    public void SetDrawingMask(Texture2D texture)
    {
        Color[] texturePixels = texture.GetPixels();
        maskPixels = new byte[texture.width * texture.height * 4];
        int pix = 0;
        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                maskPixels[pix] = (byte)(texturePixels[i * texture.width + j].r * 255);//R
                maskPixels[pix + 1] = (byte)(texturePixels[i * texture.width + j].g * 255);//G
                maskPixels[pix + 2] = (byte)(texturePixels[i * texture.width + j].b * 255);//B
                maskPixels[pix + 3] = (byte)(texturePixels[i * texture.width + j].a * 255);//A
                pix += 4;
            }
        }
    }

    public void ClearImage()
    {
        if (usingClearingImage)
        {
            ClearImageWithImage();
        }
        else
        {
            int pixel = 0;
            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    pixels[pixel] = clearColor.r;
                    pixels[pixel + 1] = clearColor.g;
                    pixels[pixel + 2] = clearColor.b;
                    pixels[pixel + 3] = clearColor.a;
                    pixel += 4;
                }
            }
            tex.LoadRawTextureData(pixels);
            tex.Apply(true);
            confirmTex.LoadRawTextureData(pixels);
            confirmTex.Apply(true);
        }
    }

    public void ClearImageWithImage()
    {
        System.Array.Copy(clearPixels, 0, pixels, 0, clearPixels.Length);
        tex.LoadRawTextureData(clearPixels);
        tex.Apply(false);
        confirmTex.LoadRawTextureData(clearPixels);
        confirmTex.Apply(false);
    }

    void MousePaint()
    {
        if (Input.GetMouseButtonDown(0) && Input.touchCount <= 1)
        {
            //가이드핸드 애니메이션 종료
            if(Statics.gameMode == GameMode.Free)
            {
                PaintSceneManager.Instance.FreeGuideHandSetUp(4);
            }
            else if(Statics.gameMode == GameMode.Quest)
            {
                QuestSceneManager.Instance.QuestGuideHandSetUp(4);
            }

            drawEnabled = true;

            Vector3[] texVerts = new Vector3[4];
            nailObj.GetComponent<RectTransform>().GetWorldCorners(texVerts);
            for (int i = 0; i < 4; i++)
            {
                texCoords[i] = Camera.main.WorldToScreenPoint(texVerts[i]);
            }
            wentOutside = false;
        }

        if (Input.GetMouseButton(0) && Input.touchCount <= 1)
        {
            if (!useAlternativeRay)
            {
                wentOutside = true;
                //return;
            }

            if (hit.collider != GetComponentInChildren<Collider>())
            {
                wentOutside = true;
                //return;
            }

            if (Input.mousePosition != currentPosition)
            {
                isDrag = true;
            }
            else
            {
                isDrag = false;
            }

            //pixelUVOld = pixelUV;

            //pixelUV = hit.textureCoord;
            var mouseCoord = Input.mousePosition;

            var pointX = new Vector2((texCoords[3].x - texCoords[0].x) / Screen.width, (texCoords[3].y - texCoords[0].y) / Screen.height); //texCoords[3] - texCoords[0];
            var pointY = new Vector2((texCoords[1].x - texCoords[0].x) / Screen.width, (texCoords[1].y - texCoords[0].y) / Screen.height); // texCoords[1] - texCoords[0];
            var pointM = new Vector2((mouseCoord.x - texCoords[0].x) / Screen.width, (mouseCoord.y - texCoords[0].y) / Screen.height); //mouseCoord - texCoords[0];

            var x = (pointY.x * pointM.y - pointY.y * pointM.x) / (pointY.x * pointX.y - pointX.x * pointY.y);
            var y = (pointX.y * pointM.x - pointX.x * pointM.y) / (pointY.x * pointX.y - pointX.x * pointY.y);
            pixelUV = new Vector2(x, y);

            if (transform.rotation.eulerAngles.y > 170f || transform.rotation.eulerAngles.y < -170f)
            {
                pixelUV = new Vector2(1 - pixelUV.x, pixelUV.y);
            }

            pixelUV.x *= texWidth;
            pixelUV.y *= texHeight;

            //if (wentOutside)
            //{
            //    pixelUVOld = pixelUV;
            //    wentOutside = false;
            //}

            switch (drawMode)
            {
                case DrawMode.Cream:
                    if (!useAdditiveColors && (pixelUVOld == pixelUV))
                        break;
                    VectorBrushesTools.DrawCustomBrush((int)pixelUV.x, (int)pixelUV.y, this, questable);
                    textureNeedsUpdate = true;
                    break;
                case DrawMode.Glittery:
                    if (!useAdditiveColors && (pixelUVOld == pixelUV))
                        break;
                    VectorBrushesTools.DrawCustomBrush((int)pixelUV.x, (int)pixelUV.y, this, questable);
                    textureNeedsUpdate = true;
                    break;
                case DrawMode.PearlRainbow:
                    if (!useAdditiveColors && (pixelUVOld == pixelUV))
                        break;
                    VectorBrushesTools.DrawCustomBrush((int)pixelUV.x, (int)pixelUV.y, this, questable);
                    textureNeedsUpdate = true;
                    break;
                case DrawMode.Eraser:
                    if (!useAdditiveColors && (pixelUVOld == pixelUV))
                        break;
                    VectorBrushesTools.DrawCustomBrush((int)pixelUV.x, (int)pixelUV.y, this, questable);
                    textureNeedsUpdate = true;
                    break;
                default:
                    break;
            }

            currentPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0) && Input.touchCount <= 1)
        {
            if (!useAlternativeRay)
            {
                //return;
            }

            if (hit.collider != gameObject.GetComponentInChildren<Collider>())
            {
                //return;
            }

            pixelUVOld = pixelUV;
        }

        //마우스 빠르게 드래그할경우 선을 그려줌
        if (connectBrushStokes && Vector2.Distance(pixelUV, pixelUVOld) > brushSize / 2f && Input.touchCount <= 1)
        {
            switch (drawMode)
            {
                case DrawMode.Cream:
                    BitmapBrushesTools.DrawLineWithBrush(pixelUVOld, pixelUV, this);
                    break;
                case DrawMode.Glittery:
                    BitmapBrushesTools.DrawLineWithBrush(pixelUVOld, pixelUV, this);
                    break;
                case DrawMode.PearlRainbow:
                    BitmapBrushesTools.DrawLineWithBrush(pixelUVOld, pixelUV, this);
                    break;
                case DrawMode.Eraser:
                    BitmapBrushesTools.DrawLineWithBrush(pixelUVOld, pixelUV, this);
                    break;
                default:
                    break;
            }
            pixelUVOld = pixelUV;
            textureNeedsUpdate = true;
        }

        if (Input.GetMouseButtonUp(0) && Input.touchCount <= 1)
        {
            if (drawMode == DrawMode.Eraser)
            {
                return;
            }

            if (Statics.gameMode == GameMode.Free)
            {

            }
            else if (Statics.gameMode == GameMode.Quest)
            {
                questable.QuestConfirm();
            }

            drawEnabled = false;
        }
    }

    void UpdateTexture()
    {
        if (textureNeedsUpdate == true)
        {
            textureNeedsUpdate = false;
            tex.LoadRawTextureData(pixels);
            tex.Apply(false);
            if (Statics.gameMode == GameMode.Quest && questable.IsChecking == true)
            {
                confirmTex.LoadRawTextureData(confirmPixels);
                confirmTex.Apply(false);
            }
        }
    }

    public void ReadCurrentCustomBrush()
    {
        if (customBrush == null)
            return;

        if (Statics.brushSize == 30)
        {
            customBrush = customBigBrush;
        }
        else if (Statics.brushSize == 15)
        {
            customBrush = customSmallBrush;
        }
        else
        {
            customBrush = customBigBrush;
        }

        customBrushWidth = customBrush/*[selectedBrush]*/.width;
        customBrushHeight = customBrush/*[selectedBrush]*/.height;
        customBrushBytes = new byte[customBrushWidth * customBrushHeight * 4];
        Color[] tmp = customBrush.GetPixels();
        int pixel = 0;
        for (int y = 0; y < customBrushHeight; y++)
        {
            for (int x = 0; x < customBrushWidth; x++)
            {
                Color brushPixel = tmp[y * customBrushHeight + x];
                customBrushBytes[pixel] = (byte)(brushPixel.r * 255);
                customBrushBytes[pixel + 1] = (byte)(brushPixel.g * 255);
                customBrushBytes[pixel + 2] = (byte)(brushPixel.b * 255);
                customBrushBytes[pixel + 3] = (byte)(brushPixel.a * 255);

                pixel += 4;
            }
        }
        customBrushWidthHalf = (int)(customBrushWidth / 2);
    }

    public void ReadCurrentCustomPattern(Texture2D patternTexture)
    {
        if (patternTexture == null)
        {
            return;
        }

        pattenTexture = patternTexture;
        customPatternWidth = patternTexture.width;
        customPatternHeight = patternTexture.height;
        patternBrushBytes = new byte[customPatternWidth * customPatternHeight * 4];
        Color[] tmp = patternTexture.GetPixels();

        int pixel = 0;
        for (int x = 0; x < customPatternWidth; x++)
        {
            for (int y = 0; y < customPatternHeight; y++)
            {
                Color brushPixel = tmp[y * customPatternHeight + x];//patternTexture.GetPixel(x,y);
                patternBrushBytes[pixel] = (byte)(brushPixel.r * 255);
                patternBrushBytes[pixel + 1] = (byte)(brushPixel.g * 255);
                patternBrushBytes[pixel + 2] = (byte)(brushPixel.b * 255);
                patternBrushBytes[pixel + 3] = (byte)(brushPixel.a * 255);
                pixel += 4;
            }
        }
    }

    public void ReadGlitteryPattern(string currentManicureName)
    {
        textureByteTest.Clear();

        for (int i = 0; i < 8; i++)
        {
            if (textureByteTest.ContainsKey(i))
            {
                return;
            }
            Sprite sprite = DataManager.Instance.GetManicureItemSprite(currentManicureName + "_" + "Glittery" + "_" + i);
            Texture2D texture = PaintUtils.ConvertSpriteToTexture2D(sprite);
            pattenTexture = texture;
            customPatternWidth = texture.width;
            customPatternHeight = texture.height;
            Color[] testcolor = texture.GetPixels();

            byte[] pixels = new byte[customPatternWidth * customPatternHeight * 4];

            int pixel = 0;
            for (int y = 0; y < customPatternHeight; y++)
            {
                for (int x = 0; x < customPatternWidth; x++)
                {
                    Color brushPixel = testcolor[y * customPatternHeight + x];//patternTexture.GetPixel(x,y);
                    pixels[pixel] = (byte)(brushPixel.r * 255);
                    pixels[pixel + 1] = (byte)(brushPixel.g * 255);
                    pixels[pixel + 2] = (byte)(brushPixel.b * 255);
                    pixels[pixel + 3] = (byte)(brushPixel.a * 255);
                    pixel += 4;
                }
            }
            textureByteTest.Add(i, pixels);
        }
    }

    public void ReadClearingImage()
    {
        clearPixels = new byte[texWidth * texHeight * 4];

        tex.SetPixels32(((Texture2D)GetComponent<Renderer>().material.GetTexture(targetTexture)).GetPixels32());
        tex.Apply(false);

        int pixel = 0;
        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                Color c = tex.GetPixel(x, y);

                clearPixels[pixel] = (byte)(c.r * 255);
                clearPixels[pixel + 1] = (byte)(c.g * 255);
                clearPixels[pixel + 2] = (byte)(c.b * 255);
                clearPixels[pixel + 3] = (byte)(c.a * 255);
                pixel += 4;
            }
        }
    }

    public void CreateAreaLockMask(int x, int y)
    {
        if (useThreshold) //Threshold : 한계점
        {
            if (useMaskLayerOnly)
            {
                FloodFillTools.LockAreaFillWithThresholdMaskOnly(x, y, this);
            }
            else
            {
                FloodFillTools.LockMaskFillWithThreshold(x, y, this);
            }
        }
        else
        { // no threshold
            if (useMaskLayerOnly)
            {
                FloodFillTools.LockAreaFillMaskOnly(x, y, this);
            }
            else
            {
                FloodFillTools.LockAreaFill(x, y, this);
            }
        }
    }

    /// <summary>
    /// 쿼드 생성
    /// </summary>
    void CreateCanvasQuad()
    {
        Mesh go_Mesh = GetComponent<MeshFilter>().mesh;

        go_Mesh.Clear();

        Vector3[] corners = new Vector3[4];
        Vector3[] corners1 = new Vector3[4];
        Vector3 canvasScale = transform.localScale;
        Vector3 canvasRotation = transform.eulerAngles;

        Transform up = transform.parent;
        do
        {
            canvasScale.x *= up.localScale.x;
            canvasScale.y *= up.localScale.y;
            canvasScale.z *= up.localScale.z;
            up = up.parent;
        }
        while (up != null);

        Vector3 canvasPosition = transform.position;

        transform.position = Vector3.zero;

        canvasScale.x = 1f / canvasScale.x;
        canvasScale.y = 1f / canvasScale.y;
        canvasScale.z = 1f / canvasScale.z;

        canvasRotation.x = 0;
        canvasRotation.y = 0;
        canvasRotation.z = 90;

        gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            Vector3 newC = corners[i];
            newC.x *= (canvasScale.x);
            newC.y *= (canvasScale.y);
            newC.z *= (canvasScale.z);
            corners1[i] = newC;
        }

        transform.position = canvasPosition;

        go_Mesh.vertices = new[]
        {
                // bottom left
                corners1[0],
                // top left
                corners1[1],
                // top right
                corners1[2],
                // bottom right
                corners1[3]
        };

        go_Mesh.uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };

        go_Mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };

        go_Mesh.RecalculateNormals();

        go_Mesh.tangents = new[] { new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f) };

        MeshFilter meshFilter = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = go_Mesh;

        if(gameObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>() == null)
        {
            gameObject.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
        }
    }

    public bool CompareThreshold(byte a, byte b)
    {
        if (a < b)
        {
            a ^= b;
            b ^= a;
            a ^= b;
        }
        return (a - b) <= paintThreshold;
    }

    /// <summary>
    /// 브러쉬 사이즈 변경
    /// </summary>
    /// <param name="size"></param>
    public void ChangeBrushSize(int size)
    {
        if (drawMode == DrawMode.Glittery)
        {
            brushSize = size;
            Statics.brushSize = size;

            if (Statics.gameMode == GameMode.Free)
            {
                PaintSceneManager.Instance.Freeables[0].SetVectorBrush(manicureName, DrawMode.Glittery, size, Color.white, pattenTexture, false, true, true, true);
            }
            if (Statics.gameMode == GameMode.Quest)
            {
                questable.SetVectorBrush(manicureName, DrawMode.Glittery, size, Color.white, pattenTexture, false, true, true, true);
            }
            ReadCurrentCustomBrush();
        }
        else
        {
            brushSize = size;
            Statics.brushSize = brushSize;
            ReadCurrentCustomBrush();
        }
    }

    /// <summary>
    /// 지우개 셋팅
    /// </summary>
    public void SetupEraser()
    {
        useAdditiveColors = true;
        Color32 eraserColor = new Color32(255, 255, 255, 0);
        tempTex = PaintUtils.ConvertSpriteToTexture2D(mainTexture);
        SetVectorBrush("", DrawMode.Eraser, brushSize, eraserColor, tempTex, false, true, true, true);
    }

    /// <summary>
    /// 캐릭터 스티커, 스티커 이동범위 제한 
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public bool IsRaycastInsideMask(Vector3 screenPosition)
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out hit, Mathf.Infinity, paintLayerMask))
        {
            return false;
        }
        if (hit.collider != gameObject.GetComponentInChildren<Collider>())
        {
            return false;
        }

        Vector2 pixelUV1 = hit.textureCoord;
        pixelUV1.x *= texWidth;
        pixelUV1.y *= texHeight;
        int startX1 = (int)pixelUV1.x;
        int startY1 = (int)pixelUV1.y;
        int pixel1 = (texWidth * startY1 + startX1) * 4;

        if ((pixel1 < 0 || pixel1 >= pixels.Length))
        {
            return false;
        }
        else if (lockMaskPixels[pixel1] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}