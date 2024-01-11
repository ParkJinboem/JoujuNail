using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CollectionBehaviour : CollectionItemBehaviour
{
    [SerializeField] private Sprite creameManicureSprite;
    [SerializeField] private Sprite glitteryManicureSprite;
    [SerializeField] private Sprite pearlRainbowManicureSprite;
    [SerializeField] private Sprite patternSprite;
    [SerializeField] private GameObject particle;
    
    [Header("CollcetionShader")]
    [SerializeField] private Material DefaultShader;
    [SerializeField] private Material whiteShader;
    [SerializeField] private Material blackShader;
    [SerializeField] GameObject collectionShaderPrefabs;
    private GameObject[] shaderObj;

    private Image itemImage;
    public Image ItemImage
    {
        get { return itemImage; }
        set { itemImage = value; }
    }

    private ManicureData manicureData;
    public ManicureData ManicureData
    {
        get { return manicureData; }
        set { manicureData = value; }
    }

    private CollectionItem collectionItem;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="isHave"></param>
    public void Init(bool isHave, string baseColor)
    {
        //타입이 매니큐어일경우
        if (manicureData.type == "Manicure")
        {
            itemImage.rectTransform.sizeDelta = new Vector2(63, 91);

            //해당 콜렉션데이터를 가지고 있을경우
            if (isHave == true)
            {
                itemImage.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureData.useOffSpriteName);
            }
            //아닐경우
            else
            {
                if (manicureData.mode == "Cream")
                {
                    itemImage.sprite = creameManicureSprite;
                }
                else if (manicureData.mode == "Glittery")
                {
                    itemImage.sprite = glitteryManicureSprite;
                }
                else
                {
                    itemImage.sprite = pearlRainbowManicureSprite;
                }
            }
        }
        //타입이 패턴일경우
        else if (manicureData.type == "Pattern")
        {
            itemImage.rectTransform.sizeDelta = new Vector2(114, 154);

            //해당 콜렉션데이터를 가지고 있을경우
            if (isHave == true)
            {
                itemImage.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureData.useOffSpriteName);
            }
            //아닐경우
            else
            {
                itemImage.sprite = patternSprite;
            }
        }
        //타입이 스키터일경우
        else
        {
            itemImage.sprite = DataManager.Instance.GetManicureBottleItemSprite(manicureData.useOffSpriteName);
            itemImage.rectTransform.sizeDelta = new Vector2(128, 128);

            //해당 콜렉션데이터를 가지고 있을경우
            if (isHave == true)
            {
                if(transform.childCount != 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        this.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                shaderObj = new GameObject[3];
                for (int i = 0; i < 3; i++)
                {
                    shaderObj[i] = Instantiate(collectionShaderPrefabs, this.transform);
                    shaderObj[i].GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                    shaderObj[i].transform.localPosition = Vector3.zero;
                }
                shaderObj[0].GetComponent<Image>().material = whiteShader;
                shaderObj[1].GetComponent<Image>().material = blackShader;
                shaderObj[2].GetComponent<Image>().material = DefaultShader;
                shaderObj[0].transform.localPosition = new Vector3(0, -1f, 0);
                shaderObj[1].transform.localPosition = new Vector3(0, +1f, 0);
            }

        }
    }

    /// <summary>
    /// 파티클 생성
    /// </summary>
    public void ParticleOn()
    {
        GameObject particleObject = Instantiate(particle, transform.parent.parent);
        SoundManager.Instance.PlayEffectSound("Preview2");
        particleObject.transform.position = transform.position;
    }

    public override void TriggerOn(CollectionItem collectionItem)
    {
        if(manicureData != null && collectionItem.ManicureData.id == manicureData.id)
        {
            this.collectionItem = collectionItem;
        }
    }
}