using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain.Factory
{
    static class LanguageVisitorFactory
    {
        public static ILanguageVisitor GetVisitor(LanguageBase language)
        {
            if (language is CompiledLanguage)
            {
                return new CompiledLangaugeVisitor();
            }
            else if (language is InterpretedLanguage)
            {
                return new InterpretedLanguageVisitor();
            }
            else
            {
                return new LanguageVisitorBase();
            }
        }
    }
}
