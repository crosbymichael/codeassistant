using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain.UnitTests
{
    public class MockLanguageRepository : ILanguageRepository
    {
        List<Languages.LanguageBase> languges = new List<Languages.LanguageBase>();

        public MockLanguageRepository()
        {
            var lang = new Languages.InterpretedLanguage
            { 
                Name = "Php",
                Extension = ".php",
                Template = "<?php\n\techo \"Hello world\";\n?>",
                Arguments = "{inputPath}"
            };

            lang.AddResource(true, @"C:\php5\php.exe");
            this.languges.Add(lang);
        }

        public Languages.LanguageBase FetchLanguage(string name)
        {
            return this.languges.FirstOrDefault(l => l.Name == name);
        }

        public IEnumerable<Languages.LanguageBase> Languages
        {
            get { return this.languges.AsReadOnly(); }
        }

        public void UpdateLanguageResource(string name, string resourcePath)
        {
            throw new NotImplementedException();
        }
    }
}
