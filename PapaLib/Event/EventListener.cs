using System;
using System.Collections.Generic;

namespace PapaLib.Event
{
    public abstract class BaseListener
    {
        protected abstract object Call(object[] args);
        public object Invoke(params object[] args)
        {
            return Call(args);
        }
        public abstract bool hasCallback(object callback);
    }

    public abstract class BaseListener<TCallback> : BaseListener where TCallback : Delegate
    {
        public TCallback callback { get; }
        public BaseListener(TCallback callback)
        {
            this.callback = callback;
        }
        public override bool hasCallback(object callback)
        {
            return this.callback == callback;
        }
    }

    public sealed class EventListener : BaseListener<Action>
    {
        public EventListener(Action callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            callback();
            return null;
        }
    }

    public sealed class EventListener<T> : BaseListener<Action<T>>
    {
        public EventListener(Action<T> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            callback((T)args[0]);
            return null;
        }
    }

    public sealed class EventListener<T, U> : BaseListener<Action<T, U>>
    {
        public EventListener(Action<T, U> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            callback((T)args[0], (U)args[1]);
            return null;
        }
    }

    public sealed class EventListener<T, U, V> : BaseListener<Action<T, U, V>>
    {
        public EventListener(Action<T, U, V> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            callback((T)args[0], (U)args[1], (V)args[2]);
            return null;
        }
    }

    public sealed class EventListener<T, U, V, W> : BaseListener<Action<T, U, V, W>>
    {
        public EventListener(Action<T, U, V, W> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            callback((T)args[0], (U)args[1], (V)args[2], (W)args[3]);
            return null;
        }
    }
}
