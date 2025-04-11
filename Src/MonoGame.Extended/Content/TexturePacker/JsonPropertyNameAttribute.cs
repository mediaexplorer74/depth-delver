using System;

namespace MonoGame.Extended.Content.TexturePacker
{
    internal class JsonPropertyNameAttribute : Attribute
    {
        private string v;

        public JsonPropertyNameAttribute(string v)
        {
            this.v = v;
        }
    }
}