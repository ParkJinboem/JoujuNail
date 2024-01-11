using UnityEngine;

namespace OnDot.System.Touch
{
    public class TouchDragBehaviour : MonoBehaviour
    {
        public delegate void TouchDragDownHandler();
        public static event TouchDragDownHandler TouchDragDownEvent;

        public delegate void TouchDragMoveHandler(Vector3 movePosition);
        public static event TouchDragMoveHandler TouchDragMoveEvent;

        public TouchController touchController;

        private Vector3 downPosition;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DragStart();
            }

            if (Input.GetMouseButton(0))
            {
                DragMove();
            }

            if (Input.GetMouseButtonUp(0))
            {
                DragEnd();
            }
        }

        private void DragStart()
        {
            if (!touchController.CanDrag)
            {
                DragEnd();
                return;
            }

            touchController.IsDrag = true;

            downPosition = Input.mousePosition;
            TouchDragDownEvent?.Invoke();
        }

        private void DragMove()
        {
            if (!touchController.IsDrag)
            {
                DragEnd();
                return;
            }

            Vector3 movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(downPosition);
            movePosition *= -1f;
            TouchDragMoveEvent?.Invoke(movePosition);
        }

        private void DragEnd()
        {
            touchController.IsDrag = false;
        }
    }
}