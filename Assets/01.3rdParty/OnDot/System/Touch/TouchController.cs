using OnDot.Util;
using UnityEngine;

public class TouchController : MonoSingleton<TouchController>
{
    public bool IsTouch
    {
        get
        {
            return !SystemUtil.IsPointerOverGameObjectWithLayerMask(overLayerMask);
        }
    }

    /// <summary>
    /// 드래그 가능 상태인지 여부
    /// </summary>
    public bool CanDrag
    {
        get { return IsTouch && !isZoom; }
    }

    /// <summary>
    /// 드래그 중인지 여부 (true인 경우만 작동, false로 변경 시 강제 종료)
    /// </summary>
    [SerializeField, ReadOnly] private bool isDrag = true;
    public bool IsDrag
    {
        get { return isDrag; }
        set { isDrag = value; }
    }

    /// <summary>
    /// 줌 가능 상태인지 여부
    /// </summary>
    public bool CanZoom
    {
        get { return IsTouch; }
    }

    /// <summary>
    /// 줌 중인지 여부 (true인 경우만 작동, false로 변경 시 강제 종료)
    /// </summary>
    [SerializeField, ReadOnly] private bool isZoom = true;
    public bool IsZoom
    {
        get { return isZoom; }
        set { isZoom = value; }
    }

    [SerializeField] private LayerMask overLayerMask;
}
