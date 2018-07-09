using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class ExampleComponent : HontPostProcessingComponent<ExampleModel>
    {
        public override string Name { get { return "Example"; } }


        public override void OnRender()
        {
        }
    }
}
