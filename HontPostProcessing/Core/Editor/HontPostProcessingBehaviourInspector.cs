using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing
{
    [CustomEditor(typeof(HontPostProcessingBehaviour))]
    public class HontPostProcessingBehaviourInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            var seri = serializedObject.FindProperty("mProfile");
            EditorGUILayout.ObjectField(seri, new GUIContent("Profile"));
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
