using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class OutlineModel : HontPostProcessingModelBase
    {
        public Color outlineColor = new Color(1, 0, 0, 0.5f);
        public float outlineWidth = 1f;
        readonly List<OutlineEffectItem> mItemsList = new List<OutlineEffectItem>();

        public override string Name { get { return "Outline"; } }

        [XmlIgnore]
        public List<OutlineEffectItem> ItemList { get { return mItemsList; } }
    }
}
