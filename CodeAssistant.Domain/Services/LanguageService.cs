using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain.Services
{
    public class LanguageService
    {
        ILanguageRepository repository;

        internal LanguageService(ILanguageRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        #region Properties

        public IEnumerable<string> LanguageNames
        {
            get
            {
                return this.repository.Languages.Select(l => l.Name);
            }
        }

        #endregion

        #region Methods

        public Languages.LanguageBase GetLanguage(string languageName)
        {
            return this.repository.FetchLanguage(languageName);
        }

        public void UpdateLanguageResource(string languageName, string resourcePath)
        {
            this.repository.UpdateLanguageResource(languageName, resourcePath);
        }

        #endregion
    }
}
