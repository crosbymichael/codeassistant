using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain.Execution
{
    class CompiledExecutionStrategy : ExecutionStrategy
    {
        public CompiledExecutionStrategy()
        {

        }

        public CompiledExecutionStrategy(IFileProvider provider, SourceCode code)
            : base(provider, code)
        {

        }

        public override Executable GetExecutable()
        {
            if (this.SourceCode.Language is CompiledLanguage == false)
            {
                Errors.InvalidLanguageType();
            }

            var compiler = new Compiler(this.fileProvider);

            File result = null;
            try
            {
                result = compiler.Compile(this.SourceCode);
            }
            catch { throw; }

            return ((CompiledFile)result).AsExecutable();
        }
    }
}
