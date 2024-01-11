using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorStickerConverter : MonoBehaviour
{
    private Vector3 firstCheckPosition;

    public void OnPointerDown()
    {
        firstCheckPosition = Input.mousePosition;
    }

    public void OnPointerDrag()
    {
        Reposition();
    }

    public void OnPointerUp()
    {
    }


    public void Reposition()
    {
        Vector3 movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(firstCheckPosition);
        movePosition *= -1;
        transform.position -= movePosition;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        firstCheckPosition = Input.mousePosition;

        //if (IsRaycastInsideMask(Input.mousePosition))
        //{
        //    if (!hasMoved)
        //    {
        //        hasMoved = true;
        //    }

        //    Vector3 movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(firstCheckPosition);
        //    movePosition *= -1;
        //    transform.position -= movePosition;
        //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        //}
        //else
        //{
        //    borderHit = false;
        //    posToCheck = Input.mousePosition;
        //    while (!borderHit)
        //    {
        //        posToCheck = Vector3.MoveTowards(posToCheck, Camera.main.WorldToScreenPoint(paintEngine.transform.position), 1f);

        //        if (paintEngine.IsRaycastInsideMask(posToCheck))
        //        {
        //            transform.position = Camera.main.ScreenToWorldPoint(posToCheck);
        //            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        //            borderHit = true;
        //        }
        //    }
        //}
        //firstCheckPosition = Input.mousePosition;
    }


    //private bool IsRaycastInsideMask(Vector3 screenPosition)
    //{
    //    if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out hit, Mathf.Infinity, paintLayerMask))
    //    {
    //        return false;
    //    }
    //    if (hit.collider != gameObject.GetComponentInChildren<Collider>())
    //    {
    //        return false;
    //    }

    //    Vector2 pixelUV1 = hit.textureCoord;
    //    pixelUV1.x *= texWidth;
    //    pixelUV1.y *= texHeight;
    //    int startX1 = (int)pixelUV1.x;
    //    int startY1 = (int)pixelUV1.y;
    //    int pixel1 = (texWidth * startY1 + startX1) * 4;

    //    if ((pixel1 < 0 || pixel1 >= pixels.Length))
    //    {
    //        return false;
    //    }
    //    else if (lockMaskPixels[pixel1] == 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
