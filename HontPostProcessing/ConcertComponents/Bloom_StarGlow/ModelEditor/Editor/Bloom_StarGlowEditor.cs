using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(Bloom_StarGlowModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class Bloom_StarGlowEditor : HontPostProcessingModelEditor<Bloom_StarGlowModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.streak_Length = EditorGUILayout.FloatField("Streak Length", Model.streak_Length);
            Model.threshold = EditorGUILayout.FloatField("Threshold", Model.threshold);
        }
    }
}
