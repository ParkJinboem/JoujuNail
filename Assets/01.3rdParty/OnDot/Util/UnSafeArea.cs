using System.Collections;
using UnityEngine;

namespace OnDot.Util
{
    public class UnSafeArea : MonoBehaviour
    {
        RectTransform Panel;
        Rect LastSafeArea = new Rect(0, 0, 0, 0);

        public bool useLeft = true;
        public bool useBottom = true;
        public bool useRight = true;
        public bool useTop = true;

        private bool isApply = false;

        void Awake()
        {
            Panel = GetComponent<RectTransform>();

            Refresh();
        }

        void Update()
        {
            if (isApply)
            {
                Refresh();
            }
        }

        void Refresh()
        {
            Rect safeArea = GetSafeArea();

            if (safeArea != LastSafeArea)
                ApplySafeArea(safeArea);
        }

        Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        void ApplySafeArea(Rect r)
        {
            StopAllCoroutines();
            StartCoroutine(IApplySafeArea(r));
        }

        IEnumerator IApplySafeArea(Rect r)
        {
            if (isApply)
            {
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            LastSafeArea = r;

            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;

            Vector2 offsetMin = anchorMin;
            Vector2 offsetMax = new Vector2(Screen.width - anchorMax.x, Screen.height - anchorMax.y);
            if (!useLeft) offsetMin.x = 0;
            if (!useBottom) offsetMin.y = 0;
            if (!useRight) offsetMax.x = 0;
            if (!useTop) offsetMax.y = 0;
            Panel.offsetMin = -offsetMin;
            Panel.offsetMax = offsetMax;

            //if (isApply)
            //{
            //    yield return YieldInstructionCache.WaitForEndOfFrame;
            //}

            //LastSafeArea = r;

            //Canvas c = GetComponentInParent<Canvas>();
            //RectTransform rtCanvas = null;
            //if (c != null)
            //{
            //    if (c.TryGetComponent(out RectTransform rt))
            //    {
            //        rtCanvas = rt;
            //    }
            //}

            //if (rtCanvas != null)
            //{
            //    float ratioX = rtCanvas.sizeDelta.x / Screen.width;
            //    float ratioY = rtCanvas.sizeDelta.y / Screen.height;

            //    Vector2 offsetMin = r.position;
            //    Vector2 offsetMax = r.position + r.size;
            //    offsetMin.x *= ratioX;
            //    offsetMin.y *= ratioY;
            //    offsetMax.x = (offsetMax.x - Screen.width) * ratioX;
            //    offsetMax.y = (offsetMax.y - Screen.height) * ratioY;
            //    if (!useLeft) offsetMin.x = 0;
            //    if (!useBottom) offsetMin.y = 0;
            //    if (!useRight) offsetMax.x = 0;
            //    if (!useTop) offsetMax.y = 0;
            //    Panel.offsetMin = -offsetMin;
            //    Panel.offsetMax = -offsetMax;
            //}

            isApply = true;

            //Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}", name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
        }

        //public void Resize()
        //{
        //    Vector2 offsetMin = r.position;
        //    Vector2 offsetMax = r.position + r.size;
        //}
    }
}