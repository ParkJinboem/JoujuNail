using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private SpriteAtlas spriteAtlas;

    private Image image;

    private CollectionObject poolObject;
    public CollectionObject PoolObject
    {
        set { poolObject = value; }
    }

    private ManicureData manicureData;
    public ManicureData ManicureData
    {
        get { return manicureData; }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    /// <param name="manicureData"></param>
    public void Init(ManicureData manicureData)
    {
        this.manicureData = manicureData;

        transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        transform.localScale = new Vector3(1f, 1f, 1f);

        image.sprite = spriteAtlas.GetSprite(manicureData.useOffSpriteName);
        image.rectTransform.eulerAngles = new Vector3(0, 0, manicureData.collectionRotZ);

        if (manicureData.type == "Manicure")
        {
            image.rectTransform.sizeDelta = new Vector2(63, 91);
        }
        else if (manicureData.type == "Pattern")
        {
            image.rectTransform.sizeDelta = new Vector2(114, 154);
        }
        else
        {
            image.rectTransform.sizeDelta = new Vector2(128 * 1.2f, 128 * 1.2f);
        }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
