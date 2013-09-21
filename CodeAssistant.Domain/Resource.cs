using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeAssistant.Domain
{
    public class Resource
    {
        internal Resource(bool isDefault, string path)
        {
            this.IsDefault = isDefault;
            this.Path = path;
        }

        public string Path { get; set; }
        public bool IsDefault { get; set; }
    }
}
