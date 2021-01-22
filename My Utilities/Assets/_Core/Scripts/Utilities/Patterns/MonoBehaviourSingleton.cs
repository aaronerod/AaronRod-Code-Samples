using UnityEngine;

namespace Rod.Utilities.Patterns
{/// <summary>
    /// Clase base para Singletons
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        /// <summary>
        /// Referencia a la instancia actual
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance)
                {
                    return instance;
                }
                else
                {
                    var instances = FindObjectsOfType<T>();
                    if (instances.Length > 0)
                    {
                        instance = instances[0];
                    }
                    if (instances.Length > 1)
                    {
                        Debug.LogError("Hay más de una instancia de " + typeof(T).Name);
                    }
                    if (!instance)
                    {
                        instance = CreateDefaultInstance();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Inicializar los componentes del singleton. Buen lugar para volverlo
        /// persistente
        /// </summary>
        public abstract void InitializeSingleton();

        /// <summary>
        /// Prevenir duplicado de instancias
        /// </summary>
        public virtual void Awake()
        {
            if (instance == null || instance==this)
            {
                instance = this as T;
                InitializeSingleton();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        /// <summary>
        /// Crear una instancia default del singleton
        /// </summary>
        /// <returns></returns>
        public static T CreateDefaultInstance()
        {
            string typeName = typeof(T).Name;
            GameObject singletonPrefab = Resources.Load(typeName) as GameObject;
            GameObject singletonInstance = null;
            if (singletonPrefab)
            {
                singletonInstance = Instantiate(singletonPrefab);
            }
            else
            {
                singletonInstance = new GameObject(typeName+"-Runtime");
                singletonInstance.AddComponent<T>();
            }
            T singleton = singletonInstance.GetComponent<T>();
            MonoBehaviourSingleton<T> genericSingleton = singleton as MonoBehaviourSingleton<T>;
            genericSingleton.InitializeSingleton();
            return singleton;
        }

        /// <summary>
        /// Vuelve el singleton persistente
        /// </summary>
        public void SetAsPersistentSingleton()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
}


