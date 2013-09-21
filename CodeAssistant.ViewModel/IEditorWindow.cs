using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CodeAssistant.ViewModel
{
    public interface IEditorWindow
    {
        void DisplayArgumentsDropdown();
        void HideArgumentsDropdown();
        void SetArgumentsViewModel(ArgumentsViewModel viewModel);
        void RegisterKeyboardEvents(Action<object, KeyEventArgs> keyboardUpHandler, Action<object, KeyEventArgs> keyboardDownHandler);
        void BusyCursor();
        void NormalCursor();

        bool IsReadonly { get; set; }
        void Show();
        object DataContext { get; set; }
    }
}
