using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain.Validation
{
    internal interface IVisitableLanguage
    {
        void AcceptVisitor(ILanguageVisitor visitor);
    }
}
