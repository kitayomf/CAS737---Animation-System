using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BlendPoint<float>))]
public class LinearBlendPointPropertyDrawer : PropertyDrawer
{
    private const float GAP = 4f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pointSerializedProperty = property.FindPropertyRelative("_position");
        var animationClipSerializedProperty = property.FindPropertyRelative("_animationClip");

        EditorGUI.BeginProperty(position, label, property);

        var pointWidth = (position.width - GAP / 2) / 4;
        var animationClipWidth = position.width - pointWidth;

        var pointRect = new Rect(position.x, position.y, pointWidth,
            EditorGUI.GetPropertyHeight(pointSerializedProperty));
        EditorGUI.PropertyField(pointRect, pointSerializedProperty, GUIContent.none, true);

        var animationClipRect = new Rect(position.x + pointWidth + GAP, position.y, animationClipWidth,
            EditorGUI.GetPropertyHeight(animationClipSerializedProperty));
        EditorGUI.PropertyField(animationClipRect, animationClipSerializedProperty, GUIContent.none, true);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(BlendPoint<Vector2>))]
public class PlanarBlendPointPropertyDrawer : PropertyDrawer
{
    private const float GAP = 4f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pointSerializedProperty = property.FindPropertyRelative("_position");
        var animationClipSerializedProperty = property.FindPropertyRelative("_animationClip");

        EditorGUI.BeginProperty(position, label, property);

        var fieldWidth = (position.width - GAP / 2) / 2;

        var pointRect = new Rect(position.x, position.y, fieldWidth,
            EditorGUI.GetPropertyHeight(pointSerializedProperty));
        EditorGUI.PropertyField(pointRect, pointSerializedProperty, GUIContent.none, true);

        var animationClipRect = new Rect(position.x + fieldWidth + GAP, position.y, fieldWidth,
            EditorGUI.GetPropertyHeight(animationClipSerializedProperty));
        EditorGUI.PropertyField(animationClipRect, animationClipSerializedProperty, GUIContent.none, true);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(LinearBlendMap))]
public class LinearBlendMapPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var pointListSerializedProperty = property.FindPropertyRelative("_pointList");
        return EditorGUI.GetPropertyHeight(pointListSerializedProperty);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pointListSerializedProperty = property.FindPropertyRelative("_pointList");
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, pointListSerializedProperty, label, true);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(PlanarBlendMap))]
public class PlanarBlendMapPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var pointListSerializedProperty = property.FindPropertyRelative("_pointList");
        return EditorGUI.GetPropertyHeight(pointListSerializedProperty);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pointListSerializedProperty = property.FindPropertyRelative("_pointList");
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, pointListSerializedProperty, label, true);

        EditorGUI.EndProperty();
    }
}
