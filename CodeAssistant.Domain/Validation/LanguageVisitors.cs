using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;

namespace CodeAssistant.Domain.Validation
{
    internal class CompiledLangaugeVisitor : LanguageVisitorBase
    {
        public override void Visit(LanguageBase language)
        {
            base.Visit(language);
            var list = language.Resources;
            ProcessResources(language.Name, list);
        }
    }

    internal class InterpretedLanguageVisitor : LanguageVisitorBase
    {
        public override void Visit(LanguageBase language)
        {
            base.Visit(language);
            var list = language.Resources;
            ProcessResources(language.Name, list);
        }
        
    }

    
}
