using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class ColorCorrectionModel : HontPostProcessingModelBase
    {
        public AnimationCurve redCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public AnimationCurve greenCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public AnimationCurve blueCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public float intensity = 1.0f;

        public override string Name { get { return "ColorCorrection"; } }
    }
}
