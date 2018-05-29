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
        public List<HontPostProcessingModelBase> modelList = new List<HontPostProcessingModelBase>();

        [SerializeField]
        string modelSerializeData;

        List<HontPostProcessingComponentBase> mComponentList;
        HontPostProcessingContext mContext;


        public void Init(Camera camera)
        {
            mContext = new HontPostProcessingContext() { Camera = camera };

            mComponentList = HontPostProcessingUtility.InstancesFromBaseClass<HontPostProcessingComponentBase>();

            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];
                var matchModel = modelList.Find(m => m.Name == item.Name);
                item.Init(mContext, matchModel);
            }
        }

        public void Enable()
        {
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];
                item.OnEnable();
            }
        }

        public void Disable()
        {
            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];
                item.OnDisable();
            }
        }

        public void Render(RenderTexture src, RenderTexture dst)
        {
            mContext.CurrentRenderRT = src;

            for (int i = 0, iMax = mComponentList.Count; i < iMax; i++)
            {
                var item = mComponentList[i];

                item.OnRender();
            }

            Graphics.Blit(mContext.CurrentRenderRT, dst);
        }

        #region --- ISerializationCallbackReceiver Members ---

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var extraTypes = HontPostProcessingUtility.GetChildrenClasses<HontPostProcessingModelBase>();
            modelSerializeData = XmlSerializationHelper.SerializationToString(modelList, extraTypes);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            var extraTypes = HontPostProcessingUtility.GetChildrenClasses<HontPostProcessingModelBase>();
            modelList = XmlSerializationHelper.DeSerializationFromString<List<HontPostProcessingModelBase>>(modelSerializeData, extraTypes);
        }
        #endregion
    }
}
