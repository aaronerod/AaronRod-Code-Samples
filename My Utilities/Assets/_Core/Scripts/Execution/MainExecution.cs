using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.Execution
{
    /// <summary>
    /// Encargado de ejecutar las fases de inicialización de sistemas importantes
    /// </summary>
    public class MainExecution : MonoBehaviour
    {
        /// <summary>
        /// Lista de objetos que serán preinicializados
        /// </summary>
        [SerializeField]
        List<GameObject> preInitializers = new List<GameObject>();
        /// <summary>
        /// Lista de objetos que serán cargados
        /// </summary>
        [SerializeField]
        List<GameObject> loadersExecutionOrder = new List<GameObject>();
        /// <summary>
        /// Lista de objetos que seran inicializados
        /// </summary>
        [SerializeField]
        List<GameObject> initializersExecutionOrder = new List<GameObject>();
        /// <summary>
        /// Lista de objetos que se guardarán
        /// </summary>
        [SerializeField]
        List<GameObject> savablesExecutionOrder = new List<GameObject>();
        private void Awake()
        {
            StartCoroutine(ExecutionProcess());
        }

        IEnumerator ExecutionProcess()
        {

            for (int i = 0; i < preInitializers.Count; i++)
            {
                IPreInitializable preInitializable = preInitializers[i].GetComponent<IPreInitializable>();
                yield return preInitializable.PreInitialize();
            }
            for (int i = 0; i < loadersExecutionOrder.Count; i++)
            {
                ILoadable loadable = loadersExecutionOrder[i].GetComponent<ILoadable>();
                yield return loadable.Load();
            }
            for (int i = 0; i < initializersExecutionOrder.Count; i++)
            {
                IInitializable initializable = initializersExecutionOrder[i].GetComponent<IInitializable>();
                yield return initializable.Initialize();
            }
            for (int i = 0; i < savablesExecutionOrder.Count; i++)
            {
                ISavable savable = savablesExecutionOrder[i].GetComponent<ISavable>();
                savable.PropertyChanged += OnSaveRequired;
            }
        }

        private void OnValidate()
        {
            bool changed = false;
            for (int i = 0; i < preInitializers.Count; i++)
            {
                if (preInitializers[i] != null && preInitializers[i].GetComponent<IPreInitializable>() == null)
                {
                    preInitializers[i] = null;
                    changed = true;
                }
            }
            for (int i = 0; i < loadersExecutionOrder.Count; i++)
            {
                if (loadersExecutionOrder[i] != null && loadersExecutionOrder[i].GetComponent<ILoadable>() == null)
                {
                    loadersExecutionOrder[i] = null;
                    changed = true;
                }
            }
            for (int i = 0; i < initializersExecutionOrder.Count; i++)
            {
                if (initializersExecutionOrder[i] != null && initializersExecutionOrder[i].GetComponent<IInitializable>() == null)
                {
                    initializersExecutionOrder[i] = null;
                    changed = true;
                }
            }
            for (int i = 0; i < savablesExecutionOrder.Count; i++)
            {
                if (savablesExecutionOrder[i] != null && savablesExecutionOrder[i].GetComponent<ISavable>() == null)
                {
                    savablesExecutionOrder[i] = null;
                    changed = true;
                }
            }
#if UNITY_EDITOR
            if (changed)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        void OnSaveRequired()
        {
            //Save implementation here
        }
    }

}