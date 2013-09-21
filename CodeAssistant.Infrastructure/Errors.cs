using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Infrastructure
{
    static class Errors
    {
        public static void NoLanguageDefinitions()
        {
            throw new InvalidOperationException();
        }
    }
}
