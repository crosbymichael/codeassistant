using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Development;

namespace CodeAssistant.Domain.Execution
{
    class ByteCodeExecutionStrategy : ExecutionStrategy
    {
        public ByteCodeExecutionStrategy(IFileProvider provider, SourceCode code) 
            : base(provider, code) 
        { 
            
        }

        public override Executable GetExecutable()
        {
            var compiler = new Compiler(this.fileProvider);
            var result = compiler.Compile(this.SourceCode);

            throw new NotImplementedException();
        }
    }
}
                 