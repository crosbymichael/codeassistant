using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using MVVM_Async.ViewModel;
using CodeAssistant.Domain.Development;
using CodeAssistant.Domain.Services;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using CodeAssistant.Domain.Factory;
using CodeAssistant.Domain.Languages;
using CodeAssistant.Domain.Execution;

namespace CodeAssistant.ViewModel
{
    public class EditorViewModel : ViewModelBase
    {
        #region Fields

        ExecutionService executionService;
        FileService fileService;
        LanguageService languageService;
        SourceCodeFactory sourceCodeFactory;
        IEditorWindow parentWindow;

        ArgumentsViewModel argumentsViewModel;

        #endregion

        #region Events

        public event ViewModelEventDelegate NewEditorEvent;

        #region IKeyboardEventDestination Members

        public void KeyDownEventDelegate(object sender, KeyEventArgs args)
        {
            if (args.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                Action func = () => { (sender as IEditorWindow).IsReadonly = true; };
                switch (args.Key)   //TODO: Shitty
                {
                    case Key.O:
                        func();
                        Open();
                        break;
                    case Key.R:
                        func();
                        Execute();
                        break;
                    case Key.S:
                        func();
                        Save(args.KeyboardDevice.Modifiers == ModifierKeys.Shift);
                        break;
                    case Key.N:
                        func();
                        NewEditor();
                        break;
                    case Key.F:
                        func();
                        LocateResource();
                        break;
                    case Key.G:
                        func();
                        ShowArguments();
                        break;
                    case Key.Q:
                        func();
                        ExitCommand();
                        break;
                    default:
                        break;
                }
            }
            else if (args.Key == Key.Escape &&
                this.argumentsViewModel.IsVisible)
            {
                ShowArguments();
            }
        }

        public void KeyUpEventDelegate(object sender, KeyEventArgs args)
        {
            (sender as IEditorWindow).IsReadonly = false;
        }

        void ResourceLocated(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            Dispatch(() =>
            {
                this.languageService.UpdateLanguageResource(
                    this.Languages.ElementAt(this.LanguageIndex),
                    ((OpenFileDialog)sender).FileName);
            });
        }

        void FileOpenSelected(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((FileDialog)sender).FileOk -= FileOpenSelected;

            if (e.Cancel)
            {
                return;
            }

            var view = sender as OpenFileDialog;
            if (view != null)
            {
                Dispatch(() =>
                {
                    string contents = this.fileService.LoadFromFile(view.FileName);
                    this.fileService.FileName = view.FileName;

                    DispatchMain(() => 
                    {
                        Code.Text = contents;
                        OnChanged("Code");
                        
                    });
                });
            }
        }

        void FileSaveSelected(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((FileDialog)sender).FileOk -= FileSaveSelected;

            if (e.Cancel)
            {
                return;
            }

            var saveview = sender as SaveFileDialog;
            if (saveview != null)
            {
                string contents = Code.Text;

                Dispatch(() =>
                {
                    this.fileService.SaveToFile(
                        saveview.FileName,
                        contents);
                });
            }
        }

        void ExecutionEvent(object sender, ExecutionEvent args)
        {
            if (!string.IsNullOrEmpty(args.Output) || 
                !string.IsNullOrEmpty(args.Error))
            {
                DispatchMain(() => SetOutput(string.Join(
                    Environment.NewLine,
                    args.Output,
                    args.Error)));
            }

            if (args.IsComplete)
            {
                DispatchMain(this.parentWindow.NormalCursor);
            }
        }

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<string> Languages { get; set; }

        TextDocument document;
        public TextDocument Code
        {
            get
            {
                if (document == null)
                {
                    document = new TextDocument();
                }
                return document;
            }
            set
            {
                document = value;
            }
        }

        IHighlightingDefinition syntaxDefinition;
        public IHighlightingDefinition Syntax
        {
            get
            {
                return syntaxDefinition;
            }
            set
            {
                syntaxDefinition = value;
                OnChanged("Syntax");
            }
        }

        string selectedCode;
        public string SelectedCode
        {
            get { return selectedCode; }
            set
            {
                selectedCode = value;
                OnChanged("SelectedCode");
            }
        }

        int languageIndex;
        public int LanguageIndex
        {
            get { return languageIndex; }
            set
            {
                languageIndex = value;
                syntaxDefinition = SyntaxProvider.GetDefinition(Language);
                Code.Text = Language.Template;//Remove call
                OnChanged("Code");
                OnChanged("Language");
            }
        }

