using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class ColorCorrectionComponent : HontPostProcessingComponent<ColorCorrectionModel>
    {
        Texture2D mCurveConvertTex;

        Material mColorCorrectionMaterial;
        Material ColorCorrectionMaterial { get { return mColorCorrectionMaterial ?? (mColorCorrectionMaterial = new Material(Shader.Find("Hidden/ColorCorrectionShader"))); } }

        public override string Name { get { return "ColorCorrection"; } }


        public override void OnPreRender()
        {
            base.OnPreRender();

            if (mCurveConvertTex == null)
            {
                mCurveConvertTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
                mCurveConvertTex.filterMode = FilterMode.Point;
                mCurveConvertTex.wrapMode = TextureWrapMode.Clamp;
            }

            for (float i = 0.0f, step = 1.0f / 255.0f; i <= 1.0f; i += step)
            {
                float rCh = Mathf.Clamp(Model.redCurve.Evaluate(i), 0.0f, 1.0f);
                float gCh = Mathf.Clamp(Model.greenCurve.Evaluate(i), 0.0f, 1.0f);
                float bCh = Mathf.Clamp(Model.blueCurve.Evaluate(i), 0.0f, 1.0f);

                var x = (int)Mathf.Floor(i * 255.0f);
                var y = 2;

                mCurveConvertTex.SetPixel(x, y, new Color(rCh, gCh, bCh));
            }

            mCurveConvertTex.Apply();
        }

        public override void OnRender()
        {
            ColorCorrectionMaterial.SetFloat("_Intensity", Model.intensity);
            ColorCorrectionMaterial.SetTexture("_RgbTex", mCurveConvertTex);

            var tempRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);
            Graphics.Blit(mContext.CurrentRenderRT, tempRT);
            Graphics.Blit(tempRT, mContext.CurrentRenderRT, ColorCorrectionMaterial);
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}
