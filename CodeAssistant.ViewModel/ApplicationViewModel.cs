using System;
using CodeAssistant.Domain.Development;
using System.Configuration;
using System.Windows.Threading;
using CodeAssistant.Domain.Factory;
using MVVM_Async.ViewModel;

namespace CodeAssistant.ViewModel
{
    public class ApplicationViewModel 
    {
        IWindowFactory factory;
        Domain.ILanguageRepository repository;

        public ApplicationViewModel(IWindowFactory factory, Action shutdown)
        {
            this.factory = factory;
            this.shutdown = shutdown;
            this.repository = Infrastructure.LanguageRepository.Repository;
        }

        Action shutdown { get; set; }

        string ApplicationName 
        {
            get { return ConfigurationManager.AppSettings["Name"]; } 
        }

        string ApplicationCode
        {
            get { return ConfigurationManager.AppSettings["AppCode"]; }
        }

        public void Start()
        {
            LaunchNewEditorViewModel();         
        }

        void ViewModelClosingEvent(object sender, ViewModelEventArgs args)
        { 
            
        }

        #region Events

        void NewEditorEvent(object sender, ViewModelEventArgs args)
        {
            LaunchNewEditorViewModel();
        }

        #endregion

        void LaunchNewEditorViewModel()
        {
            var viewModel = new EditorViewModel(this.repository);
            viewModel.NewEditorEvent += new ViewModelEventDelegate(NewEditorEvent);
            viewModel.ViewModelClosingEvent += new ViewModelEventDelegate(ViewModelClosingEvent);

            var window = this.factory.CreateEditorWindow(viewModel);

            viewModel.Init();
            window.Show();
        }

        public void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //try
            //{
                
            //    var api = new ApiRequest();
            //    var response = api.Post(
            //            "Application/Error",
            //            new ErrorModel
            //            {
            //                Application = ConfigurationManager.AppSettings["AppCode"],
            //                Type = ErrorType.Exception,
            //                Message = "See elmah",
            //                Exception = e.Exception
            //            },
            //            null,
            //            ApiRequest.ContentType.JSON,
            //            true);
                
            //}
            //catch 
            //{ 
            //    //Just rollover and die
            //    Shutdown();    
            //}
        }
    }
}
