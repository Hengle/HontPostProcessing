using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class Bloom_StarGlowModel : HontPostProcessingModelBase
    {
        public float streak_Length;

        public override string Name { get { return "Bloom_StarGlow"; } }
    }
}
