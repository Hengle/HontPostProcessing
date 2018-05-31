using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing
{
    [CustomEditor(typeof(HontPostProcessingProfile))]
    public class HontPostProcessingProfilerInspector : Editor
    {
        bool? mIsInitialized;
        List<HontPostProcessingModelEditorBase> mModelEditorList;

        HontPostProcessingProfile mConcertTarget;
        HontPostProcessingProfile ConcertTarget { get { mConcertTarget = mConcertTarget ?? base.target as HontPostProcessingProfile; return mConcertTarget; } }


        public override void OnInspectorGUI()
        {
            if (!mIsInitialized == null)
            {
                Init();
            }

            EditorGUI.BeginChangeCheck();
            for (int i = 0, iMax = mModelEditorList.Count; i < iMax; i++)
            {
                var modelEditor = mModelEditorList[i];

                modelEditor.OnGUI();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        void Init()
        {
            //Debug.
            if (ConcertTarget.ModelList.Count == 0)
                ConcertTarget.ModelList.AddRange(HontPostProcessingUtility.InstancesFromBaseClass<HontPostProcessingModelBase>());

            mModelEditorList = new List<HontPostProcessingModelEditorBase>();
            var allModelEditorList = HontPostProcessingUtility.InstancesFromBaseClass<HontPostProcessingModelEditorBase>();

            for (int i = 0, iMax = ConcertTarget.ModelList.Count; i < iMax; i++)
            {
                var model = ConcertTarget.ModelList[i];

                var targetModelEditor = allModelEditorList.Find(m => IsAttributeMatchedModel(model, m.GetType().GetCustomAttributes(false)));

                if (targetModelEditor != null)
                {
                    targetModelEditor.Init(model);
                    mModelEditorList.Add(targetModelEditor);
                }
            }

            mIsInitialized = true;
        }

        bool IsAttributeMatchedModel(HontPostProcessingModelBase model, object[] attributes)
        {
            for (int i = 0, iMax = attributes.Length; i < iMax; i++)
            {
                var item = attributes[i];

                if (item is HontPostProcessingIgnoreAttribute) return false;

                var modelEditorAttr = item as HontPostProcessingModelInspectorAttribute;

                if (modelEditorAttr != null && modelEditorAttr.type == model.GetType())
                    return true;
            }

            return false;
        }
    }
}
