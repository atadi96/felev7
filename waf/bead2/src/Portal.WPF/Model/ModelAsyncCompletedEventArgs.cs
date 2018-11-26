using System;
using System.ComponentModel;

namespace Portal.WPF.Model
{
    public class ModelAsyncCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        ModelAsyncCompletedEventArgs(Exception ex, bool cancelled) : base(ex, cancelled, null)
        { }

        ModelAsyncCompletedEventArgs(T result) : base(null, false, null)
        {
            _data = result;
        }

        private T _data;

        public T Data
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _data;
            }
        }
    }
}
