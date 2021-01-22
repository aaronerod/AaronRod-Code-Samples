using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rod.Utilities.Execution
{
    /// <summary>
    /// Última fase de ejecución
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Inicializar componentes después de haber cargado
        /// </summary>
        IEnumerator Initialize();
    }
}