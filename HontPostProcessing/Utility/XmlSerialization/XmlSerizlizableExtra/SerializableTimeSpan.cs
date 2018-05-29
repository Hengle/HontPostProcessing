using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Hont.PostProcessing.Xml
{
    public class SerializableTimeSpan
    {
        TimeSpan mInternal = TimeSpan.Zero;


        public SerializableTimeSpan()
        : this(TimeSpan.Zero)
        {
        }

        public SerializableTimeSpan(TimeSpan input)
        {
            mInternal = input;
        }

        public static implicit operator TimeSpan(SerializableTimeSpan input)
        {
            return (input != null) ? input.mInternal : TimeSpan.Zero;
        }

        // Alternative to the implicit operator TimeSpan(XmlTimeSpan input)
        public TimeSpan ToTimeSpan()
        {
            return mInternal;
        }

        public static implicit operator SerializableTimeSpan(TimeSpan input)
        {
            return new SerializableTimeSpan(input);
        }

        // Alternative to the implicit operator XmlTimeSpan(TimeSpan input)
        public void FromTimeSpan(TimeSpan input)
        {
            this.mInternal = input;
        }

        [XmlText]
        public string Value
        {
            get
            {
                return XmlConvert.ToString(mInternal);
            }
            set
            {
                mInternal = XmlConvert.ToTimeSpan(value);
            }
        }
    }
}
