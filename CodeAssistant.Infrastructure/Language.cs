using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CodeAssistant.Infrastructure
{
    [Serializable]
    public enum LanguageType
    {
        Compiled = 1,
        Interpreted = 2,
        ByteCode = 3
    }

    [Serializable]
    public class Resource
    {
        public string Path { get; set; }
        [XmlAttribute]
        public bool IsDefault { get; set; }
    }

    [Serializable]
    [DataContract]
    [XmlRoot("Language", Namespace = @"http://code-assistant.com")]
    public class Language
    {
        [DataMember]
        public LanguageType Type { get; set; }

        [DataMember]
        public string Template { get; set; }

        [DataMember]
        public string Syntax { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string Arguments { get; set; }

        [DataMember]
        [XmlArray]
        public List<Resource> Resources { get; set; }
    }
}
