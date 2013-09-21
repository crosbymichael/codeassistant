using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CodeAssistant.Views;
using CodeAssistant.ViewModel;

namespace CodeAssistant.Factories
{
    public class EditorViewFactory : IWindowFactory
    {
        public IEditorWindow CreateEditorWindow(
            EditorViewModel viewModel)
        {
            var window = new EditorView();
            viewModel.SetWindow(window);
            return window;
        }
    }
}
