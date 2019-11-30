using System;
using System.Collections.Generic;

namespace PapaLib.Event
{
    public class EventController
    {
        private Dictionary<string, List<BaseListener>> eventListenersDic;

        public EventController()
        {
            eventListenersDic = new Dictionary<string, List<BaseListener>>();
        }

        #region public methods
        public void AddEventListener(string eventType, Action eventListener)
        {
            EventTypeCheck(eventType);
            eventListenersDic[eventType].Add(new EventListener(eventListener));
        }

        public void AddEventListener<T>(string eventType, Action<T> eventListener)
        {
            EventTypeCheck(eventType);
            eventListenersDic[eventType].Add(new EventListener<T>(eventListener));
        }

        public void AddEventListener<T, U>(string eventType, Action<T, U> eventListener)
        {
            EventTypeCheck(eventType);
            eventListenersDic[eventType].Add(new EventListener<T, U>(eventListener));
        }

        public void AddEventListener<T, U, V>(string eventType, Action<T, U, V> eventListener)
        {
            EventTypeCheck(eventType);
            eventListenersDic[eventType].Add(new EventListener<T, U, V>(eventListener));
        }

        public void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> eventListener)
        {
            EventTypeCheck(eventType);
            eventListenersDic[eventType].Add(new EventListener<T, U, V, W>(eventListener));
        }

        public void RemoveEventListener(string eventType, Action eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<T>(string eventType, Action<T> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<T, U>(string eventType, Action<T, U> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void DispatchEvent(string eventType)
        {
            DispatchEvent<EventListener>(eventType);
        }

        public void DispatchEvent<T>(string eventType, T arg)
        {
            DispatchEvent<EventListener<T>>(eventType, arg);
        }

        public void DispatchEvent<T, U>(string eventType, T arg0, U arg1)
        {
            DispatchEvent<EventListener<T, U>>(eventType, arg0, arg1);
        }

        public void DispatchEvent<T, U, V>(string eventType, T arg0, U arg1, V arg2)
        {
            DispatchEvent<EventListener<T, U, V>>(eventType, arg0, arg1, arg2);
        }

        public void DispatchEvent<T, U, V, W>(string eventType, T arg0, U arg1, V arg2, W arg3)
        {
            DispatchEvent<EventListener<T, U, V, W>>(eventType, arg0, arg1, arg2, arg3);
        }
        #endregion

        #region private methods
        private void EventTypeCheck(string eventType)
        {
            if (!eventListenersDic.ContainsKey(eventType))
            {
                eventListenersDic[eventType] = new List<BaseListener>();
            }
        }

        private void RemoveCallback<TCallback>(string eventType, TCallback callBack) where TCallback : Delegate
        {
            if (eventListenersDic.TryGetValue(eventType, out var eventListeners))
            {
                var eventCount = eventListeners.Count;
                for (int i = 0; i < eventCount; i++)
                {
                    if (eventListeners[i].hasCallback(callBack))
                    {
                        eventListeners.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void DispatchEvent<TEventListener>(string eventType, params object[] args) where TEventListener : BaseListener
        {
            if (eventListenersDic.TryGetValue(eventType, out var eventListeners))
            {
                var eventCount = eventListeners.Count;
                for (var i = 0; i < eventCount; i++)
                {
                    if (eventListeners[i] is TEventListener)
                    {
                        eventListeners[i].Invoke(args);
                    }
                }
            }
        }
        #endregion
    }
}
