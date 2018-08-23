using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(VignetteModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class VignetteModelEditor : HontPostProcessingModelEditor<VignetteModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.k = EditorGUILayout.Vector4Field("K", Model.k);
        }
    }
}
