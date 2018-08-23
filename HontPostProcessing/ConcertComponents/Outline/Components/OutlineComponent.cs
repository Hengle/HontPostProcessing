using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class OutlineComponent : HontPostProcessingComponent<OutlineModel>
    {
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

            mCommandBuffer = new CommandBuffer();
            mCommandBuffer.name = "Outline";
            mMaskRT_ID = Shader.PropertyToID("OutlineEffect_MaskRT");
            mTempRT_ID = Shader.PropertyToID("OutlineEffect_TempRT");

            mContext.Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, mCommandBuffer);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            mContext.Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, mCommandBuffer);
            mCommandBuffer.Dispose();
        }

        public override void OnPreRender()
        {
            base.OnPreRender();

            if (Model.IsDirty)
            {
                UpdateCommandBuffer();
            }
        }

        public override void OnRender()
        {
        }

        void UpdateCommandBuffer()
        {
            mCommandBuffer.Clear();

            mCommandBuffer.GetTemporaryRT(mMaskRT_ID, -1, -1, 24);
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
                else
                {
                    var renderer = item.gameObject.GetComponentInChildren<MeshRenderer>();

                    if (renderer != null)
                        DrawRenderer(renderer, item.containSubMesh);
                }
            }

            mCommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);

            mCommandBuffer.SetGlobalTexture("_OutlineMaskTexture", mMaskRT_ID);

            mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mTempRT_ID);
            mCommandBuffer.Blit(mTempRT_ID, BuiltinRenderTextureType.CameraTarget, OutlineEffectMaterial);

            mCommandBuffer.ReleaseTemporaryRT(mMaskRT_ID);
            mCommandBuffer.ReleaseTemporaryRT(mTempRT_ID);
        }

        void DrawRenderer(MeshRenderer renderer, bool containSubMesh)
        {
            if (containSubMesh)
            {
                var filter = renderer.GetComponentInChildren<MeshFilter>();

                for (int i = 0, iMax = filter.sharedMesh.subMeshCount; i < iMax; i++)
                {
                    DrawRenderer_Imple(renderer, i);
                }
            }
            else
            {
                DrawRenderer_Imple(renderer);
            }
        }

        void DrawRenderer_Imple(MeshRenderer renderer, int subMeshIndex = -1)
        {
            var material = renderer.sharedMaterial;

            if (material)
                ReplaceMaterial.mainTexture = material.mainTexture;

            if (subMeshIndex == -1)
                mCommandBuffer.DrawRenderer(renderer, ReplaceMaterial);
            else
                mCommandBuffer.DrawRenderer(renderer, ReplaceMaterial, subMeshIndex);
        }
    }
}
