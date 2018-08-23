using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(GrainModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class GrainModelEditor : HontPostProcessingModelEditor<GrainModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.intensity = Mathf.Clamp01(EditorGUILayout.FloatField("Intensity", Model.intensity));
            Model.noiseRate = Mathf.Clamp01(EditorGUILayout.FloatField("NoiseRate", Model.noiseRate));
            Model.noiseSize = Mathf.Clamp(EditorGUILayout.IntField("NoiseSize", Model.noiseSize), 4, 9);
            Model.tile = Mathf.Clamp(EditorGUILayout.FloatField("tile", Model.tile), 1f, 20f);
        }
    }
}
