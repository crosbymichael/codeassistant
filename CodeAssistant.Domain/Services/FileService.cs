using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeAssistant.Domain.Services
{
    public class FileService
    {
        public string FileName
        {
            get;
            set;
        }
        IFileProvider provider;

        internal FileService(IFileProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("fileProvider");
            }

            this.provider = provider;
        }

        #region Methods

        public string LoadFromFile(string path)
        {
            return this.provider.ReadContents(path);
        }

        public bool SaveToFile(string contents)
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                return false;
            }
            this.provider.SaveContents(this.FileName, contents);

            return true;
        }

        public bool SaveToFile(string path, string contents)
        {
            this.FileName = path;
            this.provider.SaveContents(path, contents);
            return true;
        }

        #endregion
    }
}
