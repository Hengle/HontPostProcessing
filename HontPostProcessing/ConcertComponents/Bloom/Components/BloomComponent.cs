using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class BloomComponent : HontPostProcessingComponent<BloomModel>
    {
        const int PASS0_BASS = 0;
        const int PASS1_XBLUR = 1;
        const int PASS2_EXTRACTHDR = 2;

        int mBloomTex_ID;
        int mStreak_Length_ID;
        int mThreshold_ID;

        Material mBloomMaterial;

        Material BloomMaterial { get { return mBloomMaterial ?? (mBloomMaterial = new Material(Shader.Find("Hidden/BloomShader"))); } }

        public override string Name { get { return "Bloom"; } }


        public override void OnEnable()
        {
            base.OnEnable();

            mStreak_Length_ID = Shader.PropertyToID("_Streak_Length");
            mBloomTex_ID = Shader.PropertyToID("_BloomTex");
            mThreshold_ID = Shader.PropertyToID("_Threshold");
        }

        public override void OnRender()
        {
            var cacheRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);

            var descriptor = mContext.CurrentRenderRT.descriptor;
            var bloomBlur1RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, 0, RenderTextureFormat.ARGBHalf);//1/2

            descriptor = mContext.CurrentRenderRT.descriptor;
            var bloomBlur2RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, 0, RenderTextureFormat.ARGBHalf);//1/2

            BloomMaterial.SetFloat(mStreak_Length_ID, Model.streak_Length);
            BloomMaterial.SetFloat(mThreshold_ID, Model.threshold);

            Graphics.Blit(mContext.CurrentRenderRT, cacheRT, BloomMaterial, PASS2_EXTRACTHDR);

            Graphics.Blit(cacheRT, bloomBlur1RT, BloomMaterial, PASS1_XBLUR);
            Graphics.Blit(bloomBlur1RT, bloomBlur2RT, BloomMaterial, PASS1_XBLUR);
            Graphics.Blit(bloomBlur2RT, bloomBlur1RT, BloomMaterial, PASS1_XBLUR);

            BloomMaterial.SetTexture(mBloomTex_ID, bloomBlur1RT);

            Graphics.Blit(mContext.CurrentRenderRT, cacheRT);
            Graphics.Blit(cacheRT, mContext.CurrentRenderRT, BloomMaterial, PASS0_BASS);

            RenderTexture.ReleaseTemporary(cacheRT);
            RenderTexture.ReleaseTemporary(bloomBlur1RT);
            RenderTexture.ReleaseTemporary(bloomBlur2RT);
        }
    }
}
