using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain.Execution
{
    abstract class ExecutionStrategy
    {
        protected SourceCode SourceCode { get; set; }
        protected IFileProvider fileProvider;

        public ExecutionStrategy()
        {

        }

        public ExecutionStrategy(IFileProvider provider, SourceCode code)
        {
            this.SourceCode = code;
            this.SourceCode.Save();

            this.fileProvider = provider;
        }

        public abstract Executable GetExecutable();
    }
}
