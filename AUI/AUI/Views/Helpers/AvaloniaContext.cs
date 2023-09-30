using Application.ViewModels.Helpers;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace AUI.Views.Helpers
{
    public sealed class AvaloniaContext : IUIContext
    {
        private readonly Dispatcher _dispatcher;

        public bool IsSynchronized => Dispatcher.UIThread.CheckAccess();

        public AvaloniaContext() : this(Dispatcher.UIThread)
        {
        }

        public AvaloniaContext(Dispatcher dispatcher)
        {
            Debug.Assert(dispatcher != null);
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            Debug.Assert(action != null);
            _dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            Debug.Assert(action != null);
            Dispatcher.UIThread.Post(action);
        }

        public void SetViewContext(object view, object context)
        {
            ((Control)view).DataContext = context;
        }
    }
}
