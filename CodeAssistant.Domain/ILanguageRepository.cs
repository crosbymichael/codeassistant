using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;

namespace CodeAssistant.Domain
{
    /// <summary>
    /// External dependency
    /// </summary>
    public interface ILanguageRepository
    {
        IEnumerable<LanguageBase> Languages { get; }
        LanguageBase FetchLanguage(string name);
        void UpdateLanguageResource(string name, string resourcePath);
    }
}
