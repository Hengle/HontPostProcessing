using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing.ConcertComponents
{
    [HontPostProcessingModelInspector(typeof(ExampleModel))]
    [HontPostProcessingIgnore]/*Debug*/
    public class ExampleModelEditor : HontPostProcessingModelEditor<ExampleModel>
    {
        protected override void OnInspectorGUI()
        {
            Model.a = EditorGUILayout.IntField(Model.a);
        }
    }
}
