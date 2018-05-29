using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hont.PostProcessing
{
    [HontPostProcessingModelInspector(typeof(Test1Model))]
    //[HontPostProcessingIgnore]/*Debug*/
    public class Test1ModelEditor : HontPostProcessingModelEditor<Test1Model>
    {
        protected override void OnInspectorGUI()
        {
            Model.a = EditorGUILayout.IntField(Model.a);
        }
    }
}
