using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Managers
{
    public class EventManager : MonoBehaviour
    {
        private static Dictionary<string, dynamic> _eventDictionary = new Dictionary<string, dynamic>();


        public static void Listen(string eventName, Action method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                Action eventToUse = _eventDictionary[eventName];
                eventToUse += method;
                _eventDictionary[eventName] = eventToUse;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }

        public static void Listen<T>(string eventName, Action<T> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                Action<T> eventToUse = _eventDictionary[eventName];
                eventToUse += method;
                _eventDictionary[eventName] = eventToUse;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }

        public static void Listen<T, Q>(string eventName, Action<T, Q> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                Action<T, Q> eventToUse = _eventDictionary[eventName];
                eventToUse += method;
                _eventDictionary[eventName] = eventToUse;
            }
            else
            {
                _eventDictionary.Add(eventName, method);
            }
        }

        public static void Fire(string eventName)
        {
            var raiseEvent = _eventDictionary[eventName] as Action;
            raiseEvent?.Invoke();
        }

        public static void Fire<T>(string eventName, T parem)
        {
            var raiseEvent = _eventDictionary[eventName] as Action<T>;
            raiseEvent?.Invoke(parem);
        }

        public static void Fire<T, Q>(string eventName, T parem, Q parem2)
        {
            var raiseEvent = _eventDictionary[eventName] as Action<T, Q>;
            raiseEvent?.Invoke(parem, parem2);
        }

        public static void UnsubscribeEvent(string eventName, Action method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var subscribers = _eventDictionary[eventName];
                subscribers -= method;
                _eventDictionary[eventName] = subscribers;
            }
        }

        public static void UnsubscribeEvent<T>(string eventName, Action<T> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var subscribers = _eventDictionary[eventName];
                subscribers -= method;
                _eventDictionary[eventName] = subscribers;
            }
        }

        public static void UnsubscribeEvent<T, Q>(string eventName, Action<T, Q> method)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                var subscribers = _eventDictionary[eventName];
                subscribers -= method;
                _eventDictionary[eventName] = subscribers;
            }
        }
    }
}
