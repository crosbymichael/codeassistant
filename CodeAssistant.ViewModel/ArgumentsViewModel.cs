using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM_Async.ViewModel;
using System.Windows.Media.Animation;
using System.Windows;

namespace CodeAssistant.ViewModel
{
    public class ArgumentsEventArgs
    {
        public string Arguments
        {
            get;
            private set;
        }
        
        public ArgumentsEventArgs(string arguments)
        {
            this.Arguments = arguments;
        }
    }

    public class ArgumentsViewModel : ViewModelBase
    {
        #region Fields

        string argument;

        public event ArgumentsEventHandler SaveArgumentEvent;
        public delegate void ArgumentsEventHandler(object sender, ArgumentsEventArgs args);

        #endregion

        #region Properties

        public string Argument
        {
            get
            {
                return this.argument;
            }

            set
            {
                this.argument = value;
                OnChanged("Argument");
            }
        }

        public bool IsVisible
        {
            get;
            set;
        }

        #endregion

        public ArgumentsViewModel()
            : base()
        {
            this.argument = "Arguments...";
        }

        public void Save()
        {
            if (this.SaveArgumentEvent != null)
            {
                this.SaveArgumentEvent(this, new ArgumentsEventArgs(this.Argument));
            }
        }
    }
}
