using UnityEngine;

namespace OnDot.Util
{
    public class SpriteStretch : MonoBehaviour
    {
        private bool isApply = false;
        private float lastWidth, lastHeight;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();

            Refresh();
        }

        private void Update()
        {
            if (isApply)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            if (lastWidth != Screen.width ||
                lastHeight != Screen.height)
            {
                Apply();
            }
        }

        private void Apply()
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            float cameraHeight = Camera.main.orthographicSize * 2;
            Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
            Vector2 spriteSize = sr.sprite.bounds.size;
            Vector3 scale = Vector3.one;

            float ratioX = cameraSize.x / spriteSize.x;
            float ratioY = cameraSize.y / spriteSize.y;
            if (ratioX > ratioY)
            {
                scale.x = scale.y = ratioX;
            }
            else
            {
                scale.x = scale.y = ratioY;
            }
            transform.localScale = scale;
            

            isApply = true;
        }
    }
}