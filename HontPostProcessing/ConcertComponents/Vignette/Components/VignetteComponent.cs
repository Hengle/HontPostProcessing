using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class VignetteComponent : HontPostProcessingComponent<VignetteModel>
    {
        Material mVignetteMaterial;
        Material VignetteMaterial { get { return mVignetteMaterial ?? (mVignetteMaterial = new Material(Shader.Find("Hidden/VignetteShader"))); } }

        public override string Name { get { return "Vignette"; } }


        public override void OnRender()
        {
            VignetteMaterial.SetVector("_K", Model.k);

            var tempRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);
            Graphics.Blit(mContext.CurrentRenderRT, tempRT);
            Graphics.Blit(tempRT, mContext.CurrentRenderRT, VignetteMaterial);
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}
