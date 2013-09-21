using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain;
using CodeAssistant.Domain.Languages;
using CMCoreNET.Serialization;
using System.IO;

namespace CodeAssistant.Infrastructure
{
    public class LanguageRepository : ILanguageRepository
    {
        #region Singleton

        static LanguageRepository()
        {
            if (Repository == null)
            {
                Repository = new LanguageRepository();
            }
        }

        public static LanguageRepository Repository
        {
            get;
            private set;
        }

        #endregion

        #region Fields

        List<LanguageBase> models;
        FileProvider fileProvider;

        #endregion

        #region Properties

        public IFileProvider FileProvider
        {
            get { return this.fileProvider; }
        }

        #endregion

        #region Constructor

        LanguageRepository()
        {
            this.fileProvider = Infrastructure.FileProvider.Provider;
            this.models = new List<LanguageBase>();
            PopulateLanguages();
        }

        #endregion

        public IEnumerable<LanguageBase> Languages
        {
            get { return this.models.AsReadOnly(); }
        }

        public LanguageBase FetchLanguage(string name)
        {
            return this.models.FirstOrDefault(l => l.Name == name);
        }

        public void UpdateLanguageResource(string name, string resourcePath)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(resourcePath) ||
                !System.IO.File.Exists(resourcePath))
            {
                throw new InvalidOperationException();
            }

            var domainModel = this.models.FirstOrDefault(l => l.Name == name);

            if (domainModel != null)
            {
                domainModel.Resources
                    .ToList()
                    .ForEach(r => r.IsDefault = false);

                domainModel.AddResource(true, resourcePath);
                SaveLanguage(LanguageTypeFactory.GetGenericLanguage(domainModel));
            }
        }

        void PopulateLanguages()
        {
            var xmlFiles = this.fileProvider.FetchDirectoryContents(
                this.fileProvider.DataPath);

            if (!xmlFiles.Any())
            {
                Errors.NoLanguageDefinitions();
            }

            var languages = DeserializeDefinitions(xmlFiles);

            foreach (var entity in languages)
            {
                var domainModel = LanguageTypeFactory.GetLanguage(entity.Type);
                LanguageMaps.MapLanguage(entity, domainModel);
                this.models.Add(domainModel);
            }
        }

        IEnumerable<Language> DeserializeDefinitions(IEnumerable<string> languageXmlFiles)
        {
            var languages = new List<Language>();

            using (var serializer = SerializationAdapter.GetAdapter(SerializationAdapterType.XML))
            {
                foreach (var xmlFile in languageXmlFiles)
                {
                    Language language = null;
                    try
                    {
                        using (var stream = new FileStream(xmlFile, FileMode.Open))
                        {
                            language = serializer.Deserialize(stream, typeof(Language)) as Language;
                        }
                    }
                    finally
                    {
                        if (language != null)
                        {
                            languages.Add(language);
                        }
                    }
                }
            }
            return languages;
        }

        void SaveAll()
        {
            foreach (var domainModel in this.models)
            {
                var language = LanguageTypeFactory.GetGenericLanguage(domainModel);
                SaveLanguage(language);
            }
        }

        void SaveLanguage(Language language)
        {
            using (var serializer = SerializationAdapter.GetAdapter(SerializationAdapterType.XML))
            { 
                var contents = serializer.Serialize(language);
                this.fileProvider.SaveContents(
                    Path.Combine(
                    this.fileProvider.DataPath,
                    string.Format(
                        "{0}.xml",
                        language.Name.ToLower().Replace(".", string.Empty))),
                    contents);

            }
        }
    }
}
