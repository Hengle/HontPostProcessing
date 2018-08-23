using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class CameraBlurComponent : HontPostProcessingComponent<CameraBlurModel>
    {
        float mLastBlurRadius;

        int mCameraBlurRT_ID;
        Material mCameraBlurMaterial;
        CommandBuffer mCommandBuffer;

        Material CameraBlurMaterial { get { return mCameraBlurMaterial ?? (mCameraBlurMaterial = new Material(Shader.Find("Hidden/CameraBlurShader"))); } }

        public override string Name { get { return "Blur"; } }


        public override void OnEnable()
        {
            base.OnEnable();

            mCommandBuffer = new CommandBuffer();
            mCommandBuffer.name = "Blur";
            mCameraBlurRT_ID = Shader.PropertyToID("BlurTempRT1");
            mContext.Camera.AddCommandBuffer(CameraEvent.AfterImageEffects, mCommandBuffer);
        }

        public override void OnPreRender()
        {
            base.OnPreRender();

            if (!Mathf.Approximately(mLastBlurRadius, Model.blurRadius))
            {
                if (Model.blurRadius <= 0)
                {
                    ClearCommandBuffer();
                }
                else
                {
                    UpdateCommandBuffer();
                }
            }

            CameraBlurMaterial.SetFloat("_BlurRadius", Model.blurRadius);

            mLastBlurRadius = Model.blurRadius;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            mCommandBuffer.ReleaseTemporaryRT(mCameraBlurRT_ID);
            mContext.Camera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, mCommandBuffer);
            mCommandBuffer.Dispose();
        }

        public override void OnRender()
        {
        }

        void UpdateCommandBuffer()
        {
            mCommandBuffer.Clear();

            mCommandBuffer.GetTemporaryRT(mCameraBlurRT_ID, -1, -1, 0);
            mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mCameraBlurRT_ID);

            for (int i = 0; i < Model.sampleNum - 1; i++)
            {
                mCommandBuffer.Blit(mCameraBlurRT_ID, BuiltinRenderTextureType.CameraTarget, CameraBlurMaterial);
                mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mCameraBlurRT_ID);
            }

            mCommandBuffer.Blit(mCameraBlurRT_ID, BuiltinRenderTextureType.CameraTarget, CameraBlurMaterial);
        }

        void ClearCommandBuffer()
        {
            mCommandBuffer.Clear();
        }
    }
}
