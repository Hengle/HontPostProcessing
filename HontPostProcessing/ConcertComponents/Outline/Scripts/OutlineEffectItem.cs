using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class OutlineEffectItem : MonoBehaviour
    {
        [Serializable]
        public class AttachRendererSetting
        {
            public bool childrenRendererToAttachArray = true;
            public bool childrenRendererContainInactive = true;
            public List<MeshRenderer> attachMeshRenderers = new List<MeshRenderer>();
        }

        public bool containSubMesh = true;
        public AttachRendererSetting attachRendererSetting = new AttachRendererSetting();
        HontPostProcessingProfile mProfile;


        public void CollectionChildrenRenderers()
        {
            if (attachRendererSetting.childrenRendererToAttachArray)
                attachRendererSetting.attachMeshRenderers.AddRange(transform.GetComponentsInChildren<MeshRenderer>());
        }

        void Awake()
        {
            CollectionChildrenRenderers();

            var behaviour = Camera.main.GetComponent<HontPostProcessingBehaviour>();
            mProfile = behaviour.Profile;
        }

        void OnEnable()
        {
            var outlineEffect = mProfile.ModelList.Find(m => m.Name == "Outline") as OutlineModel;
            outlineEffect.ItemList.Add(this);
        }

        void OnDisable()
        {
            var outlineEffect = mProfile.ModelList.Find(m => m.Name == "Outline") as OutlineModel;
            outlineEffect.ItemList.Remove(this);
        }
    }
}
