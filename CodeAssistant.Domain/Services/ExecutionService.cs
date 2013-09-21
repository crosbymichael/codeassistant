using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Development;
using System.Configuration;
using CodeAssistant.Domain.Validation;
using CodeAssistant.Domain.Execution;
using CodeAssistant.Domain.Factory;

namespace CodeAssistant.Domain.Services
{
    public class ExecutionEvent : EventArgs
    {
        public string Output { get; private set; }
        public string Error { get; private set; }
        public bool IsComplete { get; private set; }

        internal ExecutionEvent(string output, string error)
        {
            this.Output = output;
            this.Error = error;
        }

        internal ExecutionEvent(string output, string error, bool isComplete)
            : this(output, error)
        {
            this.IsComplete = isComplete;
        }
    }

    public sealed class ExecutionService
    {
        public event DomainExecutionDelegate ExecutionDataEvent;

        IFileProvider provider;
        Executable executable;

        internal ExecutionService(IFileProvider provider)
        {
            this.provider = provider;
        }

        #region Methods

        public void ExecuteCode(
            SourceCode sourceCode)
        {
            ExecuteCode(sourceCode, null);
        }

        public void ExecuteCode(
            SourceCode sourceCode, 
            string additionalArguments)
        {
            ValidateCode(sourceCode);

            try
            {
                CreateExecutable(sourceCode, additionalArguments);

                AddAdditionalArguments(additionalArguments);
                AttachEvents();

                Execute();
            }
            catch (ResourceNotFound notFound)
            {
                Errors.DomainException(string.Format(
                    "{0} was not found for {1}. Press Ctrl+F to find a new compiler/runtime to use.",
                    notFound.Path,
                    sourceCode.Language.Name));
            }
        }

        public void Stop()
        {
            if (this.executable != null)
            {
                this.executable.Stop();
                DispatchEvent(
                    string.Empty,
                    "Execution stopped...",
                    true);
            }
        }

        void executable_Event(object sender, ExecutionEventArgs args)
        {
            DispatchEvent(
                args.StandardOutput,
                args.StandardError,
                args.IsComplete);

            if (args.IsComplete)
            {
                DisposeExecutable();
            }
        }

        void ValidateCode(SourceCode sourceCode)
        {
            var visitor = LanguageVisitorFactory.GetVisitor(sourceCode.Language);
            sourceCode.Language.AcceptVisitor(visitor);
        }

        void CreateExecutable(
            SourceCode sourceCode,
            string additionalArguments)
        {
            var executionStrategy = ExecutionStrategyFactory.Create(
                this.provider,
                sourceCode);

            this.executable = executionStrategy.GetExecutable();
        }

        void AddAdditionalArguments(
            string arguments)
        {
            if (!string.IsNullOrEmpty(arguments))
            {
                if (string.IsNullOrEmpty(executable.Arguments))
                {
                    executable.Arguments = arguments;
                }
                else
                {
                    executable.Arguments = string.Format(
                        "{0} {1}",
                        executable.Arguments,
                        arguments);
                }
            }
        }

        void AttachEvents()
        {
            this.executable.OutputEvent += executable_Event;
            this.executable.CompletionEvent += executable_CompletionEvent;
        }

        void executable_CompletionEvent(object sender, ExecutionEventArgs args)
        {
            DispatchEvent(
                args.StandardOutput,
                args.StandardError,
                true);

            DisposeExecutable();
        }

        void Execute()
        {
            this.executable.Execute();
        }

        void DisposeExecutable()
        {
            this.executable.CompletionEvent -= executable_Event;
            this.executable.OutputEvent -= executable_Event;

            this.executable.Dispose();
            this.executable = null;
        }

        void DispatchEvent(
            string output, 
            string error, 
            bool isComplete)
        {
            var copy = this.ExecutionDataEvent;
            if (copy != null)
            {
                copy(this, new ExecutionEvent(output, error, isComplete));
            }
        }

        #endregion
    }
}
