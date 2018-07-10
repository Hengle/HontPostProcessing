using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class Bloom_StarGlowComponent : HontPostProcessingComponent<Bloom_StarGlowModel>
    {
        const int PASS0_BASS = 0;
        const int PASS1_XBLUR = 1;
        const int PASS2_EXTRACTHDR = 2;

        int mBloomTex_ID;
        int mStreak_Length_ID;
        int mMode_ID;

        RenderTexture mCacheRT;
        RenderTexture mBloomBlur1RT;
        RenderTexture mBloomBlur2RT;
        Material mStarGlowMaterial;

        Material StarGlowMaterial { get { return mStarGlowMaterial ?? (mStarGlowMaterial = new Material(Shader.Find("Hidden/Bloom_StarGlowShader"))); } }

        public override string Name { get { return "Bloom_StarGlow"; } }


        public override void OnEnable()
        {
            base.OnEnable();

            mStreak_Length_ID = Shader.PropertyToID("_Streak_Length");
            mBloomTex_ID = Shader.PropertyToID("_BloomTex");
            mMode_ID = Shader.PropertyToID("_Mode");

            StarGlowMaterial.SetFloat("_StarGlowTexOverlayAlpha", 0.5f);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (mCacheRT != null)
                RenderTexture.ReleaseTemporary(mCacheRT);

            if (mBloomBlur1RT != null)
                RenderTexture.ReleaseTemporary(mBloomBlur1RT);

            if (mBloomBlur2RT != null)
                RenderTexture.ReleaseTemporary(mBloomBlur2RT);
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
                mBloomBlur1RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, descriptor.depthBufferBits, descriptor.colorFormat);
            }

            if (mBloomBlur2RT == null)
            {
                var descriptor = mContext.CurrentRenderRT.descriptor;
                mBloomBlur2RT = RenderTexture.GetTemporary(descriptor.width >> 1, descriptor.height >> 1, descriptor.depthBufferBits, descriptor.colorFormat);
            }

            StarGlowMaterial.SetFloat(mStreak_Length_ID, Model.streak_Length);

            //-----------------------------------------------------------Mode0.
            StarGlowMaterial.SetFloat(mMode_ID, 0);

            Graphics.Blit(mContext.CurrentRenderRT, mCacheRT, StarGlowMaterial, PASS2_EXTRACTHDR);

            Graphics.Blit(mCacheRT, mBloomBlur1RT, StarGlowMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur1RT, mBloomBlur2RT, StarGlowMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur2RT, mBloomBlur1RT, StarGlowMaterial, PASS1_XBLUR);

            StarGlowMaterial.SetTexture(mBloomTex_ID, mBloomBlur1RT);
            Graphics.Blit(mContext.CurrentRenderRT, mBloomBlur2RT, StarGlowMaterial, PASS0_BASS);
            Graphics.Blit(mBloomBlur2RT, mContext.CurrentRenderRT);
            //-------------------------------------------------------------------.


            //-----------------------------------------------------------Mode1.
            StarGlowMaterial.SetFloat(mMode_ID, 1);

            Graphics.Blit(mCacheRT, mBloomBlur1RT, StarGlowMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur1RT, mBloomBlur2RT, StarGlowMaterial, PASS1_XBLUR);
            Graphics.Blit(mBloomBlur2RT, mBloomBlur1RT, StarGlowMaterial, PASS1_XBLUR);

            StarGlowMaterial.SetTexture(mBloomTex_ID, mBloomBlur1RT);
            Graphics.Blit(mContext.CurrentRenderRT, mBloomBlur2RT, StarGlowMaterial, PASS0_BASS);
            Graphics.Blit(mBloomBlur2RT, mContext.CurrentRenderRT);
            //-------------------------------------------------------------------.
        }
    }
}
