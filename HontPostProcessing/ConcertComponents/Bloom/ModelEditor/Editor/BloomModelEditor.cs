using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(BloomModel))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class BloomModelEditor : HontPostProcessingModelEditor<BloomModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.streak_Length = EditorGUILayout.FloatField("Streak Length", Model.streak_Length);
        }
    }
}
