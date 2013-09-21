using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CMCoreNET;

namespace CodeAssistant.Domain
{
    public abstract class File
    {
        public enum ContentFileType
        { 
            Text,
            Binary,
            Image,
            Audio,
            Video
        }

        private byte[] contents;

        #region Constructors

        public File() { }
        
        public File(string path)
        {
            this.Path = path;
        }

        #endregion

        #region Properties

        public string Path { get; set; }
        public bool IsTemp { get; set; }

        public virtual byte[] Content 
        {
            get { return contents; }
            set { contents = value; }
        }

        public abstract ContentFileType ContentType { get; }
        public abstract bool IsReadOnly { get; }
        
        public int ContentLength
        {
            get
            { 
                return (this.contents == null) ? 0 : this.contents.Length;
            }
        }

        public virtual bool Exists
        {
            get { return System.IO.File.Exists(this.Path); }
        }

        #endregion

        public override string ToString()
        {
            return this.Path;
        }

        public virtual void Save()
        {
            if (!this.IsReadOnly && this.contents != null && this.contents.Length > 0)
            {
                System.IO.File.WriteAllBytes(this.Path, this.contents);
            }
        }

        public virtual void Open()
        {
            if (!string.IsNullOrEmpty(this.Path) && 
                System.IO.File.Exists(this.Path))
            {
                try
                {
                    this.contents = System.IO.File.ReadAllBytes(this.Path);
                }
                catch
                {
                    this.contents = null;
                }
            }
        }
    }
}
