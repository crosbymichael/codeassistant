using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace CodeAssistant.Resources
{
	//Singleton for preferences
    [Serializable]
	public class Preferences
	{
        private static object locker = new object();
        //Paths get set on init then do not change
        private string tempDir;
        private string langDef;
        private string syntaxDir;

		#region Properties
		//Editor color scheme
		public string ColorScheme { get; set; }

		//Path that the code files are written or compiled to
		public string Path { get; set; }
        //Default int value of 0 is fine is none is set
        public int SelectedItem { get; set; }
		//Reference Path
		public string ReferencePath { get; set; }

        [XmlIgnore]
        public string SyntaxDirectory { get { return syntaxDir; } }
		
        [XmlIgnore]
        public string LanguageDefintionPath { get { return langDef; } }

        public string ExecutableName { get; set; }

        [XmlIgnore]
        public string TempDirectoryPath { get { return tempDir; } }

        public string FileName { get; set; }

        public string ExecutableExtension { get; set; }

        public string LanguageResourceExtension { get; set; }

        public string CanSendErrorReports { get; set; }

        #endregion

        public static Preferences Instance
        {
            get;
            private set;
        }

        static Preferences()
        {
            if (Instance == null)
                Instance = new Preferences();
            Instance.CreateTempDirectory();
        }


        private void CreateTempDirectory()
        {
            this.tempDir = Environment.CurrentDirectory + "\\Temp";
            this.langDef = string.Format("{0}\\Data\\", Environment.CurrentDirectory);
            this.syntaxDir = string.Format("{0}\\Syntax\\", Environment.CurrentDirectory);
            DirectoryInfo info =
                new DirectoryInfo(this.tempDir);
            if (!info.Exists)
            {
                info.Create();
            }
        }
	}
}

