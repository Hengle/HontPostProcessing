using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    [Serializable]
    public abstract class HontPostProcessingModelBase
    {
        bool mEnabled;
        bool mIsUnfold;

        public abstract string Name { get; }
        public virtual bool Enabled { get { return mEnabled; } set { mEnabled = value; } }
        public virtual bool Unfold { get { return mIsUnfold; } set { mIsUnfold = value; } }
    }
}
