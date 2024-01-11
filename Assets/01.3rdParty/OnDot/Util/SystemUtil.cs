using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OnDot.Util
{
    public class SystemUtil
    {
        /// <summary>
        /// UI외 터치 제한
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverGameObject
        {
            get
            {
                return false;

                // or (일부 미작동 확인)
                //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                //{
                //    return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                //}
                //else
                //{
                //    return EventSystem.current.IsPointerOverGameObject();
                //}

                // or (정상 작동 확인)
                //PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                //pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                //List<RaycastResult> results = new List<RaycastResult>();
                //EventSystem.current.RaycastAll(pointerEventData, results);
                //return results.Count > 0;
            }
        }

        /// <summary>
        /// 특정 레이어 터치 여부 확인
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static bool IsPointerOverGameObjectWithLayerMask(LayerMask layerMask)
        {
            bool isPointerOverGameObject = false;
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            for (int i = 0; i < results.Count; i++)
            {
                if (layerMask == (layerMask | (1 << results[i].gameObject.layer)))
                {
                    isPointerOverGameObject = true;
                    break;
                }
            }
            return isPointerOverGameObject;
        }

        /// <summary>
        /// 인터넷 연결 확인
        /// </summary>
        /// <returns></returns>
        public static bool InternetReachable
        {
            get { return UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable; }
        }
    }
}