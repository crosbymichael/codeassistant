using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CMCoreNET;

namespace CodeAssistant.Domain.Development
{
    public class SourceCode : File
    {
        public LanguageBase Language 
        { 
            get; 
            private set; 
        }

        internal SourceCode() 
        { 
        
        }

        public SourceCode(string code, LanguageBase language) 
        {
            if (string.IsNullOrEmpty(code) ||
                language == null)
            {
                throw new ArgumentNullException("code");
            }
            this.Content = code.GetBytes();
            this.Language = language;
        }

        public SourceCode(string code, LanguageBase language, string filePath) 
        {
            if (string.IsNullOrEmpty(code) || 
                language == null ||
                string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException();
            }
            this.Content = code.GetBytes();
            this.Language = language;
            this.Path = filePath;
        }

        public override File.ContentFileType ContentType
        {
            get { return ContentFileType.Text; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }
    }
}
