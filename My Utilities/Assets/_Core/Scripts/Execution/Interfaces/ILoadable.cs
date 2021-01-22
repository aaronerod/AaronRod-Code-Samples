using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.Execution
{
    /// <summary>
    /// Segunda fase de ejecución
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Cargar datos
        /// </summary>
        IEnumerator Load();
    }
}