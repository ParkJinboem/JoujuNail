using Coffee.UISoftMask;
using UnityEngine;
using UnityEngine.UI;

public class StickerConverter : MonoBehaviour
{
    public static StickerConverter currentStickerConverter;

    [SerializeField] private StickerItem stickerItem;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Transform basicTransform;
    [SerializeField] private GameObject editTool;

    private AdvancedMobilePaint paintEngine;

    public LayerMask stickLayerMask;
    public StickerType stickerType;

    private float distance;
    private float ratio;
    private float clamped;

    private Vector3 firstCheckPosition;
    private Vector3 posToCheck;

    private bool hasMoved;
    private bool borderHit;

    [Header("Sticker or CharacterSticker")]
    [SerializeField] private Image stickerImage;
    public Image StickerImage
    {
        get { return stickerImage; }
    }
    [SerializeField] private Image subStickerImage;

    [Header("AnimationSticker")]
    private GameObject animationStickerObject;
    public GameObject AnimationStickerObject
    {
        get { return animationStickerObject; }
    }
    private GameObject subAnimationStickerObject;
    private Animator animationStickerAnimator;
    public Animator AnimationStickerAnimator
    {
        get { return animationStickerAnimator; }
    }

    private bool isInit;

    private IQuestable questable;
    private IFreeable freeable;

    /// <summary>
    /// 초기값 셋팅 
    /// </summary>
    /// <param name="advancedMobilePaint"></param>
    /// <param name="manicureInfoData"></param>
    /// <param name="questable"></param>
    /// <param name="freeable"></param>
    public void Init(AdvancedMobilePaint advancedMobilePaint, ManicureInfoData manicureInfoData, IQuestable questable, IFreeable freeable)
    {
        QuestConfirmBehaviour.OnChangeParentHandler += ChangeParent;
        this.questable = questable;
        this.freeable = freeable;
        basicTransform = this.transform.parent.transform;
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        //rectTransform.sizeDelta = new Vector2(256, 256);

        if(freeable == null)
        {
            if (questable.SelectPaintEngine == questable.PaintEngines[0] && Statics.selectType == SelectType.Foot)
            {
                rectTransform.sizeDelta = new Vector2(256, 256);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(128, 128);
            }
        }
        else
        {
            if (freeable.SelectPaintEngine == freeable.PaintEngines[0] && Statics.selectType == SelectType.Foot)
            {
                rectTransform.sizeDelta = new Vector2(256, 256);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(128, 128);
            }
        }
        paintEngine = advancedMobilePaint;
        paintEngine.drawEnabled = false;
        paintEngine.movable = true;

        currentStickerConverter = this;

        stickerType = System.Enum.Parse<StickerType>(manicureInfoData.type);
        string spriteName = manicureInfoData.spriteName;
        GameObject animationStickerPrefab = AddressableManager.Instance.GetAssetByKey<GameObject>(spriteName);

        if (stickerType == StickerType.AnimationSticker)
        {
            //Maskable : true
            animationStickerObject = Instantiate(animationStickerPrefab, transform);
            //Maskable : false
            subAnimationStickerObject = Instantiate(animationStickerPrefab, transform);
            SoftMaskable softMaskable = subAnimationStickerObject.GetComponent<SoftMaskable>();
            softMaskable.enabled = false;

            Image subImage = subAnimationStickerObject.GetComponent<Image>();
            subImage.color = new Color(1, 1, 1, 0.5f);
            subImage.maskable = false;

            animationStickerAnimator = animationStickerObject.GetComponent<Animator>();

            stickerImage.color = new Color(1, 1, 1, 0);
            subStickerImage.color = new Color(1, 1, 1, 0);
        }
        else if (stickerType == StickerType.Sticker || stickerType == StickerType.CharacterSticker)
        {
            //Maskable : true
            stickerImage.sprite = DataManager.Instance.GetManicureItemSprite(manicureInfoData.spriteName);
            //Maskable : false
            subStickerImage.sprite = DataManager.Instance.GetManicureItemSprite(manicureInfoData.spriteName);
        }

        boxCollider2D.size = new Vector2(rectTransform.rect.width + 60, rectTransform.rect.height + 60);
        if (Statics.gameMode == GameMode.Free)
        {
            freeable.ChangeEidtParent(this);
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            questable.ChangeEidtParent(this);
        }
        editTool.SetActive(true);

        isInit = true;
    }

