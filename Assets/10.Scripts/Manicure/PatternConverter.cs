using UnityEngine;
using UnityEngine.UI;

public class PatternConverter : MonoBehaviour
{
    [SerializeField] private PatternItem patternItem;

    private Image image;
    public Image Image
    {
        get { return image; }
    }

    private float patternDefalutSize = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 초기값 셋팅 
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="patternColor"></param>
    /// <param name="scale"></param>
    public void Init(Sprite sprite, Color patternColor, Vector3 scale)
    {
        image.sprite = sprite;
        if (Statics.gameMode == GameMode.Free)
        {
            IFreeable freeable = PaintSceneManager.Instance.Freeables[0];
            //image.rectTransform.sizeDelta = new Vector2(1024, 1024);
            image.rectTransform.sizeDelta = new Vector2(512, 512);
            transform.localPosition = Vector3.zero;
            patternDefalutSize = (freeable.PatternSizes[freeable.SelectFingerIndex] / 2) + 0.76f;
            transform.localScale = new Vector3(patternDefalutSize, patternDefalutSize, patternDefalutSize);

            //image.SetNativeSize();
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            transform.localScale = scale;

            if (Statics.selectType == SelectType.Hand)
            {
                //앨범 사이즈와 맞추기 위해 512로 수정
                //image.rectTransform.sizeDelta = new Vector2(1024, 1024);
                image.rectTransform.sizeDelta = new Vector2(512, 512);
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2(512, 512);
                //image.rectTransform.sizeDelta = new Vector2(1024, 1024);
            }
        }

        image.color = patternColor;

        if (Statics.gameMode == GameMode.Quest)
        {
            IQuestable questable = QuestSceneManager.Instance.Questables[0];
            questable.QuestConfirm();
        }
    }

    public void Hide()
    {
        patternItem.Hide();
    }
}