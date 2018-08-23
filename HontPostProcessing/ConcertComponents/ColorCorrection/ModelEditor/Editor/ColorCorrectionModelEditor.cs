using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(ColorCorrectionModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class ColorCorrectionModelEditor : HontPostProcessingModelEditor<ColorCorrectionModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.redCurve = EditorGUILayout.CurveField("Red Curve", Model.redCurve);
            Model.greenCurve = EditorGUILayout.CurveField("Green Curve", Model.greenCurve);
            Model.blueCurve = EditorGUILayout.CurveField("Blue Curve", Model.blueCurve);
            Model.intensity = EditorGUILayout.FloatField("Intensity", Model.intensity);
        }
    }
}
