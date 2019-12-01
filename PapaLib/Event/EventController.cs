using System;
using System.Collections.Generic;

namespace PapaLib.Event
{
    public class EventController
    {
        private readonly Dictionary<string, List<BaseListener>> _eventListenersDic;

        public EventController()
        {
            _eventListenersDic = new Dictionary<string, List<BaseListener>>();
        }

        #region public methods
        public void AddEventListener(string eventType, Action eventListener)
        {
            EventTypeCheck(eventType);
            _eventListenersDic[eventType].Add(new EventListener(eventListener));
        }

        public void AddEventListener<TArg0>(string eventType, Action<TArg0> eventListener)
        {
            EventTypeCheck(eventType);
            _eventListenersDic[eventType].Add(new EventListener<TArg0>(eventListener));
        }

        public void AddEventListener<TArg0, TArg1>(string eventType, Action<TArg0, TArg1> eventListener)
        {
            EventTypeCheck(eventType);
            _eventListenersDic[eventType].Add(new EventListener<TArg0, TArg1>(eventListener));
        }

        public void AddEventListener<TArg0, TArg1, TArg2>(string eventType, Action<TArg0, TArg1, TArg2> eventListener)
        {
            EventTypeCheck(eventType);
            _eventListenersDic[eventType].Add(new EventListener<TArg0, TArg1, TArg2>(eventListener));
        }

        public void AddEventListener<TArg0, TArg1, TArg2, TArg3>(string eventType, Action<TArg0, TArg1, TArg2, TArg3> eventListener)
        {
            EventTypeCheck(eventType);
            _eventListenersDic[eventType].Add(new EventListener<TArg0, TArg1, TArg2, TArg3>(eventListener));
        }

        public void RemoveEventListener(string eventType, Action eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<TArg0>(string eventType, Action<TArg0> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<TArg0, TArg1>(string eventType, Action<TArg0, TArg1> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<TArg0, TArg1, TArg2>(string eventType, Action<TArg0, TArg1, TArg2> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void RemoveEventListener<TArg0, TArg1, TArg2, TArg3>(string eventType, Action<TArg0, TArg1, TArg2, TArg3> eventListener)
        {
            RemoveCallback(eventType, eventListener);
        }

        public void DispatchEvent(string eventType)
        {
            DispatchEvent<EventListener>(eventType);
        }

        public void DispatchEvent<TArg0>(string eventType, TArg0 arg)
        {
            DispatchEvent<EventListener<TArg0>>(eventType, arg);
        }

        public void DispatchEvent<TArg0, TArg1>(string eventType, TArg0 arg0, TArg1 arg1)
        {
            DispatchEvent<EventListener<TArg0, TArg1>>(eventType, arg0, arg1);
        }

        public void DispatchEvent<TArg0, TArg1, TArg2>(string eventType, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            DispatchEvent<EventListener<TArg0, TArg1, TArg2>>(eventType, arg0, arg1, arg2);
        }

        public void DispatchEvent<TArg0, TArg1, TArg2, TArg3>(string eventType, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            DispatchEvent<EventListener<TArg0, TArg1, TArg2, TArg3>>(eventType, arg0, arg1, arg2, arg3);
        }
        #endregion

        #region private methods
        private void EventTypeCheck(string eventType)
        {
            if (!_eventListenersDic.ContainsKey(eventType))
            {
                _eventListenersDic[eventType] = new List<BaseListener>();
            }
        }

        private void RemoveCallback<TCallback>(string eventType, TCallback callBack) where TCallback : Delegate
        {
            if (!_eventListenersDic.TryGetValue(eventType, out var eventListeners)) return;
            var eventCount = eventListeners.Count;
            for (var i = 0; i < eventCount; i++)
            {
                if (!eventListeners[i].HasCallback(callBack)) continue;
                eventListeners.RemoveAt(i);
                break;
            }
        }

        private void DispatchEvent<TEventListener>(string eventType, params object[] args) where TEventListener : BaseListener
        {
            if (!_eventListenersDic.TryGetValue(eventType, out var eventListeners)) return;
            var eventCount = eventListeners.Count;
            for (var i = 0; i < eventCount; i++)
            {
                if (eventListeners[i] is TEventListener)
                {
                    eventListeners[i].Invoke(args);
                }
            }
        }
        #endregion
    }
}
