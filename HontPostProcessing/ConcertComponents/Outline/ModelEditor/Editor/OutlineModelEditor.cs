using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(OutlineModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class OutlineModelEditor : HontPostProcessingModelEditor<OutlineModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.outlineColor = EditorGUILayout.ColorField("Outline Color", Model.outlineColor);
            Model.outlineWidth = EditorGUILayout.FloatField("Outline Width", Model.outlineWidth);
        }
    }
}
