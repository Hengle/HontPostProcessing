using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hont.PostProcessing.ConcertComponents
{
    public class GrainComponent : HontPostProcessingComponent<GrainModel>
    {
        Texture2D mNoiseTex;

        Material mGrainMaterial;
        Material GrainMaterial { get { return mGrainMaterial ?? (mGrainMaterial = new Material(Shader.Find("Hidden/GrainShader"))); } }

        public override string Name { get { return "Grain"; } }


        public override void OnRender()
        {
            if (mNoiseTex == null)
            {
                mNoiseTex = new Texture2D(1 << Model.noiseSize, 1 << Model.noiseSize, TextureFormat.RGBA32, false, false);
                Hilbert(Model.noiseSize, (x, y) => mNoiseTex.SetPixel(x, y, UnityEngine.Random.value < Model.noiseRate ? Color.black : Color.clear));
                mNoiseTex.Apply();
            }

            GrainMaterial.SetFloat("_Intensity", Model.intensity);
            GrainMaterial.SetFloat("_Tile", Model.tile);
            GrainMaterial.SetTexture("_GrainTex", mNoiseTex);

            var tempRT = RenderTexture.GetTemporary(mContext.CurrentRenderRT.descriptor);
            Graphics.Blit(mContext.CurrentRenderRT, tempRT);
            Graphics.Blit(tempRT, mContext.CurrentRenderRT, GrainMaterial);
            RenderTexture.ReleaseTemporary(tempRT);
        }

        void Hilbert(int n, Action<int, int> doAction, int x = 0, int y = 0, int r = 0)
        {
            const int UP = 0;
            const int RIGHT = 1;
            const int DOWN = 2;
            const int LEFT = 3;

            if (n == 0)
            {
                if (doAction != null) doAction(x, y);
                return;
            }

            int o = (int)Mathf.Pow(2, n - 1); // width of *smaller* curve

            switch (r)
            {
                case (UP):
                    Hilbert(n - 1, doAction, x, y, RIGHT); // bottom left
                    Hilbert(n - 1, doAction, x, y + o, UP); // top left
                    Hilbert(n - 1, doAction, x + o, y + o, UP); // top right
                    Hilbert(n - 1, doAction, x + o, y, LEFT); // bottom right
                    break;
                case (RIGHT):
                    Hilbert(n - 1, doAction, x, y, UP); // bottom left
                    Hilbert(n - 1, doAction, x + o, y, RIGHT); // bottom right
                    Hilbert(n - 1, doAction, x + o, y + o, RIGHT); // top right
                    Hilbert(n - 1, doAction, x, y + o, DOWN); // top left
                    break;
                case (DOWN):
                    Hilbert(n - 1, doAction, x + o, y + o, LEFT); // top right
                    Hilbert(n - 1, doAction, x + o, y, DOWN); // bottom right
                    Hilbert(n - 1, doAction, x, y, DOWN); // bottom left
                    Hilbert(n - 1, doAction, x, y + o, RIGHT); // top left
                    break;
                case (LEFT):
                    Hilbert(n - 1, doAction, x + o, y + o, DOWN); // top right
                    Hilbert(n - 1, doAction, x, y + o, LEFT); // top left
                    Hilbert(n - 1, doAction, x, y, LEFT); // bottom left
                    Hilbert(n - 1, doAction, x + o, y, UP); // bottom right
                    break;
            }
        }
    }
}