    private void Update()
    {
        if (isInit == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0);

            if (hit.collider == null)
            {
                DeselectItem();
            }
            else if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<StickerConverter>() == null)
                {
                    DeselectItem();
                }
            }
        }
    }

    public void StartEditing()
    {
        if (paintEngine.movable)
        {
            distance = Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
            ratio = rectTransform.sizeDelta.x / distance;
        }
    }

    public void Edit()
    {
        distance = Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
        clamped = Mathf.Clamp((int)distance * ratio, 30, 512);
        rectTransform.sizeDelta = new Vector2(clamped, clamped);
        boxCollider2D.size = new Vector2(rectTransform.sizeDelta.x + 60, rectTransform.sizeDelta.y + 60);
        transform.localRotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * Quaternion.AngleAxis(135, Vector3.forward);
    }

    public void EditConfirm()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            transform.SetParent(basicTransform);
            questable.QuestConfirm();
        }
    }

    /// <summary>
    /// 오브젝트 삭제
    /// </summary>
    public void DelelteItem()
    {
        if(Statics.gameMode == GameMode.Free)
        {
            if (stickerType == StickerType.CharacterSticker)
            {
                freeable.CharacterStickerCounts[freeable.SelectFingerIndex] -= 1;
                freeable.CharacterStickerItems[freeable.SelectFingerIndex].stickerConverters.Remove(this);
            }
            else if (stickerType == StickerType.Sticker)
            {
                freeable.StickerCounts[freeable.SelectFingerIndex] -= 1;
                freeable.StickerItems[freeable.SelectFingerIndex].stickerConverters.Remove(this);
            }
            else if (stickerType == StickerType.AnimationSticker)
            {
                freeable.AnimationStickerCounts[freeable.SelectFingerIndex] -= 1;
                freeable.AnimationStickerItems[freeable.SelectFingerIndex].stickerConverters.Remove(this);
            }
            DeselectItem();
            Hide();
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            if (stickerType == StickerType.CharacterSticker)
            {
                questable.CharacterStickerItems[questable.SelectFingerIndex].stickerConverters.Remove(this);
            }
            else if (stickerType == StickerType.Sticker)
            {
                questable.StickerItems[questable.SelectFingerIndex].stickerConverters.Remove(this);
            }
            DeselectItem();
            Hide();
        }
    }

    public void SelectItem()
    {
        if (paintEngine.movable)
        {
            hasMoved = false;

            if (currentStickerConverter != null)
            {
                currentStickerConverter.DeselectItem();
            }

            currentStickerConverter = this;

            editTool.SetActive(true);

            if (paintEngine != null)
            {
                paintEngine.drawEnabled = false;
            }
            if (Statics.gameMode == GameMode.Free)
            {
                freeable.ChangeEidtParent(this);
            }
            else if (Statics.gameMode == GameMode.Quest)
            {
                questable.ChangeEidtParent(this);
            }
        }

        firstCheckPosition = Input.mousePosition;
    }

    /// <summary>
    /// 오브젝트 해제
    /// </summary>
    public void DeselectItem()
    {
        if (Statics.gameMode == GameMode.Free)
        {
            freeable.StickerItemDeselect(this);
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            questable.StickerItemDeselect(this);
        }
        transform.SetParent(basicTransform);
        editTool.SetActive(false);
        this.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 스티커 드래그할시 동작
    /// </summary>
    public void Reposition()
    {
        if (paintEngine.movable)
        {
            if (paintEngine.IsRaycastInsideMask(Input.mousePosition))
            {
                if (!hasMoved)
                {
                    hasMoved = true;
                }

                Vector3 movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(firstCheckPosition);
                movePosition *= -1;
                transform.position -= movePosition;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            }
            else
            {
                borderHit = false;
                posToCheck = Input.mousePosition;
                while (!borderHit)
                {
                    posToCheck = Vector3.MoveTowards(posToCheck, Camera.main.WorldToScreenPoint(paintEngine.transform.position), 1f);

                    if (paintEngine.IsRaycastInsideMask(posToCheck))
                    {
                        transform.position = Camera.main.ScreenToWorldPoint(posToCheck);
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
                        borderHit = true;
                    }
                }
            }
            firstCheckPosition = Input.mousePosition;
        }
    }

    public void EnterSticker()
    {
        if (stickerType == StickerType.AnimationSticker)
        {
            subAnimationStickerObject.SetActive(true);
            animationStickerAnimator.SetBool("isWalk", false);
        }
        else if (stickerType == StickerType.Sticker)
        {
            subStickerImage.gameObject.SetActive(true);
        }
        else if (stickerType == StickerType.CharacterSticker)
        {
            subStickerImage.gameObject.SetActive(true);
        }
    }

    public void ExitSticker()
    {
        if (stickerType == StickerType.AnimationSticker)
        {
            subAnimationStickerObject.SetActive(false);
            animationStickerAnimator.SetBool("isWalk",true);
        }
        else if (stickerType == StickerType.Sticker)
        {
            subStickerImage.gameObject.SetActive(false);
        }
        else if (stickerType == StickerType.CharacterSticker)
        {
            subStickerImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown()
    {
        SelectItem();
    }

    public void OnPointerDrag()
    {
        Reposition();
    }

    public void OnPointerUp()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            transform.SetParent(basicTransform);
            questable.QuestConfirm();
        }
    }

    public void OnOffCollider(bool isOn)
    {
        boxCollider2D.enabled = isOn;
    }

    public void Hide()
    {
        QuestConfirmBehaviour.OnChangeParentHandler -= ChangeParent;
        Destroy(animationStickerObject);
        Destroy(subAnimationStickerObject);
        stickerItem.Hide();
    }

    private void OnDestroy()
    {
        QuestConfirmBehaviour.OnChangeParentHandler -= ChangeParent;
    }
    public void ChangeParent()
    {
        questable.ChangeEidtParent(this);
    }
}