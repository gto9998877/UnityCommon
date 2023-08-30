using System;
using UnityEngine;
using UnityEditor;

// namespace NEON.AttributeScript
// {

    /// <summary>
    /// 在编辑器Inspector显示说明文字（如有多行，每行之间用\n分开）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CommentTextAttribute : PropertyAttribute
    {
        public string textContent;
        public string[] lines;
        
        
        public CommentTextAttribute(string text)
        {
            textContent = text;
            lines = textContent.Split('\n');
        }
    }
// }


// namespace NEON.Editor.XNode
// {
    [CustomPropertyDrawer(typeof(CommentTextAttribute))]
    public class CommentTextDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!(attribute is CommentTextAttribute commentTextAttribute)) return;
            
            var content = commentTextAttribute.lines;
            rect.height /= content.Length;
            for (var i = 0; i < content.Length; ++i)
            {
                EditorGUI.LabelField(rect, content[i], "");
                rect.position = new Vector2(rect.position.x,rect.position.y+rect.height);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if (attribute is CommentTextAttribute commentTextAttribute)
            {
                return height * commentTextAttribute.lines.Length;
            }

            return height;
        }
    }
// }