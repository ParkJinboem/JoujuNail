using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [SerializeField] private QuestPaintScenePool questPaintScenePool;
    [SerializeField] private PaintScenePool paintScenePool;
    [SerializeField] private AlbumScenePool albumScenePool;

    /// <summary>
    /// 퀘스트씬 생성 
    /// </summary>
    /// <param name="level"></param>
    /// <param name="bgObject"></param>
    /// <param name="transformUI"></param>
    public void CreateQuestPaintScene(int level, GameObject bgObject, Transform transformUI)
    {
        QuestPaintSceneSpawn(level, bgObject, transformUI);
    }

    public void QuestPaintSceneSpawn(int level, GameObject bgObject, Transform transformUI)
    {
        //퀘스트씬의 로딩씬 스크립트
        QuestSceneIntroController questSceneIntroController = questPaintScenePool.Pop(transformUI.position).instance.GetComponent<QuestSceneIntroController>();
        questSceneIntroController.Init(level, bgObject);
    }

    /// <summary>
    /// 프리씬 생성 
    /// </summary>
    /// <param name="bgObject"></param>
    /// <param name="transformUI"></param>
    public void CreatePaintScene( GameObject bgObject, Transform transformUI)
    {
        PaintSceneSpawn(bgObject, transformUI);
    }

    public void PaintSceneSpawn(GameObject bgObject, Transform transformUI)
    {
        PaintSceneIntroController paintSceneIntroController = paintScenePool.Pop(transformUI.position).instance.GetComponent<PaintSceneIntroController>();
        paintSceneIntroController.Init(bgObject);
    }

    /// <summary>
    /// 앨범씬 생성 
    /// </summary>
    /// <param name="transformUI"></param>
    public void CreateAlbumScene(Transform transformUI)
    {
        AlbumSceneSpawn(transformUI);
    }

    public void AlbumSceneSpawn(Transform transformUI)
    {
        AlbumSceneIntroController albumSceneIntroController = albumScenePool.Pop(transformUI.position).instance.GetComponent<AlbumSceneIntroController>();
        albumSceneIntroController.Init();
    }
}
