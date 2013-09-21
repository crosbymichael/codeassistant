using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain
{
    public class ExecutionException : DomainException
    {
        public ExecutionException(string error)
            : base(error)
        {
        }
    }

    public class ResourceNotFound : DomainException
    {
        public ResourceNotFound(string error)
            : base(error)
        { }
        public ResourceNotFound(string error, string path)
            : base(error)
        {
            this.Path = path;
        }

        public string Path { get; set; }
    }

    public class DomainException : Exception
    {
        public DomainException(string error)
        {
            this.Error = error;
        }

        public string Error { get; set; }
    }

    public class LanguageNotSupported : Exception
    {
        public LanguageNotSupported() { }
        public LanguageNotSupported(string message) : base(message) { }
    }

    public class WebApiException : Exception
    {
        public int? StatusCode { get; private set; }
        public string StatusDescription { get; private set; }

        public WebApiException()
        { 
        
        }

        public WebApiException(int? statuscode, string description)
        {
            this.StatusCode = statuscode;
            this.StatusDescription = description;
        }
    }
}
