using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Lette.Wpf.AppCommands.Internals
{
    public abstract class AttachableCollection<T> :
        FreezableCollection<T>,
        IAttachedObject where T : DependencyObject, IAttachedObject
    {
        private Collection<T> _snapshot;
        private DependencyObject _associatedObject;

        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return _associatedObject;
            }
        }

        internal AttachableCollection()
        {
            var notifyCollectionChanged = (INotifyCollectionChanged)this;
            notifyCollectionChanged.CollectionChanged += OnCollectionChanged;

            _snapshot = new Collection<T>();
        }

        protected abstract void OnAttached();

        protected abstract void OnDetaching();

        protected abstract void ItemAdded(T item);

        protected abstract void ItemRemoved(T item);

        [Conditional("DEBUG")]
        private void VerifySnapshotIntegrity()
        {
            var isValid = (Count == _snapshot.Count);
            if (isValid)
            {
                for (var i = 0; i < Count; i++)
                {
                    if (!ReferenceEquals(this[i], _snapshot[i]))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            Debug.Assert(isValid, "ReferentialCollection integrity has been compromised.");
        }

        private void VerifyAdd(T item)
        {
            if (_snapshot.Contains(item))
            {
                throw new InvalidOperationException("Duplicate item in collection.");
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                // We fix the snapshot to mirror the structure, even if an exception is thrown. This may not be desirable.
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        try
                        {
                            VerifyAdd(item);
                            ItemAdded(item);
                        }
                        finally
                        {
                            _snapshot.Insert(IndexOf(item), item);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems)
                    {
                        ItemRemoved(item);
                        _snapshot.Remove(item);
                    }

                    foreach (T item in e.NewItems)
                    {
                        try
                        {
                            VerifyAdd(item);
                            ItemAdded(item);
                        }
                        finally
                        {
                            _snapshot.Insert(IndexOf(item), item);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems)
                    {
                        ItemRemoved(item);
                        _snapshot.Remove(item);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _snapshot)
                    {
                        ItemRemoved(item);
                    }

                    _snapshot = new Collection<T>();
                    foreach (var item in this)
                    {
                        VerifyAdd(item);
                        ItemAdded(item);
                    }

                    break;

                default:
                    Debug.Fail("Unsupported collection operation attempted.");
                    break;
            }
            #if DEBUG
            VerifySnapshotIntegrity();
            #endif
        }

        DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject != AssociatedObject)
            {
                if (AssociatedObject != null)
                {
                    throw new InvalidOperationException();
                }

                if (!(bool)GetValue(DesignerProperties.IsInDesignModeProperty))
                {
                    WritePreamble();
                    _associatedObject = dependencyObject;
                    WritePostscript();
                }

                OnAttached();
            }
        }

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            _associatedObject = null;
            WritePostscript();
        }
    }
}
