using UnityEngine;
using UnityEngine.UI;

public class GuideStickerConverter : MonoBehaviour
{
    [SerializeField] private GuideStickerItem guideStickerItem;
    public Animator animator;
    public bool anim;

    private Image image;
    public Image Image
    {
        get { return image; }
    }
    private RectTransform rectTransform;
    public RectTransform RectTransform
    {
        get { return rectTransform; }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 초기값 셋팅
    /// </summary>
    /// <param name="questStickerData"></param>
    public void Init(QuestStickerData questStickerData)
    {
        rectTransform.localPosition = new Vector3(questStickerData.stickerPosX, questStickerData.stickerPosY, questStickerData.stickerPosZ);
        rectTransform.localRotation = Quaternion.Euler(questStickerData.stickerRotX, questStickerData.stickerRotY, questStickerData.stickerRotZ);
        rectTransform.sizeDelta = new Vector2(questStickerData.stickerWidth, questStickerData.stickerHeight);
        image.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
        
        if(questStickerData.stickerType == StickerType.AnimationSticker)
        {
            anim = true;
            animator.SetBool("isWalk", false);
        }
        else
        {
            anim = false;
        }
        image.color = new Color(1, 1, 1, 0.4f);
        image.raycastTarget = false;
    }

    public void SetAnimator()
    {
        animator.runtimeAnimatorController = this.animator.runtimeAnimatorController;
        StartAnim();
    }

    public void StartAnim()
    {
        animator.SetBool("isWalk", true);
    }

    public void Hide()
    {
        guideStickerItem.Hide();
    }
}
