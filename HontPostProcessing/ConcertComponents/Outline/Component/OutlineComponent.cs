using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class OutlineComponent : HontPostProcessingComponent<OutlineModel>
    {
        int mCacheItemListCount;
        int mMaskRT_ID;
        int mTempRT_ID;
        Material mOutlineEffectMaterial;
        Material mReplaceMaterial;
        CommandBuffer mCommandBuffer;

        Material ReplaceMaterial { get { return mReplaceMaterial ?? (mReplaceMaterial = new Material(Shader.Find("Hidden/OutlineEffectMaskShader"))); } }
        Material OutlineEffectMaterial { get { return mOutlineEffectMaterial ?? (mOutlineEffectMaterial = new Material(Shader.Find("Hidden/OutlineEffectCoreShader"))); } }

        public override string Name { get { return "Outline"; } }


        public override void OnEnable()
        {
            base.OnEnable();

            mCacheItemListCount = -1;

            mCommandBuffer = new CommandBuffer();
            mCommandBuffer.name = "Outline";
            mContext.Camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, mCommandBuffer);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            mContext.Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, mCommandBuffer);
            mCommandBuffer.Dispose();
        }

        public override void OnPreRender()
        {
            base.OnPreRender();

            if (mCacheItemListCount != Model.ItemList.Count)
            {
                RebuildCommandBuffer();

                mCacheItemListCount = Model.ItemList.Count;
            }
        }

        public override void OnRender()
        {
        }

        void RebuildCommandBuffer()
        {
            mCommandBuffer.Clear();

            mMaskRT_ID = Shader.PropertyToID("OutlineEffect_MaskRT");
            mCommandBuffer.GetTemporaryRT(mMaskRT_ID, -1, -1);

            mTempRT_ID = Shader.PropertyToID("OutlineEffect_TempRT");
            mCommandBuffer.GetTemporaryRT(mTempRT_ID, -1, -1);

            mCommandBuffer.SetRenderTarget(mMaskRT_ID);
            mCommandBuffer.ClearRenderTarget(true, true, Color.black);

            for (int i = 0, iMax = Model.ItemList.Count; i < iMax; i++)
            {
                var item = Model.ItemList[i];

                var renderer = item.gameObject.GetComponentInChildren<Renderer>();
                mCommandBuffer.DrawRenderer(renderer, ReplaceMaterial);
            }

            mCommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);

            mCommandBuffer.SetGlobalTexture("_OutlineMaskTexture", mMaskRT_ID);

            mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mTempRT_ID);
            mCommandBuffer.Blit(mTempRT_ID, BuiltinRenderTextureType.CameraTarget, OutlineEffectMaterial);

            mCommandBuffer.ReleaseTemporaryRT(mMaskRT_ID);
            mCommandBuffer.ReleaseTemporaryRT(mTempRT_ID);
        }
    }
}
