using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Validation;

namespace CodeAssistant.Domain.Languages
{
	public abstract class LanguageBase : IVisitableLanguage, IEquatable<LanguageBase>
	{
        List<Resource> resources;

        public string Template { get; set; }
        public string Syntax { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Arguments { get; set; }

        public IEnumerable<Resource> Resources 
        { 
            get { return this.resources.AsReadOnly(); } 
        }

        public LanguageBase()
        {
            this.resources = new List<Resource>();
        }

        public Resource AddResource(bool isDefault, string path)
        {
            var temp = new Resource(isDefault, path);
            this.resources.Add(temp);
            return temp;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            if (!string.IsNullOrEmpty(Template))
            {
                hash = hash ^ Template.GetHashCode();
            }
            if (!string.IsNullOrEmpty(Syntax))
            {
                hash = hash ^ Syntax.GetHashCode();
            }
            if (!string.IsNullOrEmpty(Extension))
            {
                hash = hash ^ Extension.GetHashCode();
            }
            if (!string.IsNullOrEmpty(Arguments))
            {
                hash = hash ^ Arguments.GetHashCode();
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LanguageBase);
        }

        public bool Equals(LanguageBase other)
        {
            return (other != null) ? this.Name == other.Name : false;
        }

        public void AcceptVisitor(ILanguageVisitor visitor)
        {
            visitor.Visit(this);
        } 
    }
}

