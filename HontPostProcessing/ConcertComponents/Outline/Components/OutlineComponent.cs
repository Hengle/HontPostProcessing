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
        }

        public override void OnDisable()
        {
            base.OnDisable();

            mCommandBuffer.ReleaseTemporaryRT(mMaskRT_ID);
            mCommandBuffer.ReleaseTemporaryRT(mTempRT_ID);
            mContext.Camera.RemoveCommandBuffer(CameraEvent.AfterFinalPass, mCommandBuffer);
            mCommandBuffer.Dispose();
        }

        public override void OnPreRender()
        {
            base.OnPreRender();

            if (mCacheItemListCount != Model.ItemList.Count)
            {
                UpdateCommandBuffer();

                mCacheItemListCount = Model.ItemList.Count;
            }
        }

        public override void OnRender()
        {
        }

        void UpdateCommandBuffer()
        {
            if (mCommandBuffer != null)
            {
                mCommandBuffer.ReleaseTemporaryRT(mMaskRT_ID);
                mCommandBuffer.ReleaseTemporaryRT(mTempRT_ID);
                mContext.Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, mCommandBuffer);
                mCommandBuffer.Dispose();
            }

            mCommandBuffer = new CommandBuffer();
            mCommandBuffer.name = "Outline";

            mMaskRT_ID = Shader.PropertyToID("OutlineEffect_MaskRT");
            mCommandBuffer.GetTemporaryRT(mMaskRT_ID, -1, -1, 24);

            mTempRT_ID = Shader.PropertyToID("OutlineEffect_TempRT");
            mCommandBuffer.GetTemporaryRT(mTempRT_ID, -1, -1, 24);

            mCommandBuffer.SetRenderTarget(mMaskRT_ID);
            mCommandBuffer.ClearRenderTarget(true, true, Color.black);

            for (int i = 0, iMax = Model.ItemList.Count; i < iMax; i++)
            {
                var item = Model.ItemList[i];

                if (item.attachRendererSetting.attachMeshRenderers.Count > 0)
                {
                    for (int j = 0, jMax = item.attachRendererSetting.attachMeshRenderers.Count; j < jMax; j++)
                    {
                        var attachMeshRenderers = item.attachRendererSetting.attachMeshRenderers[j];

                        DrawRenderer(attachMeshRenderers, item.containSubMesh);
                    }
                }

                var renderer = item.gameObject.GetComponentInChildren<MeshRenderer>();
                DrawRenderer(renderer, item.containSubMesh);
            }

            mCommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);

            mCommandBuffer.SetGlobalTexture("_OutlineMaskTexture", mMaskRT_ID);

            mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mTempRT_ID);
            mCommandBuffer.Blit(mTempRT_ID, BuiltinRenderTextureType.CameraTarget, OutlineEffectMaterial);

            mContext.Camera.AddCommandBuffer(CameraEvent.AfterFinalPass, mCommandBuffer);
        }

        void DrawRenderer(MeshRenderer renderer, bool containSubMesh)
        {
            if (containSubMesh)
            {
                var filter = renderer.GetComponentInChildren<MeshFilter>();

                for (int j = 0, jMax = filter.sharedMesh.subMeshCount; j < jMax; j++)
                {
                    mCommandBuffer.DrawRenderer(renderer, ReplaceMaterial, j);
                }
            }
            else
            {
                mCommandBuffer.DrawRenderer(renderer, ReplaceMaterial);
            }
        }
    }
}
