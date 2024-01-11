using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OnDot.Util
{
    public class LabelAttribute : PropertyAttribute
    {
        public string label { get; private set; }
        public LabelAttribute(string label)
        {
            this.label = label;
        }
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class RenameEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new GUIContent((attribute as LabelAttribute).label));
        }
    }
    #endif
}