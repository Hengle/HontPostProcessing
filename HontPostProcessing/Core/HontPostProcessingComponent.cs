using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    public abstract class HontPostProcessingComponent<T> : HontPostProcessingComponentBase
        where T : HontPostProcessingModelBase
    {
        T mModel;
        protected T Model { get { return mModel; } }


        public override void Init(HontPostProcessingContext context, HontPostProcessingModelBase model)
        {
            base.Init(context, model);

            mModel = (T)model;
        }
    }
}
