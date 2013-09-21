using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAssistant.ViewModel
{
    public interface IWindowFactory
    {
        IEditorWindow CreateEditorWindow(ViewModel.EditorViewModel viewModel);
    }
}
