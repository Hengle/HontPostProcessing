using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing
{
    public class HontPostProcessingBehaviour : MonoBehaviour
    {
        [SerializeField]
        HontPostProcessingProfile mProfile;

        public HontPostProcessingProfile Profile { get { return mProfile; } }


        public void ReplaceProfile(HontPostProcessingProfile profile)
        {
            mProfile = profile;
            mProfile.Init(Camera.main);
            mProfile.Enable();
        }

        void OnEnable()
        {
            mProfile.Init(Camera.main);

            mProfile.Enable();
        }

        void OnDisable()
        {
            mProfile.Disable();
        }

        private void OnPreRender()
        {
            mProfile.PreRender();
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            mProfile.Render(source, destination);
        }
    }
}
