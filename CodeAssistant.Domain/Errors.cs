using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeAssistant.Domain
{
    static class Errors
    {
        public static void FileNotFound(string message)
        {
            throw new FileNotFoundException(message);
        }

        public static void ExecutionException(string message)
        {
            throw new ExecutionException(message);
        }

        public static void DomainException(string message)
        {
            throw new DomainException(message);
        }

        public static void InvalidLanguageType()
        {
            throw new DomainException("The language type is not supported for this application");
        }

        //public static void LanguageNotSupported(LanguageType type)
        //{
        //    throw new LanguageNotSupported(string.Format(
        //        "Language type of {0} is not supported",
        //        type.ToString()));
        //}

        public static void ResourceNotFound(string message)
        {
            throw new ResourceNotFound(message);
        }

        public static void ResourceNotFound(string message, string path)
        {
            throw new ResourceNotFound(message);
        }

        public static void WebApiRequestFailed()
        {
            WebApiRequestFailed(null, null);
        }

        public static void WebApiRequestFailed(
            int? statusCode, 
            string statusDescription)
        {
            throw new WebApiException(statusCode, statusDescription);
        }
    }
}
