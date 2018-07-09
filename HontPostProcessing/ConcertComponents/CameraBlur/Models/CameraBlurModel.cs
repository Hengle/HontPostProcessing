using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class CameraBlurModel : HontPostProcessingModelBase
    {
        public float blurRadius = 0.2f;
        public int sampleNum = 3;

        public override string Name { get { return "Blur"; } }
    }
}
