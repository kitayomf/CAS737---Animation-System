using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationPlayer))]
[CanEditMultipleObjects]
public class AnimationPlayerCustomEditor : Editor
{
    private SerializedProperty animator;
    private SerializedProperty applyRootMotion;
    private SerializedProperty additionalLayers;

    void OnEnable()
    {
        animator = serializedObject.FindProperty("animator");
        applyRootMotion = serializedObject.FindProperty("applyRootMotion");
        additionalLayers = serializedObject.FindProperty("additionalLayers");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(animator);

        if (animator.objectReferenceValue == null)
        {
            EditorGUILayout.PropertyField(applyRootMotion);
        }

        EditorGUILayout.PropertyField(additionalLayers);

        serializedObject.ApplyModifiedProperties();
    }

}