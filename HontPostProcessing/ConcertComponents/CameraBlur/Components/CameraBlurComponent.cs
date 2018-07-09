using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class CameraBlurComponent : HontPostProcessingComponent<CameraBlurModel>
    {
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
            mCommandBuffer.GetTemporaryRT(mCameraBlurRT_ID, -1, -1, 0);
            mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mCameraBlurRT_ID);

            for (int i = 0; i < Model.sampleNum - 1; i++)
            {
                mCommandBuffer.Blit(mCameraBlurRT_ID, BuiltinRenderTextureType.CameraTarget, CameraBlurMaterial);
                mCommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, mCameraBlurRT_ID);
            }

            mCommandBuffer.Blit(mCameraBlurRT_ID, BuiltinRenderTextureType.CameraTarget, CameraBlurMaterial);

            mContext.Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, mCommandBuffer);
        }

        public override void OnPreRender()
        {
            base.OnPreRender();

            CameraBlurMaterial.SetFloat("_BlurRadius", Model.blurRadius);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            mCommandBuffer.ReleaseTemporaryRT(mCameraBlurRT_ID);
            mContext.Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, mCommandBuffer);
            mCommandBuffer.Dispose();
        }

        public override void OnRender()
        {
        }
    }
}
