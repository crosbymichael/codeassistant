using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CodeAssistant.Domain.Development
{
    public class ExecutionEventArgs : EventArgs
    {
        public bool IsComplete { get; private set; }
        public string StandardOutput { get; private set; }
        public string StandardError { get; private set; }

        internal ExecutionEventArgs(string standardOutput, string standardError)
        {
            this.StandardOutput = standardOutput;
            this.StandardError = standardError;
        }

        internal ExecutionEventArgs(string standardOutput, string standardError, bool isComplete)
            : this(standardOutput, standardError)
        {
            this.IsComplete = IsComplete;
        }
    }

    class Executable : CodeAssistant.Domain.File, IDisposable
    {
        #region Fields

        public const string DEFAULT_EXTENSION = "exe";

        Process process;
        StringBuilder standardError;
        StringBuilder standardOutput;

        #endregion

        #region Properties

        public string Arguments 
        { 
            get; 
            set; 
        }

        public ProcessStartInfo ProcessInfo 
        { 
            get; 
            set; 
        }

        public ProcessStartInfo DefaultProcessInfo
        {
            get
            {
                var info = new ProcessStartInfo();
                info.CreateNoWindow = true;
                info.RedirectStandardError = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.FileName = this.Path;
                return info;
            }
        }

        public override File.ContentFileType ContentType
        {
            get { return File.ContentFileType.Binary; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public bool IsExecuting
        {
            get
            {
                bool result = false;
                if (this.process != null)
                {
                    result = !this.process.HasExited;
                }
                return result;
            }
        }

        protected string Output
        {
            get
            {
                return this.standardOutput.ToString();
            }
        }

        protected string Error
        {
            get
            {
                return this.standardError.ToString();
            }
        }

        #endregion

        #region Events

        public delegate void ExecutionEventDelegate(object sender, ExecutionEventArgs args);

        /// <summary>
        /// Event when the execution has completed regardless of outcome
        /// </summary>
        public event ExecutionEventDelegate CompletionEvent;

        /// <summary>
        /// Progress event when any standard output and/or error is received
        /// from the process
        /// </summary>
        public event ExecutionEventDelegate OutputEvent;

        #endregion

        #region Constructors

        public Executable()
        {
            this.standardOutput = new StringBuilder();
            this.standardError = new StringBuilder();
        }

        public Executable(string path)
            : this()
        {
            this.Path = path;
        }

        public Executable(string path, string arguments)
            : this(path)
        {
            this.Arguments = arguments;
        }

        #endregion

        #region Methods

        public void Execute()
        {
            ValidatePath();
            CreateProcess();

            this.process.StartInfo.Arguments = this.Arguments;

            this.process.Start();

            this.process.BeginErrorReadLine();
            this.process.BeginOutputReadLine();
        }

        public void Stop()
        {
            if (this.process != null && 
                !this.process.HasExited)
            {
                this.process.Kill();
            }
        }

        public virtual void Wait()
        {
            this.process.WaitForExit();
        }

        ProcessStartInfo GetStartupInfo()
        {

            if (this.ProcessInfo == null)
            {
                return DefaultProcessInfo;
            }

            return ProcessInfo;
        }

        void ValidatePath()
        {
            if (string.IsNullOrEmpty(this.Path) ||
                !System.IO.File.Exists(this.Path))
            {
                Errors.FileNotFound(string.Format(
                    "This executable path {0} is not valid.",
                    this.Path));
            }
        }

        Process CreateProcess()
        {
            this.process = new Process 
            { 
                StartInfo = GetStartupInfo(),
                EnableRaisingEvents = true
            };

            this.process.Exited += process_Exited;

            this.process.OutputDataReceived += process_OutputDataReceived;
            this.process.ErrorDataReceived += process_ErrorDataReceived;

            return this.process;
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                this.standardError.AppendLine(e.Data);
                DispatchEvent(OutputEvent);
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.standardOutput.AppendLine(e.Data);
                DispatchEvent(OutputEvent);
            }
        }

        void process_Exited(object sender, EventArgs e)
        {
            DispatchEvent(
                CompletionEvent,
                this.standardOutput.ToString(),
                this.standardError.ToString(),
                true);
        }

        void DispatchEvent(
            ExecutionEventDelegate eventDelegate)
        {
            DispatchEvent(
                eventDelegate,
                this.standardOutput.ToString(),
                this.standardError.ToString());
        }

        void DispatchEvent(
            ExecutionEventDelegate eventDelegate,
            string standardOutput,
            string standardError)
        {
            DispatchEvent(eventDelegate, standardOutput, standardError, false);
        }

        void DispatchEvent(
            ExecutionEventDelegate eventDelegate, 
            string standardOutput, 
            string standardError, 
            bool isComplete)
        {
            var copy = eventDelegate;
            if (copy != null)
            {
                copy(this, new ExecutionEventArgs(standardOutput, standardError, isComplete));
            }
        }

        public void Dispose()
        {
            this.standardError.Clear();
            this.standardOutput.Clear();

            if (this.process != null)
            {
                this.process.ErrorDataReceived -= process_ErrorDataReceived;
                this.process.Exited -= process_Exited;
                this.process.OutputDataReceived -= process_OutputDataReceived;

                this.process.Dispose();
                this.process = null;
            }
        }

        #endregion
    }
}
