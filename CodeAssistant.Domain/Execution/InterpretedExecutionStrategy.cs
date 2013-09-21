using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain.Execution
{
    class InterpretedExecutionStrategy : ExecutionStrategy
    {
        public InterpretedExecutionStrategy()
        {

        }

        public InterpretedExecutionStrategy(IFileProvider provider, SourceCode code)
            : base(provider, code)
        {

        }

        public override Executable GetExecutable()
        {
            if (this.SourceCode.Language is InterpretedLanguage == false)
            {
                Errors.InvalidLanguageType();
            }

            return this.CreateRuntimeFromSource();
        }

        private Executable CreateRuntimeFromSource()
        {
            return new Runtime(this.SourceCode);
        }
    }
}
