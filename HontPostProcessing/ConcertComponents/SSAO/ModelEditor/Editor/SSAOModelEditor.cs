using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(SSAOModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class SSAOModelEditor : HontPostProcessingModelEditor<SSAOModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.radius = EditorGUILayout.FloatField("Radius", Model.radius);
            Model.occlusionIntensity = EditorGUILayout.FloatField("OcclusionIntensity", Model.occlusionIntensity);
            Model.downsampling = EditorGUILayout.IntField("Downsampling", Model.downsampling);
            Model.downsampling = Mathf.Max(Model.downsampling, 0);
            Model.minZ = EditorGUILayout.FloatField("MinZ", Model.minZ);
        }
    }
}
