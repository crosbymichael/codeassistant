using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.Domain
{
    /// <summary>
    /// Intended to provide an IO file layer
    /// </summary>
    public interface IFileProvider
    {
        string GetTempFileName();
        string GetTempFileName(string extension);

        string GetTempFilePath();
        string GetTempFilePath(string extension);

        string ReadContents(string path);
        void SaveContents(string path, string contents);
    }
}
