using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hont.PostProcessing.Xml
{
    public static class XmlSerializationHelper
    {
        #region --- Memory ---
        public static T DeSerializationFromMemory<T>(Stream stream, params Type[] extraTypeArr)
        {
            var xmlSer = new XmlSerializer(typeof(T), extraTypeArr);
            return (T)xmlSer.Deserialize(stream);
        }

        public static object DeSerializationFromMemory(Stream stream, Type type, params Type[] extraTypeArr)
        {
            var xmlSer = new XmlSerializer(type, extraTypeArr);
            return xmlSer.Deserialize(stream);
        }

        public static MemoryStream SerializationToMemory(object obj, params Type[] extraTypeArr)
        {
            var setting = new XmlWriterSettings();
            setting.Encoding = new UTF8Encoding(false);
            setting.Indent = true;

            var xmlSer = new XmlSerializer(obj.GetType(), extraTypeArr);
            var stream = new MemoryStream();

            using (var writer = XmlWriter.Create(stream, setting))
            {
                xmlSer.Serialize(writer, obj);
            }

            return stream;
        }
        #endregion

        #region --- Disk ---
        public static T DeSerializationFromDiskCurrentDir<T>(string fileName, params Type[] extraTypeArr)
        {
            var currentPath = Directory.GetCurrentDirectory();
            var result = default(T);
            try
            {
                result = DeSerializationFromString<T>(File.ReadAllText(currentPath + @"\" + fileName), extraTypeArr);
            }
            catch { }

            return result;
        }

        public static T DeSerializationFromDisk<T>(string path, params Type[] extraTypeArr)
        {
            return DeSerializationFromString<T>(File.ReadAllText(path), extraTypeArr);
        }

        public static void SerializationToDiskCurrentDir(string fileName, object obj, params Type[] extraTypeArr)
        {
            SerializationToDiskCurrentDir(fileName, obj, Encoding.UTF8, extraTypeArr);
        }

        public static void SerializationToDiskCurrentDir(string fileName, object obj, Encoding encoding, params Type[] extraTypeArr)
        {
            var currentPath = Directory.GetCurrentDirectory();
            SerializationToDisk(currentPath + @"\" + fileName, obj, encoding, extraTypeArr);
        }

        public static void SerializationToDisk(string path, object obj, params Type[] extraTypeArr)
        {
            SerializationToDisk(path, obj, Encoding.UTF8, extraTypeArr);
        }

        public static void SerializationToDisk(string path, object obj, Encoding encoding, params Type[] extraTypeArr)
        {
            var serResult = SerializationToString(obj, extraTypeArr);
            if (File.Exists(path)) File.Delete(path);
            File.AppendAllText(path, serResult, encoding);
        }
        #endregion

        #region --- String ---
        public static T DeSerializationFromStringBase64<T>(string xml, params Type[] extraTypeArr)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);
            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(Convert.FromBase64String(xml));
            }
            catch { }

            if (memoryStream == null) return default(T);

            return DeSerializationFromMemory<T>(memoryStream, extraTypeArr);
        }

        public static object DeSerializationFromStringBase64(string xml, Type type, params Type[] extraTypeArr)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(Convert.FromBase64String(xml));
            }
            catch { }

            if (memoryStream == null) return null;

            return DeSerializationFromMemory(memoryStream, type, extraTypeArr);
        }

        public static string SerializationToStringBase64(object obj, params Type[] extraTypeArr)
        {
            if (obj == null) return "";

            return Convert.ToBase64String((SerializationToMemory(obj, extraTypeArr)).ToArray());
        }

        public static object DeSerializationFromString(string xml, Type type, params Type[] extraTypeArr)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var xmlSer = new XmlSerializer(type, extraTypeArr);
            var reader = new StringReader(xml);
            return xmlSer.Deserialize(reader);
        }

        public static T DeSerializationFromString<T>(string xml, params Type[] extraTypeArr)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            var xmlSer = new XmlSerializer(typeof(T), extraTypeArr);
            var reader = new StringReader(xml);
            return (T)xmlSer.Deserialize(reader);
        }

        public static string SerializationToString(object obj, params Type[] extraTypeArr)
        {
            if (obj == null) return "";

            var memoryStream = SerializationToMemory(obj, extraTypeArr);

            var sr = new StreamReader(memoryStream);
            var str = Encoding.UTF8.GetString(memoryStream.ToArray());

            memoryStream.Flush();
            memoryStream.Close();
            sr.Close();

            return str;
        }
        #endregion
    }
}
