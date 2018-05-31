using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    public abstract class HontPostProcessingComponentBase
    {
        HontPostProcessingModelBase mModel;
        protected HontPostProcessingContext mContext;
        public virtual bool IsSupported { get { return true; } }

        public abstract string Name { get; }


        public virtual void Init(HontPostProcessingContext context, HontPostProcessingModelBase model)
        {
            mContext = context;
            mModel = model;
        }

        public HontPostProcessingModelBase GetModel() { return mModel; }

        public virtual void OnPreRender() { }
        public abstract void OnRender();

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}
