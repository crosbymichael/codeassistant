using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Services;

namespace CodeAssistant.Domain
{
    public delegate void DomainExecutionDelegate(object sender, ExecutionEvent args);
}
