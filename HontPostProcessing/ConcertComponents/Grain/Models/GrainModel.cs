using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class GrainModel : HontPostProcessingModelBase
    {
        public int noiseSize = 8;
        public float noiseRate = 0.5f;
        public float intensity = 1.0f;
        public float tile = 1.0f;

        public override string Name { get { return "Grain"; } }
    }
}
