using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    public class Test1Component : HontPostProcessingComponent<Test1Model>
    {
        public override string Name { get { return "Test1"; } }


        public override void OnRender()
        {
            Debug.Log("model: " + Model.a);
        }
    }
}
