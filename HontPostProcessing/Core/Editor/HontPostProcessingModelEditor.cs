using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hont.PostProcessing
{
    public abstract class HontPostProcessingModelEditor<T> : HontPostProcessingModelEditorBase
        where T : HontPostProcessingModelBase
    {
        T mModel;
        protected T Model { get { return mModel; } }


        public override void Init(HontPostProcessingModelBase target)
        {
            base.Init(target);

            mModel = (T)mTarget;
        }
    }
}

