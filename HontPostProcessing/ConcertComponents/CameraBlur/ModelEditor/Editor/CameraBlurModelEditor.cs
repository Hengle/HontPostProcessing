using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(CameraBlurModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class CameraBlurModelEditor : HontPostProcessingModelEditor<CameraBlurModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.sampleNum = EditorGUILayout.IntField("Sample Num", Model.sampleNum);
            Model.blurRadius = EditorGUILayout.FloatField("Blur Radius", Model.blurRadius);
        }
    }
}
