using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hont.PostProcessing.ConcertComponents
{
    public class OutlineEffectItem : MonoBehaviour
    {
        HontPostProcessingProfile mProfile;


        void OnEnable()
        {
            var behaviour = Camera.main.GetComponent<HontPostProcessingBehaviour>();
            mProfile = behaviour.Profile;

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
