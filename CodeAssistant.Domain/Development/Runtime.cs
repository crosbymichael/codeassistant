using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CMCoreNET;

namespace CodeAssistant.Domain.Development
{
    class Runtime : Executable
    {
        public Runtime()
        {

        }

        public Runtime(SourceCode code)
        {
            if (!code.Exists)
            {
                code.Save();
            }

            this.Path = code.Language
                .Resources
                .Where(r => r.IsDefault)
                .FirstOrDefault()
                    .Path;

            this.Arguments = code.Language.Arguments.ReplaceWithProperties(new
            {
                inputPath = code.Path
            });
        }
    }
}
