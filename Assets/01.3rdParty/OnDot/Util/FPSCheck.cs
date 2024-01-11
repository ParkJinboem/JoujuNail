using System.Collections.Generic;
using UnityEngine;

namespace OnDot.Util
{
    public class FPSCheck : MonoBehaviour
    {
        float deltaTime = 0.0f;
        float horizontal, vertical, mouseX, mouseY;

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            //horizontal = Input.GetAxis("Horizontal");
            //vertical = Input.GetAxis("Vertical");
            //mouseX = Input.GetAxis("Mouse X");
            //mouseY = Input.GetAxis("Mouse Y");
        }

        void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(w - 200, 100, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            //text += string.Format("\n Horizontal: {0}, Vertical: {1}, Mouse X: {2}, Mouse Y: {3}", horizontal, vertical, mouseX, mouseY); 
            GUI.Label(rect, text, style);
        }
    }
}