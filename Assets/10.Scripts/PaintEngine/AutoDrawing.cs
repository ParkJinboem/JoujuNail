using System.Collections.Generic;
using UnityEngine;

public class AutoDrawing : MonoBehaviour
{
    /// <summary>
    /// 단색 매니큐어 자동완성
    /// </summary>
    /// <param name="color"></param>
    /// <param name="selectType"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Texture2D SetDrawingTexture(Color color, SelectType selectType, Vector2 scale)
    {
        Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false); ;
        tex = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RGBA32, false);
        byte[] pixels = new byte[tex.width * tex.height * 4];
        int totalLength = pixels.Length;

        int pix = 0;
        for (int i = 0; i < totalLength; i++)                                                                                                                                                                                                                                                                                                                         
        {
            if (pix >= pixels.Length)
            {
                pix = pixels.Length - 4;
            }
            pixels[pix] = (byte)(color.r * 255);//R
            if (pix >= pixels.Length)
            {
                pix = pixels.Length - 3;
            }
            pixels[pix + 1] = (byte)(color.g * 255);//G
            if (pix >= pixels.Length)
            {
                pix = pixels.Length - 2;
            }
            pixels[pix + 2] = (byte)(color.b * 255);//B
            if (pix >= pixels.Length)
            {
                pix = pixels.Length - 1;
            }
            pixels[pix + 3] = (byte)(color.a * 255);//A
            pix += 4;
        }
        tex.LoadRawTextureData(pixels);
        tex.Apply(false);
        return tex;
    }

    /// <summary>
    /// 패턴매니큐어 자동완성
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="selectType"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Texture2D SetDrawingPatternTexture(Texture2D texture, SelectType selectType, Vector2 scale)
    {
        Vector2 PicelUV = new Vector2();
        Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
        PicelUV = new Vector2((int)scale.x / 2, (int)scale.y / 2);
        tex = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RGBA32, false);

        int pixel = 0;
        float yy = 0;
        float xx = 0;
        int pixel2 = 0;
        byte[] pixels = new byte[tex.width * tex.height * 4];
        byte[] patternBrushBytes = new byte[texture.width * texture.height * 4];
        Color[] tmp = texture.GetPixels();

        int patternPixel = 0;
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color patternbrushPixel = tmp[y * texture.height + x];
                patternBrushBytes[patternPixel] = (byte)(patternbrushPixel.r * 255);
                patternBrushBytes[patternPixel + 1] = (byte)(patternbrushPixel.g * 255);
                patternBrushBytes[patternPixel + 2] = (byte)(patternbrushPixel.b * 255);
                patternBrushBytes[patternPixel + 3] = (byte)(patternbrushPixel.a * 255);
                patternPixel += 4;
            }
        }

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                /*float*/
                yy = Mathf.Repeat(PicelUV.y + (y - tex.height / 2), texture.height);
                /*float*/
                xx = Mathf.Repeat(PicelUV.x + (x - tex.width / 2), texture.width);
                /*int*/
                pixel2 = (int)Mathf.Repeat((texture.width * xx + yy) * 4, patternBrushBytes.Length);
                pixels[pixel] = (byte)Mathf.Lerp(pixels[pixel], patternBrushBytes[pixel2], 255/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                pixels[pixel + 1] = (byte)Mathf.Lerp(pixels[pixel + 1], patternBrushBytes[pixel2 + 1], 255/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                pixels[pixel + 2] = (byte)Mathf.Lerp(pixels[pixel + 2], patternBrushBytes[pixel2 + 2], 255/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                pixels[pixel + 3] = (byte)Mathf.Lerp(pixels[pixel + 3], patternBrushBytes[pixel2 + 3], 255/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                pixel += 4;
            }
        }

        tex.LoadRawTextureData(pixels);
        tex.Apply(false);

        return tex;
    }

    /// <summary>
    /// 글리터리 자동완성
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="selectType"></param>
    /// <param name="scale"></param>
    /// <param name="drawCount"></param>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static Texture2D SetDrawingGlitteryTexture(string sprite, SelectType selectType, Vector2 scale, int drawCount, float ratio)
    {
        List<Texture2D> textures = new List<Texture2D>();
        List<byte[]> texturesByte = new List<byte[]>();
        for (int i = 0; i < 8; i++)
        {
            string[] spriteName = sprite.Split("(");
            Sprite rotatesprite = DataManager.Instance.GetManicureItemSprite(spriteName[0] + "_Glittery_" + i);
            Texture2D rotatepattenTexture = PaintUtils.ConvertSpriteToTexture2D(rotatesprite);
            int texWidth = rotatepattenTexture.width;
            int texHeight = rotatepattenTexture.height;
            byte[] patternBrushBytes = new byte[texWidth * texHeight * 4];

            Color[] tmp = rotatepattenTexture.GetPixels();

            int pixel = 0;
            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    Color brushPixels = tmp[y * texHeight + x];
                    patternBrushBytes[pixel] = (byte)(brushPixels.r * 255);
                    patternBrushBytes[pixel + 1] = (byte)(brushPixels.g * 255);
                    patternBrushBytes[pixel + 2] = (byte)(brushPixels.b * 255);
                    patternBrushBytes[pixel + 3] = (byte)(brushPixels.a * 255);
                    pixel += 4;
                }
            }
            textures.Add(rotatepattenTexture);
            texturesByte.Add(patternBrushBytes);
        }

        Texture2D tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
        tex = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RGBA32, false);

        byte[] pixels = new byte[tex.width * tex.height * 4];
        int brushPixel = 0;
        float lerpVal = 1f;
        int count = Random.Range(0, textures.Count);

        for (int y = 0; y < drawCount; y++)
        {
            for (int x = 0; x < 11; x++) //11은 적당한 값을 찾다보니 나온 수_윤정덕
            {
                float pixelUVx = tex.width  * 0.1f * x;
                float pixelUVy = tex.height * ratio * y; //ratio = 1 / (drawcount - 1)_윤정덕  
                int startX = ((int)pixelUVx - textures[count].width / 2);
                int startY = ((int)pixelUVy - textures[count].height / 2);
                int pixel = (tex.width * startY + startX) * 4;
                if (pixel < 0)
                {
                    pixel = -1 * pixel;
                }
                for (int h = 0; h < textures[count].height; h++)
                {
                    for (int w = 0; w < textures[count].width; w++)
                    {
                        brushPixel = (textures[count].width * (h) + w) * 4;
                        lerpVal = texturesByte[count][brushPixel + 3] / 255f;
                        pixels[pixel] = (byte)Mathf.Lerp(pixels[pixel], texturesByte[count][brushPixel], lerpVal);
                        pixels[pixel + 1] = (byte)Mathf.Lerp(pixels[pixel + 1], texturesByte[count][brushPixel + 1], lerpVal);
                        pixels[pixel + 2] = (byte)Mathf.Lerp(pixels[pixel + 2], texturesByte[count][brushPixel + 2], lerpVal);
                        pixels[pixel + 3] = (byte)Mathf.Lerp(pixels[pixel + 3], texturesByte[count][brushPixel + 3], lerpVal);
                        pixel += 4;
                    }
                }
                count = Random.Range(0, textures.Count);
            }
        }
        tex.LoadRawTextureData(pixels);
        tex.Apply(false);
        return tex;
    }
}