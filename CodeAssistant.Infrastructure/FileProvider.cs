using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain;
using System.IO;

namespace CodeAssistant.Infrastructure
{
    public class FileProvider : IFileProvider
    {
        #region Singleton

        public static FileProvider Provider
        {
            get;
            private set;
        }

        static FileProvider()
        {
            if (Provider == null)
            {
                Provider = new FileProvider();
            }
        }

        #endregion

        const string TEMP_DIRECTORY_NAME = "temp";
        const string DATA_DIRECTORY_NAME = "Data";
        const string SYNTAX_DIRECTORY_NAME = "Syntax";

        string baseFilePath;

        public string DataPath
        {
            get { return Path.Combine(this.baseFilePath, DATA_DIRECTORY_NAME); }
        }

        public string SyntaxPath
        {
            get { return Path.Combine(this.baseFilePath, SYNTAX_DIRECTORY_NAME); }
        }

        public string TempDirectory
        {
            get { return Path.Combine(this.baseFilePath, TEMP_DIRECTORY_NAME); }
        }

        public Encoding Encoder
        {
            get { return Encoding.UTF8; }
        }

        FileProvider()
        {
            this.baseFilePath = Environment.CurrentDirectory;
            CreateTempDirectory();
        }

        public string GetTempFileName()
        {
            return Path.GetRandomFileName()
                .Split('.')
                .ElementAt(0);
        }

        public string GetTempFileName(string extension)
        {
            return string.Format(
                "{0}.{1}",
                GetTempFileName(),
                extension);
        }

        public string ReadContents(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public void SaveContents(string path, string contents)
        {
            System.IO.File.WriteAllText(path, contents);
        }

        public IEnumerable<string> FetchDirectoryContents(string path)
        {
            return Directory.GetFiles(path);
        }

        public string GetTempFilePath()
        {
            return Path.Combine(
                this.TempDirectory,
                GetTempFileName());
        }

        public string GetTempFilePath(string extension)
        {
            return Path.Combine(
                this.TempDirectory,
                GetTempFileName(extension));
        }

        void CreateTempDirectory()
        {
            Directory.CreateDirectory(this.TempDirectory);
        } 
    }
}
