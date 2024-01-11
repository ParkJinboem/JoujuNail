using OnDot.System;
using UnityEngine;

public class BgClicked : MonoBehaviour
{
    [SerializeField] private Transform scale;
    [SerializeField] private Transform tr;

    private SpriteRenderer spriteRenderer;

    private FreeAnimController freeAnimController;
    public FreeAnimController FreeAnimController
    {
        set { freeAnimController = value; }
    }
    public Animator bgAnimator;

    public Sprite baseSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        spriteRenderer.sprite = baseSprite;
    }

    /// <summary>
    /// 손,발 클릭시 애니메이션 실행
    /// </summary>
    public void OnMouseDown()
    {
        //로딩 완료 전까지는 실행 못하게함
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("FreePaintScene01");
        string name = gameObject.name;
        if (name == "HandBg")
        {
            DotweenManager.Instance.DoLocalMoveX(((spriteRenderer.sprite.bounds.size.x / 2) * scale.localScale.x) + 1f, 1f, tr);
        }
        else if (name == "FootBg")
        {
            DotweenManager.Instance.DoLocalMoveX(-((spriteRenderer.sprite.bounds.size.x / 2) * scale.localScale.x) - 1f, 1f, tr);
        }
        freeAnimController.ClickBg(name, tr.gameObject.transform.parent.gameObject);
    }
}
