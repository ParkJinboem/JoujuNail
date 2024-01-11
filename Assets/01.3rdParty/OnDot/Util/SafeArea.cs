using System.Collections;
using UnityEngine;

namespace OnDot.Util
{
    public class SafeArea : MonoBehaviour
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

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            if (!useLeft) anchorMin.x = 0;
            if (!useBottom) anchorMin.y = 0;
            if (!useRight) anchorMax.x = 1;
            if (!useTop) anchorMax.y = 1;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = anchorMax;

            //UnSafeArea[] unSafeAreas = GetComponentsInChildren<UnSafeArea>();
            //for (int i = 0; i < unSafeAreas.Length; i++)
            //{
            //    unSafeAreas[i].
            //}

            isApply = true;

            //Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}", name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
        }
    }
}