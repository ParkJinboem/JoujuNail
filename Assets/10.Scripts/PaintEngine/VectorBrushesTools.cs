using UnityEngine;

public class VectorBrushesTools
{
    #region VectorBrush
    public static void DrawCustomBrush(int px, int py, AdvancedMobilePaint paintEngine, IQuestable questable)
    {
        int startX =(px - paintEngine.customBrushWidth / 2);
        int startY =(py - paintEngine.customBrushHeight / 2);
        int pixel = (paintEngine.texWidth * startY + startX) * 4;
        int brushPixel = 0;
        bool skip = false;
        float yy = 0;
        float xx = 0;
        int pixel2 = 0;
        float lerpVal = 1f;

        byte[] test = new byte[0];
        if (paintEngine.brushMode == BrushProperties.Glittery)
        {
            if (paintEngine.IsDrag == true)
            {
                test = paintEngine.textureByteTest[Random.Range(0, paintEngine.textureByteTest.Count)];
            }
            else
            {
                return;
            }
        }

        for (int y = 0; y < paintEngine.customBrushHeight; y++)
        {
            for (int x = 0; x < paintEngine.customBrushWidth; x++)
            {
                brushPixel = (paintEngine.customBrushWidth * (y) + x) * 4;
                skip = false;
                if ((startX + x) > (paintEngine.texWidth - 2) || (startX + x) < -1)
                {
                    skip = true;
                }
                if (pixel < 0 || pixel >= paintEngine.pixels.Length)
                {
                    skip = true;
                }
                if (brushPixel < 0 || brushPixel > paintEngine.customBrushBytes.Length)
                {
                    skip = true;
                }
                if (!paintEngine.canDrawOnBlack && !skip)
                {
                    if (paintEngine.pixels[pixel + 3] != 0 && paintEngine.pixels[pixel] == 0 && paintEngine.pixels[pixel + 1] == 0 && paintEngine.pixels[pixel + 2] == 0)
                    {
                        skip = true;
                    }
                }
                if (paintEngine.customBrushBytes[brushPixel + 3] != 0 && !skip)
                {
                    if (paintEngine.useCustomBrushAlpha)
                    {
                        if (paintEngine.useAdditiveColors)
                        {
                            if (!paintEngine.useLockArea)
                            {
                                lerpVal = paintEngine.customBrushBytes[brushPixel + 3] / 255f;
                                switch (paintEngine.brushMode)
                                {
                                    //지우개로 색칠할시
                                    case BrushProperties.Clear:
                                        //TODO
                                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.clearColor.r, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.clearColor.g, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.clearColor.b, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.clearColor.a, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        if (questable != null && questable.IsChecking == true)
                                        {
                                            paintEngine.confirmPixels[pixel] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel], paintEngine.clearColor.r, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 1], paintEngine.clearColor.g, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 2], paintEngine.clearColor.b, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 3], paintEngine.clearColor.a, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        }
                                        break;
                                        //크림매니큐어 색칠할시
                                    case BrushProperties.Default:
                                        //TODO
                                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.paintColor.r, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a, lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        break;
                                    case BrushProperties.Simple:
                                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.customBrushBytes[brushPixel], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.customBrushBytes[brushPixel + 1], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.customBrushBytes[brushPixel + 2], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.customBrushBytes[brushPixel + 3], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        break;
                                        //펄 레인보우 매니큐어 색칠할시
                                    case BrushProperties.Pattern:
                                        //TODO
                                        /*float*/
                                        yy = Mathf.Repeat(/*py+y*/py + (y - paintEngine.customBrushHeight / 2f), paintEngine.customPatternHeight);
                                        /*float*/
                                        xx = Mathf.Repeat(/*px+x*/px + (x - paintEngine.customBrushWidth / 2f), paintEngine.customPatternWidth);
                                        /*int*/
                                        pixel2 = (int)Mathf.Repeat((paintEngine.customPatternWidth * xx + yy) * 4, paintEngine.patternBrushBytes.Length);            
                                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.patternBrushBytes[pixel2], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.patternBrushBytes[pixel2 + 1], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.patternBrushBytes[pixel2 + 2], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.patternBrushBytes[pixel2 + 3], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        break;
                                        //글리터리 색칠할시
                                    case BrushProperties.Glittery:
                                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], test[brushPixel], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], test[brushPixel + 1], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], test[brushPixel + 2], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], test[brushPixel + 3], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        //정답일 경우만 들어오는 이프문 -> 글리터리 정답확인용
                                        if (questable != null && questable.IsChecking == true)
                                        {
                                            paintEngine.confirmPixels[pixel] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel], test[brushPixel], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 1], test[brushPixel + 1], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 2], test[brushPixel + 2], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                            paintEngine.confirmPixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.confirmPixels[pixel + 3], test[brushPixel + 3], lerpVal/*paintEngine.customBrushBytes[brushPixel+3]/255f*/);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                pixel += 4;
            }
            pixel = (paintEngine.texWidth * (startY == 0 ? -1 : startY + y) + startX + 1) * 4;
        }
    }

    public static void DrawLineWithVectorBrush(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
    {
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistanceX = (int)(paintEngine.customBrushWidth >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int minDistanceY = (int)(paintEngine.customBrushHeight >> 1);
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistanceX || pixelCount > minDistanceY)
            {
                pixelCount = 0;
                DrawRectangle(x0, y0, paintEngine);
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    }

    public static void DrawRectangle(int px, int py, AdvancedMobilePaint paintEngine)
    {
        //Debug.Log ("DrawRectangle");
        int startX =/*(int)*/(px - paintEngine.customBrushWidth / 2);
        int startY =/*(int)*/(py - paintEngine.customBrushHeight / 2);
        int pixel = (paintEngine.texWidth * startY + startX) * 4;
        //int brushPixel = 0;
        bool skip = false;
        //float yy =0;
        //float xx =0;
        //int pixel2 = 0;
        for (int y = 0; y < paintEngine.customBrushHeight; y++)
        {
            for (int x = 0; x < paintEngine.customBrushWidth; x++)
            {
                skip = false;
                if ((startX + x) > (paintEngine.texWidth - 2) || (startX + x) < -1) skip = true;
                if (pixel < 0 || pixel >= paintEngine.pixels.Length)
                    skip = true;//
                if (!skip)
                {
                    //TODO: add more modes
                    paintEngine.pixels[pixel] = paintEngine.paintColor.r;
                    paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
                    paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
                    paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
                }
                pixel += 4;
            }//za x

            pixel = (paintEngine.texWidth * (startY == 0 ? -1 : startY + y) + startX + 1) * 4;
        }//za y
    }//DrawRectabgle() end


    // main painting function, http://stackoverflow.com/a/24453110
    /// <summary>
    /// Draws the circle.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// x좌표와 y좌표이 RawImage를 변경
    public static void DrawCircle(int x, int y, AdvancedMobilePaint paintEngine)
    {
        if (!paintEngine.canDrawOnBlack)
        {

            if (paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0
                && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0) return;
        }

        int pixel = 0;

        // draw fast circle: 
        int r2 = paintEngine.brushSize * paintEngine.brushSize;
        int area = r2 << 2;
        int rr = paintEngine.brushSize << 1;
        float lerpVal = 1f;
        lerpVal = paintEngine.paintColor.a / 255f * paintEngine.brushAlphaStrength;
        for (int i = 0; i < area; i++)
        {
            int tx = (i % rr) - paintEngine.brushSize;
            int ty = (i / rr) - paintEngine.brushSize;
            if (tx * tx + ty * ty < r2)
            {
                if (x + tx < 0 || y + ty < 0 || x + tx >= paintEngine.texWidth || y + ty >= paintEngine.texHeight) continue; // temporary fix for corner painting

                pixel = (paintEngine.texWidth * (y + ty) + x + tx) * 4;
                //pixel = ( texWidth*( (y+ty) % texHeight )+ (x+tx) % texWidth )*4;

                if (paintEngine.useAdditiveColors)
                {
                    // additive over white also
                    if (!paintEngine.useLockArea /*|| (paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                    {
                        //toLerpVal=
                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.paintColor.r, lerpVal);
                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g, lerpVal);
                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b, lerpVal);
                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a, lerpVal);
                    }
                }
                else
                { // no additive, just paint my colors
                    if (!paintEngine.useLockArea /*|| (paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                    {
                        paintEngine.pixels[pixel] = paintEngine.paintColor.r;
                        paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
                        paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
                        paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
                    }
                } // if additive
            } // if in circle
        } // for area
    } // DrawCircle()

    public static void DrawGlitteryCircle(int x, int y, AdvancedMobilePaint paintEngine)
    {
        if (!paintEngine.canDrawOnBlack)
        {
            if (paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0)
                return;
        }
        //int randomRotateValue = UnityEngine.Random.Range(0, paintEngine.patternBrushByteList.Count);
        int randomRotateValue = UnityEngine.Random.Range(0, paintEngine.textureByteTest.Count);
        int pixel = 0;
        //byte[] test = PaintUtils.RotateAngle(paintEngine.customPatternWidth, paintEngine.customPatternHeight, paintEngine.testcolor);
        byte[] test = paintEngine.textureByteTest[randomRotateValue];

        // draw fast circle:
        // 브러쉬 사이즈 * 브러쉬 사이즈
        int brushSize = paintEngine.customPatternWidth / 2;
        int r2 = brushSize * brushSize;//povrsina kruga             //r2 = 3600
        int area = r2 << 2;//sve rgb vrednosti koje cine povrsinu kruga -piksela u krugu    //area = 3600
                           //쉬프트 연산자
                           // << : b = a << c 라면
                           // b = a * 2^c
                           // >> : b = a >> c 라면
                           // b = a / 2^c

        int tx = 0;
        int ty = 0;
        int rr = brushSize << 1;//precnik kruga
        float lerpVal = 1f;
        for (int i = 0; i < area; i++)
        {
            /*int*/
            tx = (i % rr) - brushSize;
            /*int*/
            ty = (i / rr) - brushSize;

            if (x + tx < 0 || y + ty < 0 || x + tx >= paintEngine.texWidth || y + ty >= paintEngine.texHeight)
            {
                continue; // temporary fix for corner painting
            }

            pixel = (paintEngine.texWidth * (y + ty) + x + tx) * 4; // << 2
                                                                    //if(pixel<0 || pixel>pixels.Length) continue;
            if (paintEngine.useAdditiveColors)
            {
                if (!paintEngine.useLockArea /*||(paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                {
                    //Mathf.Repeat : 나머지 연산과 비슷함(소수점으로도 나올수 있음)
                    /*float*/
                    lerpVal = test[i * 4 + 3] / 255f * paintEngine.brushAlphaStrength;
                    paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], test[i * 4], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                    paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], test[i * 4 + 1], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                    paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], test[i * 4 + 2], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                    paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], test[i * 4 + 3], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                }
            }
            else // no additive, just paint my colors
            {
                if (!paintEngine.useLockArea /*|| /*(paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                {
                    //회전값 적용한 코드_230413 박진범
                    if (test[i * 4 + 3] > 0)
                    {
                        //Color32 clolorMixValue = ColorMix(
                        // paintEngine.pixels[pixel],
                        // paintEngine.pixels[pixel + 1],
                        // paintEngine.pixels[pixel + 2],
                        // paintEngine.pixels[pixel + 3],
                        // test[i * 4],
                        // test[i * 4 + 1],
                        // test[i * 4 + 2],
                        // test[i * 4 + 3]
                         //paintEngine.patternBrushByteList[randomRotateValue][i * 4],
                         //paintEngine.patternBrushByteList[randomRotateValue][i * 4 + 1],
                         //paintEngine.patternBrushByteList[randomRotateValue][i * 4 + 2],
                         //paintEngine.patternBrushByteList[randomRotateValue][i * 4 + 3]
                        // );

                        //paintEngine.pixels[pixel] = clolorMixValue.r;//r
                        //paintEngine.pixels[pixel + 1] = clolorMixValue.g;
                        //paintEngine.pixels[pixel + 2] = clolorMixValue.b;
                        //paintEngine.pixels[pixel + 3] = clolorMixValue.a;
                    }
                }
            } // if additive
        } // for area
    }

    #region DrawPatternCircle_Glittery Test ============
    private static Color32 ColorMix(byte r1, byte g1, byte b1, byte a1, byte r2, byte g2, byte b2, byte a2)
    {
        Color color1 = new Color32(r1, g1, b1, a1);
        Color color2 = new Color32(r2, g2, b2, a2);        
        return ColorMix(color1, color2);
    }

    private static Color ColorMix(Color c1, Color c2)
    {
        //원본_ 박진범
        //float oneminusalpha = 1 - c2.a;

        //float newR = (c1.r * c1.a * oneminusalpha) + (c2.r * c2.a);
        //float newG = (c1.g * c1.a * oneminusalpha) + (c2.g * c2.a);
        //float newB = (c1.b * c1.a * oneminusalpha) + (c2.b * c2.a);
        //return new Color(newR, newG, newB, 1);

        float oneminusalpha = 1 - c2.a;

        float newR = (c1.r * c1.a * oneminusalpha) + (c2.r * c2.a);
        float newG = (c1.g * c1.a * oneminusalpha) + (c2.g * c2.a);
        float newB = (c1.b * c1.a * oneminusalpha) + (c2.b * c2.a);
        float newA = (c1.a * oneminusalpha) + c2.a;

        return new Color(newR, newG, newB, newA);
    }
    #endregion DrawPatternCircle_Glittery Test ============

    /// <summary>
    /// Draws the pattern circle.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public static void DrawPearlrainbowCircle(int x, int y, AdvancedMobilePaint paintEngine)
    {
        //			Debug.Log ("DrawPatternCircle "+ x +" ,"+y);

        if (!paintEngine.canDrawOnBlack)
        {
            if (paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0)
                return;
        }

        int pixel = 0;

        // draw fast circle:
        // 브러쉬 사이즈 * 브러쉬 사이즈
        int r2 = paintEngine.brushSize * paintEngine.brushSize;//povrsina kruga             //r2 = 900
        int area = r2 << 2;//sve rgb vrednosti koje cine povrsinu kruga -piksela u krugu    //area = 3600
                           //쉬프트 연산자
                           // << : b = a << c 라면
                           // b = a * 2^c
                           // >> : b = a >> c 라면
                           // b = a / 2^c

        int rr = paintEngine.brushSize << 1;//precnik kruga                                 //rr = 60
        int tx = 0;
        int ty = 0;
        float yy = 0;
        float xx = 0;
        int pixel2 = 0;
        float lerpVal = 1f;
        for (int i = 0; i < area; i++)
        {
            /*int*/
            tx = (i % rr) - paintEngine.brushSize;
            /*int*/
            ty = (i / rr) - paintEngine.brushSize;

            if (tx * tx + ty * ty < r2)//(if in circle) 
            {
                if (x + tx < 0 || y + ty < 0 || x + tx >= paintEngine.texWidth || y + ty >= paintEngine.texHeight)
                {
                    continue; // temporary fix for corner painting
                }

                pixel = (paintEngine.texWidth * (y + ty) + x + tx) * 4; // << 2
                                                                        //if(pixel<0 || pixel>pixels.Length) continue;
                if (paintEngine.useAdditiveColors)
                {
                    // additive over white also
                    if (!paintEngine.useLockArea /*|| /*(paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                    {
                        //Mathf.Repeat : 나머지 연산과 비슷함(소수점으로도 나올수 있음)
                        /*float*/
                        yy = Mathf.Repeat(y + ty, paintEngine.customPatternWidth);
                        /*float*/
                        xx = Mathf.Repeat(x + tx, paintEngine.customPatternWidth);
                        /*int*/
                        pixel2 = (int)Mathf.Repeat((paintEngine.customPatternWidth * xx + yy) * 4, paintEngine.patternBrushBytes.Length);
                        lerpVal = paintEngine.patternBrushBytes[pixel2 + 3] / 255f * paintEngine.brushAlphaStrength;
                        paintEngine.pixels[pixel] = (byte)Mathf.Lerp(paintEngine.pixels[pixel], paintEngine.patternBrushBytes[pixel2], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                        paintEngine.pixels[pixel + 1] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 1], paintEngine.patternBrushBytes[pixel2 + 1], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                        paintEngine.pixels[pixel + 2] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 2], paintEngine.patternBrushBytes[pixel2 + 2], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                        paintEngine.pixels[pixel + 3] = (byte)Mathf.Lerp(paintEngine.pixels[pixel + 3], paintEngine.patternBrushBytes[pixel2 + 3], lerpVal/*paintEngine.patternBrushBytes[pixel2+3]/255f*paintEngine.brushAlphaStrength*/);
                    }
                }
                else // no additive, just paint my colors
                {
                    if (!paintEngine.useLockArea /*|| /*(paintEngine.useLockArea && paintEngine.lockMaskPixels[pixel] == 1)*/)
                    {
                        // TODO: pattern dynamic scalar value?

                        /*float*/
                        yy = Mathf.Repeat(y + ty, paintEngine.customPatternWidth);
                        /*float*/
                        xx = Mathf.Repeat(x + tx, paintEngine.customPatternWidth);//Debug.Log ("P"+xx+","+yy);
                        /*int*/
                        pixel2 = (int)Mathf.Repeat((paintEngine.customPatternWidth * xx + yy) * 4, paintEngine.patternBrushBytes.Length);

                        if (paintEngine.patternBrushBytes[pixel2 + 3] > 100) //FIXME CACKANO! Izlaz ako je pattern pixel alpha == 0
                        {
                            paintEngine.pixels[pixel] = paintEngine.patternBrushBytes[pixel2];//r
                            paintEngine.pixels[pixel + 1] = paintEngine.patternBrushBytes[pixel2 + 1];//g
                            paintEngine.pixels[pixel + 2] = paintEngine.patternBrushBytes[pixel2 + 2];//b                           
                            paintEngine.pixels[pixel + 3] = paintEngine.patternBrushBytes[pixel2 + 3];//a
                        }
                    }
                } // if additive
            } // if in circle
        } // for area
    } // DrawPatternCircle()


    // draw line between 2 points (if moved too far/fast)
    // http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    public static void DrawLine(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
    {
        //            Debug.Log("Drawing Line");
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(paintEngine.brushSize >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                DrawCircle(x0, y0, paintEngine);
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    } // drawline


    public static void DrawLineWithGlittery(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
    {
        //            Debug.Log("Drawing Line With Pattern");
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(paintEngine.brushSize >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                DrawGlitteryCircle(x0, y0, paintEngine);
            }
            if ((x0 == x1) && (y0 == y1))
            {
                loop = false;
            }
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    }

    public static void DrawLineWithPearlRainbow(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
    {
        //            Debug.Log("Drawing Line With Pattern");
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(paintEngine.brushSize >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                DrawPearlrainbowCircle(x0, y0, paintEngine);
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    }


    #endregion VectorBrush
}
