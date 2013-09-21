using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Execution;
using CodeAssistant.Domain.Development;
using CodeAssistant.Domain.Languages;

namespace CodeAssistant.Domain.Factory
{
    class ExecutionStrategyFactory
    {
        public static ExecutionStrategy Create(IFileProvider provider, SourceCode sourceCode) 
        {
            if (sourceCode.Language is CompiledLanguage)
            {
                return new CompiledExecutionStrategy(provider, sourceCode);
            }
            if (sourceCode.Language is InterpretedLanguage)
            {
                return new InterpretedExecutionStrategy(provider, sourceCode);
            }
            return null;
        }
    }
}
