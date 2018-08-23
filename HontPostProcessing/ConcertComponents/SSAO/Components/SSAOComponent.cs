using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class SSAOComponent : HontPostProcessingComponent<SSAOModel>
    {
        const int PASS0_SSAO = 0;
        const int PASS1_COMBINE = 1;

        int mSSAOTex_ID;
        int m_Params_ID;

        Material mSSAOMaterial;

        Material SSAOMaterial { get { return mSSAOMaterial ?? (mSSAOMaterial = new Material(Shader.Find("Hidden/SSAO"))); } }

        public override string Name { get { return "SSAO"; } }


        public override void OnEnable()
        {
            base.OnEnable();

            mSSAOTex_ID = Shader.PropertyToID("_SSAO");
            m_Params_ID = Shader.PropertyToID("_Params");

            mContext.Camera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        public override void OnRender()
        {
            var aoRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.width / Model.downsampling, mContext.CurrentRenderRT.height / Model.downsampling, 0);
            SSAOMaterial.SetVector(m_Params_ID, new Vector4(
                                                     Model.radius,
                                                     Model.minZ,
                                                     0,
                                                     Model.occlusionIntensity));

            Graphics.Blit(mContext.CurrentRenderRT, aoRT, SSAOMaterial, PASS0_SSAO);

            var tempRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);
            Graphics.Blit(mContext.CurrentRenderRT, tempRT);
            SSAOMaterial.SetTexture(mSSAOTex_ID, aoRT);
            Graphics.Blit(tempRT, mContext.CurrentRenderRT, SSAOMaterial, PASS1_COMBINE);

            RenderTexture.ReleaseTemporary(aoRT);
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}
