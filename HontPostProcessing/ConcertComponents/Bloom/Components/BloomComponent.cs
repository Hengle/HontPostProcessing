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

        RenderTexture mCacheRT;
        RenderTexture mBloomBlur1RT;
        RenderTexture mBloomBlur2RT;
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
            if (mCacheRT == null)
            {
                mCacheRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);
            }

            if (mBloomBlur1RT == null)
            {
                var descriptor = mContext.CurrentRenderRT.descriptor;
                mBloomBlur1RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, 0, descriptor.colorFormat);
            }

            if (mBloomBlur2RT == null)
            {
                var descriptor = mContext.CurrentRenderRT.descriptor;
                mBloomBlur2RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, 0, descriptor.colorFormat);
            }

            BloomMaterial.SetFloat(mStreak_Length_ID, Model.streak_Length);
            BloomMaterial.SetFloat(mThreshold_ID, Model.threshold);

            Graphics.Blit(mContext.CurrentRenderRT, mCacheRT, BloomMaterial, PASS2_EXTRACTHDR);

            Graphics.Blit(mCacheRT, mBloomBlur1RT, BloomMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur1RT, mBloomBlur2RT, BloomMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur2RT, mBloomBlur1RT, BloomMaterial, PASS1_XBLUR);

            BloomMaterial.SetTexture(mBloomTex_ID, mBloomBlur1RT);

            Graphics.Blit(mContext.CurrentRenderRT, mCacheRT);
            Graphics.Blit(mCacheRT, mContext.CurrentRenderRT, BloomMaterial, PASS0_BASS);
        }
    }
}
