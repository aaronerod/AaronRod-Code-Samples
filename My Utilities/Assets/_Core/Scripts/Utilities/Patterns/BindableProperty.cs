using System;
using UnityEngine;

namespace LittleSasquatch.Framework.Utilities.Patterns
{
    [Serializable]
    public struct BindableProperty<T>
    {
        private Action<T> OnValueChanged;
        [SerializeField] private T content;
        public T Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnValueChanged?.Invoke(content);
            }
        }
        public void AddListener(Action<T> listener)
        {
            listener?.Invoke(content);
            OnValueChanged += listener;
        }
        public void RemoveListener(Action<T> listener)
        {
            OnValueChanged -= listener;
        }
    }
}
