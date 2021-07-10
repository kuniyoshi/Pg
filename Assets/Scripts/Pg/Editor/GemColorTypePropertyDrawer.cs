#nullable enable
using System.Linq;
using Pg.Data;
using Pg.Scene.Game;
using UnityEditor;
using UnityEngine;

namespace Pg.Editor
{
    [CustomPropertyDrawer(typeof(SerializableGemColorType))]
    public class GemColorTypePropertyDrawer
        : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var target = property.FindPropertyRelative("Id");
            var newValue = EditorGUI.Popup(
                position,
                target.displayName,
                GemColorType.Values.ToList().FindIndex(value => value.Id == target.intValue),
                GemColorType.Values.Select(v => v.ToString()).ToArray()
            );
            newValue = newValue < 0 ? 0 : newValue;
            var intValue = GemColorType.Values.ElementAt(newValue).Id;
            target.intValue = intValue; // :( when assign new value, then inspector quit drawing somehow
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();

            EditorGUI.EndProperty();
        }
    }
}
