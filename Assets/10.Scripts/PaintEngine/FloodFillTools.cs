using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloodFillTools
{
	public static void LockAreaFillWithThresholdMaskOnly(int x, int y, AdvancedMobilePaint paintEngine)
	{
		Debug.Log("LockAreaFillWithThresholdMaskOnly");

		// get canvas color from this point
		byte hitColorR = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 0];
		byte hitColorG = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 1];
		byte hitColorB = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 2];
		byte hitColorA = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 3];

		if (!paintEngine.canDrawOnBlack)
		{
			if (hitColorR == 0 && hitColorG == 0 && hitColorB == 0 && hitColorA != 0) {/*Debug.Log ("CANT DRAW ON BLACK");*/return; }
		}

		Queue<int> fillPointX = new Queue<int>();
		Queue<int> fillPointY = new Queue<int>();
		fillPointX.Enqueue(x);
		fillPointY.Enqueue(y);

		int ptsx, ptsy;
		int pixel = 0;

		paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];

		while (fillPointX.Count > 0)
		{

			ptsx = fillPointX.Dequeue();
			ptsy = fillPointY.Dequeue();

			if (ptsy - 1 > -1)
			{
				pixel = (paintEngine.texWidth * (ptsy - 1) + ptsx) * 4; // down

				if (paintEngine.lockMaskPixels[pixel] == 0 // this pixel is not used yet
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 1], hitColorG))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 2], hitColorB))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 3], hitColorA)))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy - 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx + 1 < paintEngine.texWidth)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx + 1) * 4; // right
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 1], hitColorG))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 2], hitColorB))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 3], hitColorA)))
				{
					fillPointX.Enqueue(ptsx + 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx - 1 > -1)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx - 1) * 4; // left
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 1], hitColorG))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 2], hitColorB))
					&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 3], hitColorA)))
				{
					fillPointX.Enqueue(ptsx - 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsy + 1 < paintEngine.texHeight)
			{
				pixel = (paintEngine.texWidth * (ptsy + 1) + ptsx) * 4; // up
																		//                    if(pixel > paintEngine.lockMaskPixels.Length-1) return;
				try
				{
					if (paintEngine.lockMaskPixels[pixel] == 0
						&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 0], hitColorR)) // if pixel is same as hit color OR same as paint color
						&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 1], hitColorG))
						&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 2], hitColorB))
						&& (paintEngine.CompareThreshold(paintEngine.maskPixels[pixel + 3], hitColorA)))
					{
						fillPointX.Enqueue(ptsx);
						fillPointY.Enqueue(ptsy + 1);
						paintEngine.lockMaskPixels[pixel] = 1;
					}
				}
				catch
				{
					//                        Debug.Log("e");
					//                        Debug.Log(pixel +" " + paintEngine.maskPixels.Length);                            
					Debug.Log(pixel + " " + paintEngine.lockMaskPixels.Length);

				}
			}
		}
	} // LockMaskFillWithTreshold

	public static void LockMaskFillWithThreshold(int x, int y, AdvancedMobilePaint paintEngine)
	{
		Debug.Log("LockMaskFillWithTreshold");
		// get canvas color from this point
		byte hitColorR = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 0];
		byte hitColorG = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 1];
		byte hitColorB = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 2];
		byte hitColorA = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 3];

		if (!paintEngine.canDrawOnBlack)
		{
			if (hitColorR == 0 && hitColorG == 0 && hitColorB == 0 && hitColorA != 0) return;
		}

		Queue<int> fillPointX = new Queue<int>();
		Queue<int> fillPointY = new Queue<int>();
		fillPointX.Enqueue(x);
		fillPointY.Enqueue(y);

		int ptsx, ptsy;
		int pixel = 0;

		paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];

		while (fillPointX.Count > 0)
		{

			ptsx = fillPointX.Dequeue();
			ptsy = fillPointY.Dequeue();

			if (ptsy - 1 > -1)
			{
				pixel = (paintEngine.texWidth * (ptsy - 1) + ptsx) * 4; // down

				if (paintEngine.lockMaskPixels[pixel] == 0 // this pixel is not used yet
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], hitColorR) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], paintEngine.paintColor.r)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], hitColorG) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], hitColorB) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], hitColorA) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a)))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy - 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx + 1 < paintEngine.texWidth)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx + 1) * 4; // right
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], hitColorR) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], paintEngine.paintColor.r)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], hitColorG) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], hitColorB) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], hitColorA) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a)))
				{
					fillPointX.Enqueue(ptsx + 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx - 1 > -1)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx - 1) * 4; // left
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], hitColorR) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], paintEngine.paintColor.r)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], hitColorG) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], hitColorB) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], hitColorA) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a)))
				{
					fillPointX.Enqueue(ptsx - 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsy + 1 < paintEngine.texHeight)
			{
				pixel = (paintEngine.texWidth * (ptsy + 1) + ptsx) * 4; // up
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], hitColorR) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 0], paintEngine.paintColor.r)) // if pixel is same as hit color OR same as paint color
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], hitColorG) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 1], paintEngine.paintColor.g))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], hitColorB) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 2], paintEngine.paintColor.b))
					&& (paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], hitColorA) || paintEngine.CompareThreshold(paintEngine.pixels[pixel + 3], paintEngine.paintColor.a)))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy + 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}
		}
	} // LockMaskFillWithTreshold

	/// <summary>
	/// Floodfill by using mask lock area.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public static void LockAreaFillMaskOnly(int x, int y, AdvancedMobilePaint paintEngine)
	{
		//			Debug.Log ("LockAreaFillMaskOnly " + x + " " + y);
		//            Debug.Log(paintEngine.maskPixels.Length);
		byte hitColorR = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 0];
		byte hitColorG = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 1];
		byte hitColorB = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 2];
		byte hitColorA = paintEngine.maskPixels[((paintEngine.texWidth * (y) + x) * 4) + 3];

		if (!paintEngine.canDrawOnBlack)
		{
			if (hitColorR == 0 && hitColorG == 0 && hitColorB == 0 && hitColorA != 0) return;
		}

		Queue<int> fillPointX = new Queue<int>();
		Queue<int> fillPointY = new Queue<int>();
		fillPointX.Enqueue(x);
		fillPointY.Enqueue(y);

		int ptsx, ptsy;
		int pixel = 0;

		paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];

		while (fillPointX.Count > 0)
		{

			ptsx = fillPointX.Dequeue();
			ptsy = fillPointY.Dequeue();

			if (ptsy - 1 > -1)
			{
				pixel = (paintEngine.texWidth * (ptsy - 1) + ptsx) * 4; // down

				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.maskPixels[pixel + 0] == hitColorR || paintEngine.maskPixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.maskPixels[pixel + 1] == hitColorG || paintEngine.maskPixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.maskPixels[pixel + 2] == hitColorB || paintEngine.maskPixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.maskPixels[pixel + 3] == hitColorA || paintEngine.maskPixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy - 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx + 1 < paintEngine.texWidth)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx + 1) * 4; // right
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.maskPixels[pixel + 0] == hitColorR || paintEngine.maskPixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.maskPixels[pixel + 1] == hitColorG || paintEngine.maskPixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.maskPixels[pixel + 2] == hitColorB || paintEngine.maskPixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.maskPixels[pixel + 3] == hitColorA || paintEngine.maskPixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx + 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx - 1 > -1)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx - 1) * 4; // left
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.maskPixels[pixel + 0] == hitColorR || paintEngine.maskPixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.maskPixels[pixel + 1] == hitColorG || paintEngine.maskPixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.maskPixels[pixel + 2] == hitColorB || paintEngine.maskPixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.maskPixels[pixel + 3] == hitColorA || paintEngine.maskPixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx - 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsy + 1 < paintEngine.texHeight)
			{
				pixel = (paintEngine.texWidth * (ptsy + 1) + ptsx) * 4; // up
				try
				{
					if (paintEngine.lockMaskPixels[pixel] == 0
						&& (paintEngine.maskPixels[pixel + 0] == hitColorR || paintEngine.maskPixels[pixel + 0] == paintEngine.paintColor.r)
						&& (paintEngine.maskPixels[pixel + 1] == hitColorG || paintEngine.maskPixels[pixel + 1] == paintEngine.paintColor.g)
						&& (paintEngine.maskPixels[pixel + 2] == hitColorB || paintEngine.maskPixels[pixel + 2] == paintEngine.paintColor.b)
						&& (paintEngine.maskPixels[pixel + 3] == hitColorA || paintEngine.maskPixels[pixel + 3] == paintEngine.paintColor.a))
					{
						fillPointX.Enqueue(ptsx);
						fillPointY.Enqueue(ptsy + 1);
						paintEngine.lockMaskPixels[pixel] = 1;
					}
				}
				catch
				{
					Debug.Log("e");
				}
			}
		}
	} // LockAreaFillMaskOnly

	public static void LockAreaFill(int x, int y, AdvancedMobilePaint paintEngine)
	{
		Debug.Log("LockAreaFill");
		byte hitColorR = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 0];
		byte hitColorG = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 1];
		byte hitColorB = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 2];
		byte hitColorA = paintEngine.pixels[((paintEngine.texWidth * (y) + x) * 4) + 3];

		if (!paintEngine.canDrawOnBlack)
		{
			if (hitColorR == 0 && hitColorG == 0 && hitColorB == 0 && hitColorA != 0) return;
		}

		Queue<int> fillPointX = new Queue<int>();
		Queue<int> fillPointY = new Queue<int>();
		fillPointX.Enqueue(x);
		fillPointY.Enqueue(y);

		int ptsx, ptsy;
		int pixel = 0;

		paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];

		while (fillPointX.Count > 0)
		{

			ptsx = fillPointX.Dequeue();
			ptsy = fillPointY.Dequeue();

			if (ptsy - 1 > -1)
			{
				pixel = (paintEngine.texWidth * (ptsy - 1) + ptsx) * 4; // down

				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.pixels[pixel + 0] == hitColorR || paintEngine.pixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.pixels[pixel + 1] == hitColorG || paintEngine.pixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.pixels[pixel + 2] == hitColorB || paintEngine.pixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.pixels[pixel + 3] == hitColorA || paintEngine.pixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy - 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx + 1 < paintEngine.texWidth)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx + 1) * 4; // right
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.pixels[pixel + 0] == hitColorR || paintEngine.pixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.pixels[pixel + 1] == hitColorG || paintEngine.pixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.pixels[pixel + 2] == hitColorB || paintEngine.pixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.pixels[pixel + 3] == hitColorA || paintEngine.pixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx + 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsx - 1 > -1)
			{
				pixel = (paintEngine.texWidth * ptsy + ptsx - 1) * 4; // left
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.pixels[pixel + 0] == hitColorR || paintEngine.pixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.pixels[pixel + 1] == hitColorG || paintEngine.pixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.pixels[pixel + 2] == hitColorB || paintEngine.pixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.pixels[pixel + 3] == hitColorA || paintEngine.pixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx - 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}

			if (ptsy + 1 < paintEngine.texHeight)
			{
				pixel = (paintEngine.texWidth * (ptsy + 1) + ptsx) * 4; // up
				if (paintEngine.lockMaskPixels[pixel] == 0
					&& (paintEngine.pixels[pixel + 0] == hitColorR || paintEngine.pixels[pixel + 0] == paintEngine.paintColor.r)
					&& (paintEngine.pixels[pixel + 1] == hitColorG || paintEngine.pixels[pixel + 1] == paintEngine.paintColor.g)
					&& (paintEngine.pixels[pixel + 2] == hitColorB || paintEngine.pixels[pixel + 2] == paintEngine.paintColor.b)
					&& (paintEngine.pixels[pixel + 3] == hitColorA || paintEngine.pixels[pixel + 3] == paintEngine.paintColor.a))
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy + 1);
					paintEngine.lockMaskPixels[pixel] = 1;
				}
			}
		}
	} // LockAreaFill
}
