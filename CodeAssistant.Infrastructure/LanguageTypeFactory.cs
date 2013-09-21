using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;

namespace CodeAssistant.Infrastructure
{
    static class LanguageTypeFactory
    {
        public static LanguageBase GetLanguage(LanguageType type)
        {
            switch (type)
            {
                case LanguageType.Compiled:
                    return new CompiledLanguage();
                case LanguageType.Interpreted:
                    return new InterpretedLanguage();
                case LanguageType.ByteCode:
                    return new ByteCodeLanguage();
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Language GetGenericLanguage(LanguageBase language)
        {
            var generic = new Language
            {
                Name = language.Name,
                Extension = language.Extension,
                Arguments = language.Arguments,
                Template = language.Template,
                Syntax = language.Syntax
            };

            generic.Type = GetLanguageType(language);
            generic.Resources = (from r in language.Resources
                                 select new Infrastructure.Resource { IsDefault = r.IsDefault, Path = r.Path }).ToList();

            return generic;
        }

        static LanguageType GetLanguageType(LanguageBase language)
        {
            return (language is CompiledLanguage) ? LanguageType.Compiled : LanguageType.Interpreted;
        }
    }
}
