using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using CMCoreNET;

namespace CodeAssistant.Domain.Development
{
    class Compiler : Executable
    {
        IFileProvider provider;

        public Compiler(IFileProvider provider) 
        {
            this.provider = provider;
        }

        public File Compile(SourceCode code) 
        {
            if (code == null)
            {
                throw new ArgumentNullException();
            }

            return this.Compile(code, null);
        }

        public File Compile(SourceCode code, string extension) 
        {
            if (code == null)
            {
                throw new ArgumentNullException();
            }

            if (!code.Exists)
            {
                code.Save();
            }

            string ext = (extension != null) ? 
                extension : 
                Executable.DEFAULT_EXTENSION;

            var language = code.Language as CompiledLanguage;

            this.Path = language.Resources
                .FirstOrDefault(r => r.IsDefault == true).Path;

            string outputFilePath = this.provider.GetTempFilePath(ext);

            if (!string.IsNullOrEmpty(language.Arguments))
            {
                this.Arguments = language.Arguments.ReplaceWithProperties(
                    new { 
                        outputPath = outputFilePath,
                        inputPath = code.Path
                    });
            }
            
            try
            {
                this.Execute();
                this.Wait();
            }
            catch
            {
                Errors.ExecutionException(GetCurrentOutput());
            }

            if (!System.IO.File.Exists(outputFilePath))
            {
                Errors.ExecutionException(GetCurrentOutput());
            }

            return new CompiledFile(outputFilePath);
        }

        string GetCurrentOutput()
        {
            return string.Format(
                "{0}{1}{2}",
                this.Output,
                Environment.NewLine,
                this.Error);
        }
    }
}
