using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CodeAssistant.Domain.Languages
{
    public class ByteCodeLanguage : LanguageBase
    {
        public string Compiler { get; set; }
        public string Runtime { get; set; }
    }
}
