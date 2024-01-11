using System;
using UnityEngine;

namespace OnDot.Util
{
    public class TextUtil
    {
        public static void CopyToClipboard(string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }
    }
}