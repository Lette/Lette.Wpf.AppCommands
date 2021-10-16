using System;
using System.Windows;
using Lette.Wpf.AppCommands.Internals;
using Microsoft.Xaml.Behaviors;

namespace Lette.Wpf.AppCommands
{
    public static class Window
    {
        public static readonly DependencyProperty AppCommandBindingsProperty =
            DependencyProperty.RegisterAttached(
                "ShadowAppCommandBindings",
                typeof(AppCommandBindingCollection),
                typeof(Window),
                new FrameworkPropertyMetadata(OnBindingsChanged));

        public static AppCommandBindingCollection GetAppCommandBindings(DependencyObject dep)
        {
            var bindings = (AppCommandBindingCollection)dep.GetValue(AppCommandBindingsProperty);

            if (bindings == null)
            {
                bindings = new AppCommandBindingCollection();
                dep.SetValue(AppCommandBindingsProperty, bindings);
            }

            return bindings;
        }

        private static void OnBindingsChanged(DependencyObject dep, DependencyPropertyChangedEventArgs args)
        {
            var oldCollection = (IAttachedObject)args.OldValue;
            var newCollection = (IAttachedObject)args.NewValue;

            if (oldCollection != newCollection)
            {
                if (oldCollection?.AssociatedObject != null)
                {
                    oldCollection.Detach();
                }

                if (newCollection != null && dep != null)
                {
                    if (newCollection.AssociatedObject != null)
                    {
                        throw new InvalidOperationException("Cannot host bindings collection multiple times.");
                    }

                    newCollection.Attach(dep);
                }
            }
        }
    }
}
