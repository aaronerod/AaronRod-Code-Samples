using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Rod.Utilities.Patterns
{
    public abstract class SystemConnector<T> : ScriptableObject where T : MonoBehaviour
    {
        private event Action<T> controllerAssigned;
        public event Action<T> ControllerAssigned
        {
            remove => controllerAssigned -= value;
            add
            {
                controllerAssigned += value;
                if (controller != null)
                    value(controller);
            }
        }

        T controller;
        public T Controller
        {
            get => controller;
            set
            {
                controller = value;
                if (controller != null)
                {
                    controllerAssigned?.Invoke(controller);
                }
            }
        }
    }
}