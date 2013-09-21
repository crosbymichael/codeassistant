using CodeAssistant.Domain.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain.Validation
{
    public interface ILanguageVisitor
    {
        void Visit(LanguageBase language);
    }

    internal class LanguageVisitorBase : ILanguageVisitor
    {

        #region ILanguageValidator Members

        public virtual void Visit(LanguageBase language)
        {
            //Arguments and Syntax are not required
            if (string.IsNullOrEmpty(language.Extension))
            {
                Errors.DomainException("Extension cannot be empty");
            }

            if (string.IsNullOrEmpty(language.Template))
            {
                Errors.DomainException("Template cannot be empty");
            }

            if (string.IsNullOrEmpty(language.Name))
            {
                Errors.DomainException("Name cannot be empty");
            }
        }

        #endregion

        protected bool PathExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        protected void ProcessResources(string language, IEnumerable<Resource> resources)
        {
            bool anyValid = true;
            foreach (var resource in resources)
            {
                string path = resource.Path;

                if (string.IsNullOrEmpty(path))
                {
                    anyValid = false;
                }
                else if (!PathExists(path))
                {
                    anyValid = false;
                }
                else
                {
                    anyValid = true;
                    break;
                }
            }

            if (!anyValid)
            {
                resources.FirstOrDefault().IsDefault = true;
                Errors.ResourceNotFound(
                    string.Format(
                            "Runtime path for {0} is invalid",
                            language),
                            resources.Where(r => r.IsDefault)
                                .FirstOrDefault()
                                    .Path);
            }
        }
    }
}
