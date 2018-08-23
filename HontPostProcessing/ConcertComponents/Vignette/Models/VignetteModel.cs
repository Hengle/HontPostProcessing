using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class VignetteModel : HontPostProcessingModelBase
    {
        public Vector4 k = new Vector4(0.02f, 1.47f, 3.27f, 0f);

        public override string Name { get { return "Vignette"; } }
    }
}
