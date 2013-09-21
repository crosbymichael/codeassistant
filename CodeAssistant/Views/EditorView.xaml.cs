using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using CodeAssistant.ViewModel;
using CodeAssistant.Animation;

namespace CodeAssistant.Views
{
    /// <summary>
    /// The cleanest code behind ever!
    /// </summary>
    public partial class EditorView : Window, IEditorWindow
    {
        DropdownAnimation argumentsAnimation;
        DropdownAnimation controlsAnimation;
        public EditorView()
        {
            InitializeComponent();

            this.argumentsAnimation = new DropdownAnimation(
                this, 
                this.argumentsControl,
                0,
                42,
                TimeSpan.FromSeconds(0.40));

            this.controlsAnimation = new DropdownAnimation(
                this,
                this.controlsPanel,
                42,
                0,
                TimeSpan.FromSeconds(0.40));
            
            this.controlsAnimation.Toggle = true;
            this.argumentsAnimation.Toggle = true;
        }

        public void DisplayArgumentsDropdown()
        {
            this.argumentsAnimation.Begin();
            this.controlsAnimation.Begin();
            (this.argumentsControl.DataContext as ArgumentsViewModel).IsVisible = true;
        }

        public void HideArgumentsDropdown()
        {
            this.argumentsAnimation.Begin();
            this.controlsAnimation.Begin();
            (this.argumentsControl.DataContext as ArgumentsViewModel).IsVisible = false;
        }

        public void BusyCursor()
        {
            this.Cursor = Cursors.Wait;
        }

        public void NormalCursor()
        {
            this.Cursor = Cursors.Arrow;
        }

        //Should be transformed into a property bound to the main datacontext
        public void SetArgumentsViewModel(
            ArgumentsViewModel viewModel)
        {
            this.argumentsControl.DataContext = viewModel;
            viewModel.SaveArgumentEvent += new ArgumentsViewModel.ArgumentsEventHandler(viewModel_SaveArgumentEvent);
        }

        void viewModel_SaveArgumentEvent(object sender, ArgumentsEventArgs args)
        {
            HideArgumentsDropdown();
        }


        public bool IsReadonly
        {
            get
            {
                return this.codeEditor.IsReadOnly;
            }
            set
            {
                this.codeEditor.IsReadOnly = value;
            }
        }

        public void RegisterKeyboardEvents(
            Action<object, KeyEventArgs> keyboardUpHandler, 
            Action<object, KeyEventArgs> keyboardDownHandler)
        {
            this.KeyDown += new KeyEventHandler(keyboardDownHandler);
            this.KeyUp += new KeyEventHandler(keyboardUpHandler);
        }
    }
}
