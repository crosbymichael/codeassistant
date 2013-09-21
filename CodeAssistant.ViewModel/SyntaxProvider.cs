using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Highlighting;
using CodeAssistant.Domain.Languages;

namespace CodeAssistant.ViewModel
{
    static class SyntaxProvider
    {
        public static IHighlightingDefinition GetDefinition(LanguageBase language)
        {
            IHighlightingDefinition syntax = null;

            if (!string.IsNullOrEmpty(language.Syntax))
            {
                //TODO: Read from files
                syntax = HighlightingManager.Instance.GetDefinitionByExtension("." + language.Extension);
            }
            else
            {
                syntax = HighlightingManager.Instance.GetDefinitionByExtension("." + language.Extension);
            }
            return syntax;
        }
    }
}