        StringBuilder output;
        public string Output
        {
            get 
            {
                return output.ToString();
            }
            set
            {
                throw new NotSupportedException("Output set not supported, juse 'SetOutput' method");
            }
        }

        public string Arguments
        {
            get
            {
                return this.argumentsViewModel.Argument;
            }

            set
            {
                this.argumentsViewModel.Argument = value;
                OnChanged("Argument");
            }
        }

        public LanguageBase Language
        {
            get
            { 
                return this.languageService.GetLanguage(
                    Languages.ElementAt(languageIndex));
            }
        }

        #endregion

        #region Methods

        public void Execute()
        {
            string code = Code.Text;
            SetOutput(string.Empty);
            this.parentWindow.BusyCursor();

            Dispatch(() => 
            {
                try
                {
                    var sourceCode = this.sourceCodeFactory.GetSourceCode(
                        Language, 
                        code);

                    string arguments = null;
                    if (!string.IsNullOrEmpty(this.Arguments) && 
                        this.Arguments != "Arguments...")
                    {
                        arguments = this.Arguments;
                    }

                    this.executionService.ExecuteCode(sourceCode, arguments);
                }
                catch (Domain.DomainException ex)
                {
                    SetOutput(ex.Error);
                }
            });
        }

        public void Stop()
        {
            this.executionService.Stop();
        }

        public void Save()
        {
            Save(null);
        }

        public void Save(bool? saveAs)
        {
            if (string.IsNullOrEmpty(this.fileService.FileName) || 
                (saveAs.HasValue && saveAs.Value))
            {
                SaveFileDialog view = new SaveFileDialog();
                var lang = Language;
                view.Title = "Save Code to File";
                view.FileOk += new System.ComponentModel.CancelEventHandler(FileSaveSelected);
                view.Filter = string.Format(
                    "{0} code|*.{1}",
                    lang.Name,
                    lang.Extension);
                view.OverwritePrompt = true;
                view.ShowDialog();
            }
            else
            {
                this.fileService.SaveToFile(this.Code.Text);
            }
        }

        /// <summary>
        /// Open code from a file
        /// </summary>
        public void Open()
        {
            OpenFileDialog view = new OpenFileDialog();
            var lang = Language;
            view.FileOk += new System.ComponentModel.CancelEventHandler(FileOpenSelected);
            view.Title = "Open Code File";
            view.Filter = string.Format(
                "{0} code|*.{1}",
                lang.Name,
                lang.Extension);
            view.Multiselect = false;
            view.ShowDialog();
        }

        public void SetWindow(IEditorWindow editorWindow)
        {
            this.parentWindow = editorWindow;
            editorWindow.DataContext = this;

            this.parentWindow.RegisterKeyboardEvents(
                this.KeyUpEventDelegate, 
                this.KeyDownEventDelegate);

            this.parentWindow.SetArgumentsViewModel(argumentsViewModel);
        }

        void LocateResource()
        {
            OpenFileDialog view = new OpenFileDialog();
            view.FileOk += new System.ComponentModel.CancelEventHandler(ResourceLocated);
            view.Title = "Locate Compiler or Runtime";
            view.Filter = "exe|*.exe";
            view.Multiselect = false;
            view.ShowDialog();
        }

        public void NewEditor()
        {
            if (this.NewEditorEvent != null)
            {
                NewEditorEvent(this, new ViewModelEventArgs());
            }
        }

        void ExitCommand()
        {
            ClosingEvent();
        }

        void ShowArguments()
        {
            this.parentWindow.DisplayArgumentsDropdown();
        }

        void SetOutput(string line)
        {
            this.output.Clear();
            this.output.Append(line);

            OnChanged("Output");
        }

        #endregion

        #region Constructors

        public EditorViewModel(Domain.ILanguageRepository repository)
        {
            this.output = new StringBuilder();
            this.Languages = new ObservableCollection<string>();
            this.argumentsViewModel = new ArgumentsViewModel();

            var fileProvider = (repository as Infrastructure.LanguageRepository).FileProvider;
            var factory = new ServiceFactory(fileProvider, repository);

            this.fileService = factory.GetFileService();
            this.languageService = factory.GetLanguageService();
            this.executionService = factory.GetExecutionService();
            this.sourceCodeFactory = new SourceCodeFactory(fileProvider);

            this.executionService.ExecutionDataEvent += new Domain.DomainExecutionDelegate(ExecutionEvent);
        }

        public void Init()
        {
            if (!Languages.Any())
            {
                foreach (string name in this.languageService.LanguageNames)
                {
                    Languages.Add(name);
                }
            }
            LanguageIndex = 0;
        }

        #endregion
    }
}
