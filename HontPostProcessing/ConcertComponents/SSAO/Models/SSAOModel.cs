using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class SSAOModel : HontPostProcessingModelBase
    {
        public float radius = 0.08f;
        public float occlusionIntensity = 0.45f;
        public int downsampling = 7;
        public float minZ = 0.83f;

        public override string Name { get { return "SSAO"; } }
    }
}
