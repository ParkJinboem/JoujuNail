using System;
using UnityEngine;

namespace OnDot.System.Touch
{
    public abstract class TouchDragMoveBehaviour : MonoBehaviour
    {
        public abstract void DragMove(Vector3 mousePosition);

        private Vector3 startPosition;

        private void OnEnable()
        {
            TouchDragBehaviour.TouchDragDownEvent += HandlerTouchDragDown;
            TouchDragBehaviour.TouchDragMoveEvent += HandlerTouchDragMove;
        }

        private void OnDisable()
        {
            TouchDragBehaviour.TouchDragDownEvent -= HandlerTouchDragDown;
            TouchDragBehaviour.TouchDragMoveEvent -= HandlerTouchDragMove;
        }

        private void HandlerTouchDragDown()
        {
            startPosition = transform.position;
        }

        private void HandlerTouchDragMove(Vector3 movePosition)
        {
            DragMove(startPosition + movePosition);
        }
    }
}