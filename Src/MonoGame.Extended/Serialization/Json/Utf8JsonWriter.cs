using Newtonsoft.Json;
using System;

namespace MonoGame.Extended.Serialization.Json
{
    public class Utf8JsonWriter : JsonWriter
    {
        public Utf8JsonWriter()
        {
        }
        public override void WriteStartObject()
        {
            base.WriteStartObject();
        }
        public override void WriteEndObject()
        {
            base.WriteEndObject();
        }
        public override void WriteStartArray()
        {
            base.WriteStartArray();
        }
        public override void WriteEndArray()
        {
            base.WriteEndArray();
        }
        public override void WritePropertyName(string propertyName)
        {
            base.WritePropertyName(propertyName);
        }
      
        public override void Flush()
        {
            //
        }

        internal void WriteString(string v1, string v2)
        {
            //
        }

        internal void WriteStringValue(string assetName)
        {
            throw new NotImplementedException();
        }

        internal void WriteNumberValue(float value)
        {
            throw new NotImplementedException();
        }

        internal void WriteNumber(string v, float length)
        {
            throw new NotImplementedException();
        }

        internal void WriteNumberValue(double totalSeconds)
        {
            throw new NotImplementedException();
        }
    }  
}