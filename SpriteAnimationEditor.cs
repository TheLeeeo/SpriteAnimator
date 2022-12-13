using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteAnimation))]
public class SpriteAnimationEditor : Editor
{
    SpriteAnimation animation;

    private double editorStartTime;

    private float fps;
    private double animationTimer;

    private Texture currentTexture;

    private SerializedProperty animationTime;
    private SerializedProperty animationType;
    private SerializedProperty animationFrameOnFinished;
    private SerializedProperty animationName;
    private SerializedProperty animationFrames;

    private void OnEnable()
    {
        animationTime = serializedObject.FindProperty("_animationTime");
        animationType = serializedObject.FindProperty("_animationType");
        animationName = serializedObject.FindProperty("_animationName");
        animationFrames = serializedObject.FindProperty("_frames");
        animationFrameOnFinished = serializedObject.FindProperty("_frameOnFinished");

        animation = target as SpriteAnimation;        

        editorStartTime = EditorApplication.timeSinceStartup;

        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
        }            

        EditorGUILayout.PropertyField(animationTime);
        EditorGUILayout.PropertyField(animationName);

        EditorGUILayout.PropertyField(animationType);

        if(animationType.enumValueIndex == ((int)AnimationType.PlayOnce))
        {
            EditorGUILayout.PropertyField(animationFrameOnFinished);
        }

        EditorGUILayout.PropertyField(animationFrames);

        serializedObject.ApplyModifiedProperties();

        if(null != currentTexture)
        {
            Rect position = GUILayoutUtility.GetRect(new GUIContent(currentTexture), GUIStyle.none);
            position.size = new Vector2(120, 120);

            GUI.DrawTexture(position, currentTexture);
        }        

        if (0 == animation.animaitonTime)
        {
            fps = 1;
        }
        else
        {
            fps = animation.frames.Length / animation.animaitonTime;
        }
        
    }

    private void EditorUpdate()
    {
        if(null == animation.frames || 0 == animation.frames.Length)
        {
            return;
        }

        animationTimer = (EditorApplication.timeSinceStartup - editorStartTime) * fps;

        Sprite sprite = animation.frames[(int)animationTimer % animation.frames.Length];

        if(null == sprite)
        {
            return;
        }

        currentTexture = sprite.ConvertToTexture();

        Repaint();
    }
}
