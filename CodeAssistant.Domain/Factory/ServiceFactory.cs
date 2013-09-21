using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Services;

namespace CodeAssistant.Domain.Factory
{
    public sealed class ServiceFactory
    {
        ILanguageRepository repository;
        IFileProvider provider;

        public ServiceFactory(IFileProvider provider, ILanguageRepository repository)
        {
            this.repository = repository;
            this.provider = provider;
        }

        public FileService GetFileService()
        {
            return new FileService(this.provider);
        }

        public LanguageService GetLanguageService()
        {
            return new LanguageService(this.repository);
        }

        public ExecutionService GetExecutionService()
        {
            return new ExecutionService(this.provider);
        }
    }
}
