using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    public class HontPostProcessingBehaviour : MonoBehaviour
    {
        public HontPostProcessingProfile profile;


        void OnEnable()
        {
            profile.Init(Camera.main);

            profile.Enable();
        }

        void OnDisable()
        {
            profile.Disable();
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            profile.Render(source, destination);
        }
    }
}
