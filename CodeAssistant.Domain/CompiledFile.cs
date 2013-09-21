using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain
{
    class CompiledFile : File
    {
        public CompiledFile(string path)
        {
            this.Path = path;
        }

        public override File.ContentFileType ContentType
        {
            get { return ContentFileType.Binary; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public Executable AsExecutable()
        {
            return new Executable(this.Path);
        }
    }
}
