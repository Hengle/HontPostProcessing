using System;

namespace Hont.PostProcessing
{
    public class HontPostProcessingModelInspectorAttribute : Attribute
    {
        public readonly Type type;


        public HontPostProcessingModelInspectorAttribute(Type type)
        {
            this.type = type;
        }
    }
}
