using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hont.PostProcessing
{
    using Hont.PostProcessing.Xml;

    [CreateAssetMenu(fileName = "Hont Post-Processing Profile", menuName = "Hont Post Processing Profile", order = 204)]
    public class HontPostProcessingProfile : ScriptableObject, ISerializationCallbackReceiver
    {
        List<HontPostProcessingModelBase> mModelList = new List<HontPostProcessingModelBase>();

        [SerializeField]
        string modelSerializeData;

        List<HontPostProcessingComponentBase> mComponentList;
        HontPostProcessingContext mContext;

        public List<HontPostProcessingModelBase> ModelList { get { return mModelList; } }


        public void Init(Camera camera)
        {
            mContext = new HontPostProcessingContext() { Camera = camera };

            mComponentList = HontPostProcessingUtility.InstancesFromBaseClass<HontPostProcessingComponentBase>();

            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];
                var matchModel = mModelList.Find(m => m.Name == item.Name);
                item.Init(mContext, matchModel);
            }
        }

        public void Enable()
        {
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];

                if (item.GetModel().Enabled)
                    item.OnEnable();
            }
        }

        public void Disable()
        {
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];

                if (item.GetModel().Enabled)
                    item.OnDisable();
            }
        }

        public void PreRender()
        {
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];

                if (item.GetModel().Enabled)
                    item.OnPreRender();
            }
        }

        public void Render(RenderTexture src, RenderTexture dst)
        {
            mContext.CurrentRenderRT = RenderTexture.GetTemporary(src.descriptor);
            mContext.CurrentDstRT = RenderTexture.GetTemporary(src.descriptor);

            Graphics.Blit(src, mContext.CurrentRenderRT);

            var currentDstRT = mContext.CurrentRenderRT;
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];

                if (item.GetModel().Enabled)
                {
                    item.OnRender();

                    currentDstRT = mContext.CurrentDstRT;
                }

                var cacheRT = mContext.CurrentRenderRT;
                mContext.CurrentRenderRT = mContext.CurrentDstRT;
                mContext.CurrentDstRT = cacheRT;
            }

            Graphics.Blit(currentDstRT, dst);

            RenderTexture.ReleaseTemporary(mContext.CurrentRenderRT);
            RenderTexture.ReleaseTemporary(mContext.CurrentDstRT);
        }

        #region --- ISerializationCallbackReceiver Members ---

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var extraTypes = HontPostProcessingUtility.GetChildrenClasses<HontPostProcessingModelBase>();
            modelSerializeData = XmlSerializationHelper.SerializationToString(mModelList, extraTypes);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            try
            {
                var extraTypes = HontPostProcessingUtility.GetChildrenClasses<HontPostProcessingModelBase>();
                mModelList = XmlSerializationHelper.DeSerializationFromString<List<HontPostProcessingModelBase>>(modelSerializeData, extraTypes);
            }
            catch
            {
            }
        }
        #endregion
    }
}
