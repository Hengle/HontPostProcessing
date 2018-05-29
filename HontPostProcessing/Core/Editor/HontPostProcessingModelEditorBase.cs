using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hont.PostProcessing
{
    public abstract class HontPostProcessingModelEditorBase
    {
        protected HontPostProcessingModelBase mTarget;


        public virtual void Init(HontPostProcessingModelBase target)
        {
            mTarget = target;
        }

        public virtual void OnGUI()
        {
            var headerIsClicked = false;
            var enableButtonClicked = false;
            HontPostProcessingGUIHelper.Header(mTarget.Name, mTarget.Enabled, out headerIsClicked, out enableButtonClicked);
            if (headerIsClicked) mTarget.Unfold = !mTarget.Unfold;
            if (enableButtonClicked) mTarget.Enabled = !mTarget.Enabled;

            if(mTarget.Unfold)
            {
                EditorGUI.indentLevel++;

                using (new EditorGUI.DisabledGroupScope(!mTarget.Enabled))
                {
                    OnInspectorGUI();
                }

                EditorGUI.indentLevel--;
            }
        }

        protected abstract void OnInspectorGUI();
    }
}

