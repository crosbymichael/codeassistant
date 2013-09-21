using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CodeAssistant.Domain
{
    class ByteCodeFile : File
    {
        public string Extension { get; set; }
        public ByteCodeFile() { }

        public ByteCodeFile(string path, string extension) {
            this.Path = Path;
            this.Extension = extension;
        }

        public override File.ContentFileType ContentType
        {
            get { return ContentFileType.Binary; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }
    }
}
