using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Lette.Wpf.AppCommands.Internals;
using Microsoft.Xaml.Behaviors;

namespace Lette.Wpf.AppCommands
{
    public class AppCommandBinding : Behavior<System.Windows.Window>
    {
        public static readonly DependencyProperty AppCommandProperty =
            DependencyProperty.Register(
                nameof(AppCommand),
                typeof(AppCommand),
                typeof(AppCommandBinding));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(AppCommandBinding));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(AppCommandBinding),
                new PropertyMetadata(""));

        public AppCommand AppCommand
        {
            get => (AppCommand)GetValue(AppCommandProperty);
            set => SetValue(AppCommandProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private HwndSource _hwndSource;
        private HwndSourceHook _hwndSourceHook;

        protected override void OnAttached()
        {
            AssociatedObject.SourceInitialized += (s, e) =>
            {
                _hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(AssociatedObject).Handle);
                _hwndSourceHook = WndProc;

                _hwndSource?.AddHook(_hwndSourceHook);
            };
        }

        protected override void OnDetaching()
        {
            _hwndSource?.RemoveHook(_hwndSourceHook);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == NativeMethods.WM_APPCOMMAND)
            {
                var appCommand = NativeMethods.GetAppCommand(lparam);

                if (appCommand == (int)AppCommand)
                {
                    var command = Command;
                    if (command?.CanExecute(CommandParameter) ?? false)
                    {
                        command.Execute(CommandParameter);
                        handled = true;
                    }
                }
            }

            return IntPtr.Zero;
        }
    }
}
