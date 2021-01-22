using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.Execution
{
    /// <summary>
    /// Primera fase de ejecución
    /// </summary>
    public interface IPreInitializable
    {
        /// <summary>
        /// Preinicializar componentes. 
        /// </summary>
        IEnumerator PreInitialize();
    }
}
