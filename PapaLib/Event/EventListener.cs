using System;

namespace PapaLib.Event
{
    public abstract class BaseListener
    {
        protected abstract object Call(object[] args);
        public object Invoke(params object[] args)
        {
            return Call(args);
        }
        public abstract bool HasCallback(object callback);
    }

    public abstract class BaseListener<TCallback> : BaseListener where TCallback : Delegate
    {
        protected TCallback Callback { get; }

        protected BaseListener(TCallback callback)
        {
            this.Callback = callback;
        }
        public override bool HasCallback(object callback)
        {
            return Callback == (TCallback) callback;
        }
    }

    public sealed class EventListener : BaseListener<Action>
    {
        public EventListener(Action callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            Callback();
            return null;
        }
    }

    public sealed class EventListener<TArg> : BaseListener<Action<TArg>>
    {
        public EventListener(Action<TArg> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            Callback((TArg)args[0]);
            return null;
        }
    }

    public sealed class EventListener<TArg0, TArg1> : BaseListener<Action<TArg0, TArg1>>
    {
        public EventListener(Action<TArg0, TArg1> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            Callback((TArg0)args[0], (TArg1)args[1]);
            return null;
        }
    }

    public sealed class EventListener<TArg0, TArg1, TAgr2> : BaseListener<Action<TArg0, TArg1, TAgr2>>
    {
        public EventListener(Action<TArg0, TArg1, TAgr2> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            Callback((TArg0)args[0], (TArg1)args[1], (TAgr2)args[2]);
            return null;
        }
    }

    public sealed class EventListener<TArg0, TArg1, TArg2, TArg3> : BaseListener<Action<TArg0, TArg1, TArg2, TArg3>>
    {
        public EventListener(Action<TArg0, TArg1, TArg2, TArg3> callback) : base(callback) { }
        protected override object Call(object[] args)
        {
            Callback((TArg0)args[0], (TArg1)args[1], (TArg2)args[2], (TArg3)args[3]);
            return null;
        }
    }
}
