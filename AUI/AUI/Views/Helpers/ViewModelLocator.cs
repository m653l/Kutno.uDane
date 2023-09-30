using Avalonia.Controls;
using Avalonia;
using System.Linq;

namespace AUI.Views.Helpers
{
    public class ViewModelLocator
    {
        public static bool GetAutoWireViewModel(AvaloniaObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(AvaloniaObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        public static readonly AttachedProperty<bool> AutoWireViewModelProperty =
            AvaloniaProperty.RegisterAttached<ViewModelLocator, AvaloniaObject, bool>("AutoWireViewModel", false,
                coerce: OnAutoWireViewModelChanged);

        private static bool OnAutoWireViewModelChanged(AvaloniaObject d, bool e)
        {
            if (Design.IsDesignMode)
            {
                return false;
            }

            string viewType = d.GetType().Name;
            string viewModelTypeName = viewType + "Model";
            System.Reflection.Assembly assembly = typeof(Application.DependencyInjection).Assembly;
            System.Type? viewModelType = assembly.GetTypes().FirstOrDefault(t => t.FullName.Contains(viewModelTypeName));
            if (viewModelType is null)
            {
                return false;
            }

            var viewModel = App.Current.Services.GetService(viewModelType);
            ((Control)d).DataContext = viewModel;

            return true;
        }
    }
}
