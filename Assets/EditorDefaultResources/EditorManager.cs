using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    [Header("Data")]
    public EditorAlbumData editorAlbumData;
    public int startStage;
    public int clearStage;

    [Header("EditorPaint")]
    public GameObject paintScene;

    [Header("Album")]
    public GameObject albumScene;
    public GameObject editorAlbumtPrefabs;
    public Transform albumParent;

    public void Start()
    {
        AddressableManager.Instance.LoadAssets();
        startStage = 1;
        clearStage = 140;
        Invoke("DataInit", 1.0f);
    }

    #region Data
    public void DataInit()
    {
        DataManager.Instance.Init();
        CreateAlbumData();
    }


    public void CreateAlbumData()
    {
        for (int i = startStage; i < clearStage + 1; i++)
        {
            editorAlbumData.Init(i);
        }
    }
    #endregion


    #region Data
    public void ClickedEditorPaint()
    {
        paintScene.SetActive(true);
    }
    public void PaintClose()
    {
        EditorPaintSceneManager.Instance.DeleteData();
        paintScene.SetActive(false);
    }
    #endregion


    #region Album

    public void ClickedEditorAlbum()
    {
        albumScene.SetActive(true);

        for (int i = 1; i < clearStage + 1; i++)
        {
            GameObject editorAlbum = Instantiate(editorAlbumtPrefabs, albumParent);
            editorAlbum.GetComponent<EditorAlbum>().Init(i);
        }
    }

    public void AlbumClose()
    {
        for (int i = 0; i < albumParent.childCount; i++)
        {
            Destroy(albumParent.GetChild(i).gameObject);
        }
        albumScene.SetActive(false);
    }

    #endregion

}
