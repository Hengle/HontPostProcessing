using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont
{
    public class HontPostProcessingContext
    {
        public Camera Camera { get; set; }
        public RenderTexture CurrentRenderRT { get; set; }
    }
}
