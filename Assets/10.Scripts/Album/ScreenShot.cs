using System.Collections;
using UnityEngine;
using System.IO;
using System;
using OnDot.System;

public class ScreenShot : MonoBehaviour
{
    private Camera cam;
    public Camera Cam
    {
        set { cam = value; }
    }

    private int resWidth;
    private int resHeight;

    void Awake()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
        cam = Camera.main;
    }

    public void ClickScreenShot()
    {
        TakeScreenShot();
    }

    public void TakeScreenShot()
    {
        StartCoroutine(IRequestNativeGalleryPermission());
    }

    private IEnumerator IRequestNativeGalleryPermission()
    {
        yield return new WaitForEndOfFrame();

        NativeGallery.Permission permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Granted)
        {
            StartCoroutine(Capture());
        }
    }

    private IEnumerator Capture()
    {
        AlbumSceneManager.Instance.Album.uiObject.SetActive(false);
        SoundManager.Instance.PlayEffectSound("ScreenShot");
        cam.cullingMask = ~(1 << LayerMask.NameToLayer("UI"));
        yield return null;

        RenderTexture rt = RenderTexture.GetTemporary(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, screenShot.width, screenShot.height);

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        string path = OnDotManager.Instance.CaptureFolderName;

        yield return null;

        SaveTextureAsPNG(screenShot, path, fileName);
        
        RenderTexture.active = null;
        cam.targetTexture = null;
        RenderTexture.ReleaseTemporary(rt);
        cam.cullingMask = -1;

        AlbumSceneManager.Instance.Album.uiObject.SetActive(true);
    }

    public void SaveTextureAsPNG(Texture2D texture, string path, string name)
    {
        byte[] bytes = texture.EncodeToPNG();
        string title;
#if UNITY_EDITOR
        File.WriteAllBytes(Application.dataPath + "/ScreenShot/" + name, bytes);
        title = Application.dataPath + "/ScreenShot/" + name + "으로 저장되었습니다.";
#elif UNITY_IOS
            NativeGallery.SaveImageToGallery(texture, path, name);
            //title = string.Format(LocalizationManager.GetTermTranslation("SaveAlbum"), path);
#elif UNITY_ANDROID
            NativeGallery.SaveImageToGallery(texture, path, name);
            //title = string.Format(LocalizationManager.GetTermTranslation("SavePath"), path, name);
           
            //원본_230307 박진범
            //title = string.Format(LocalizationManager.GetTermTranslation("SavePath"), path, name);
#endif
    }
}