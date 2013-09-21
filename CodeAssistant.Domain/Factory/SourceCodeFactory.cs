using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain.Factory
{
    public class SourceCodeFactory
    {
        IFileProvider provider;

        public SourceCodeFactory(IFileProvider provider)
        {
            this.provider = provider;
        }

		public SourceCode GetSourceCode(LanguageBase language, string code) 
        {
            return new SourceCode(
                code, 
                language, 
                this.provider.GetTempFilePath(language.Extension));
		}
    }
}
