using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Lette.Wpf.AppCommands.Internals
{
    public sealed class AppCommandBindingCollection : AttachableCollection<Behavior>
    {
        protected override void OnAttached()
        {
            foreach (var behavior in this)
            {
                behavior.Attach(AssociatedObject);
            }
        }

        protected override void OnDetaching()
        {
            foreach (var behavior in this)
            {
                behavior.Detach();
            }
        }

        protected override void ItemAdded(Behavior item)
        {
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
        }

        protected override void ItemRemoved(Behavior item)
        {
            if (((IAttachedObject)item).AssociatedObject != null)
            {
                item.Detach();
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new AppCommandBindingCollection();
        }
    }
}
