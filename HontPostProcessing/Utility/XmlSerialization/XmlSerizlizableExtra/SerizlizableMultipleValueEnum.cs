using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Hont.PostProcessing.Xml
{
    [XmlRoot("SerizlizableMultipleValueEnum"), Serializable]
    public class SerizlizableMultipleValueEnum<TEnum> : IXmlSerializable
        where TEnum : IConvertible
    {
        TEnum mEnumValue;

        public TEnum EnumValue { get { return mEnumValue; } set { mEnumValue = value; } }


        public SerizlizableMultipleValueEnum()
        {
        }

        public SerizlizableMultipleValueEnum(TEnum enumValue)
        {
            this.mEnumValue = enumValue;
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var typeSer = new XmlSerializer(typeof(int));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("EnumValue");
                mEnumValue = (TEnum)typeSer.Deserialize(reader);
                reader.ReadEndElement();

                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            var typeSer = new XmlSerializer(typeof(int));

            writer.WriteStartElement("EnumValue");
            typeSer.Serialize(writer, Convert.ToInt32(mEnumValue));
            writer.WriteEndElement();
        }

        #endregion
    }
}
