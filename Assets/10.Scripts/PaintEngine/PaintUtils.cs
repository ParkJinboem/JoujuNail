using UnityEngine;
using System;
using System.Collections.Generic;

public class PaintUtils : MonoBehaviour
{
	public static Dictionary<int, byte[]> colortests = new Dictionary<int, byte[]>();

	/// <summary>
	/// Rotates the texture.
	/// </summary>
	/// <param name="tex"></param>
	/// <param name="angle"></param>
	/// <returns></returns>
	public static byte[] RotateAngle(int w, int h, Color[] tex)
    {
		int angle = UnityEngine.Random.Range(0, 36) * 10;
		byte[] result2;

		//if (colortests.TryGetValue(angle, out result2))
        //{
		//	return result2;
		//}

		int x, y;
		float x1, y1, x2, y2;

		float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
		float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

		float dx_x = rot_x(angle, 1.0f, 0.0f);
		float dx_y = rot_y(angle, 1.0f, 0.0f);
		float dy_x = rot_x(angle, 0.0f, 1.0f);
		float dy_y = rot_y(angle, 0.0f, 1.0f);

		x1 = x0;
		y1 = y0;
		Color[] result = new Color[tex.Length];
		Color c;
		for (x = 0; x < w; x++)
		{
			x2 = x1;
			y2 = y1;
			for (y = 0; y < h; y++)
			{
				x2 += dx_x;//rot_x(angle, x1, y1);
				y2 += dx_y;//rot_y(angle, x1, y1);
						   //
				int pixX = (int)Mathf.Floor(x2);
				int pixY = (int)Mathf.Floor(y2);
				if (pixX >= w || pixX < 0 || pixY >= h || pixY < 0)
				{
					c = Color.clear;
				}
				else
				{
					c = tex[pixX * w + pixY];// = tex.GetPixel(x1,y1);
				}
				result[(int)Mathf.Floor(x) * w + (int)Mathf.Floor(y)] = c;
			}

			x1 += dy_x;
			y1 += dy_y;
		}

		result2 = new byte[w * h * 4];
		int pixel = 0;
		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				Color brushPixel = result[i * h + j];
				result2[pixel] = (byte)(brushPixel.r * 255);
				result2[pixel + 1] = (byte)(brushPixel.g * 255);
				result2[pixel + 2] = (byte)(brushPixel.b * 255);
				result2[pixel + 3] = (byte)(brushPixel.a * 255);
				pixel += 4;
			}
		}
		//colortests.Add(angle, result2);
		return result2;
	}

	public static Texture2D RotateTexture(Texture2D tex, float angle)
	{
		Texture2D rotImage = new Texture2D(tex.width, tex.height, tex.format, false);
		int x, y;
		float x1, y1, x2, y2;

		int w = tex.width;
		int h = tex.height;
		float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
		float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

		float dx_x = rot_x(angle, 1.0f, 0.0f);
		float dx_y = rot_y(angle, 1.0f, 0.0f);
		float dy_x = rot_x(angle, 0.0f, 1.0f);
		float dy_y = rot_y(angle, 0.0f, 1.0f);


		x1 = x0;
		y1 = y0;
		Color32[] pixels = tex.GetPixels32(0);
		Color32[] result = new Color32[pixels.Length];
		Color c;
		int pixX = 0;
		int pixY = 0;
		//			int pixelPos=0;
		for (x = 0; x < tex.width; x++)
		{
			x2 = x1;
			y2 = y1;
			for (y = 0; y < tex.height; y++)
			{
				x2 += dx_x;//rot_x(angle, x1, y1);
				y2 += dx_y;//rot_y(angle, x1, y1);
						   //
				pixX = (int)Mathf.Floor(x2);
				pixY = (int)Mathf.Floor(y2);
				if (pixX >= tex.width || pixX < 0 || pixY >= tex.height || pixY < 0)
				{
					c = Color.clear;
				}
				else
				{
					c = (Color)pixels[pixX * w + pixY];// = tex.GetPixel(x1,y1);
				}
				result[(int)Mathf.Floor(x) * w + (int)Mathf.Floor(y)] = (Color32)c;
			}

			x1 += dy_x;
			y1 += dy_y;

		}
		rotImage.SetPixels32(result);
		rotImage.Apply();
		return rotImage;
	}

	public static byte[] RotateTexturewithByte(int texWidth, int texHeight, byte[] testa, float angle)
	{
		int x, y;
		float x1, y1, x2, y2;

		int w = texWidth;
		int h = texHeight;
		float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
		float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

		float dx_x = rot_x(angle, 1.0f, 0.0f);
		float dx_y = rot_y(angle, 1.0f, 0.0f);
		float dy_x = rot_x(angle, 0.0f, 1.0f);
		float dy_y = rot_y(angle, 0.0f, 1.0f);

		x1 = x0;
		y1 = y0;

		byte[] newa = new byte[testa.Length];
		byte c;
		int pixX = 0;
		int pixY = 0;
		for (x = 0; x < w; x++)
		{
			x2 = x1;
			y2 = y1;
			for (y = 0; y < h; y++)
			{
				x2 += dx_x;//rot_x(angle, x1, y1);
				y2 += dx_y;//rot_y(angle, x1, y1);
						   //
				pixX = (int)Mathf.Floor(x2);
				pixY = (int)Mathf.Floor(y2);
				if (pixX >= w || pixX < 0 || pixY >= h || pixY < 0)
				{
					
				}
				else
				{
					int index = (x * w * 4) + (y * 4);
					newa[index] = testa[index];// = tex.GetPixel(x1,y1);
					newa[index + 1] = testa[index + 1];// = tex.GetPixel(x1,y1);
					newa[index + 2] = testa[index + 2];// = tex.GetPixel(x1,y1);
					newa[index + 3] = testa[index + 3];// = tex.GetPixel(x1,y1);
				}
			}
			x1 += dy_x;
			y1 += dy_y;
		}
		return newa;
	}

	private static float rot_x(float angle, float x, float y)
	{
		float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
		float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
		return (x * cos + y * (-sin));
	}
	private static float rot_y(float angle, float x, float y)
	{
		float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
		float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
		return (x * sin + y * cos);
	}

	/// <summary>
	/// Converts the Sprite to Texture2D. Texture's Mesh Type needs to be set to Full Rect!
	/// </summary>
	/// <returns>o Texture2D.</returns>
	/// <param name="sr">Sprite</param>
	public static Texture2D ConvertSpriteToTexture2D(Sprite sr)
	{
        Texture2D texTex = new Texture2D((int)sr.textureRect.width, (int)sr.textureRect.height, TextureFormat.ARGB32, false);
		Color[] tmp = sr.texture.GetPixels((int)sr.textureRect.x,
										   (int)sr.textureRect.y,
										   (int)sr.textureRect.width,
										   (int)sr.textureRect.height, 0);
		texTex.SetPixels(tmp, 0);
		texTex.Apply(false, false);
		return texTex;
	}

    /// <summary>
    /// Sprite를 Texture로 반환
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public static Texture2D TextureFromSprite(Sprite sprite)
	{
		Texture2D texTex = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height, TextureFormat.ARGB32, false);
		Color[] tmp = sprite.texture.GetPixels((int)sprite.textureRect.x,
										   (int)sprite.textureRect.y,
										   (int)sprite.textureRect.width,
										   (int)sprite.textureRect.height, 0);
		texTex.SetPixels(tmp, 0);
		texTex.Apply(false, false);
		return texTex;
	}

	/// <summary>
	/// Texture를 String으로 변환 
	/// </summary>
	/// <param name="tex"></param>
	/// <returns></returns>
	public static string StringFromTexture(Texture2D tex)
	{
		byte[] texByte = tex.EncodeToPNG();
		if (texByte == null)
		{
			string base64Tex = null;
			return base64Tex;
		}
		else
		{
			string base64Tex = System.Convert.ToBase64String(texByte);
			return base64Tex;
		}
	}

	/// <summary>
	/// Texture를 Sprite로 반환
	/// </summary>
	/// <param name="tex"></param>
	/// <returns></returns>
	public static Sprite SpriteFromTexture(Texture2D tex)
	{
		Rect rect = new Rect(0, 0, tex.width, tex.height);
		Sprite sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
		return sprite;
	}

	/// <summary>
	/// String을 Texture로 반환 
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static Texture2D TextureFromString(string str)
	{
		byte[] bytes = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(bytes);
		return texture2D;
	}
}
