using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rod.Utilities.Execution
{
    /// <summary>
    /// Guardables
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// Notifica que una propiedad ha cambiado
        /// </summary>
        event Action PropertyChanged;
        /// <summary>
        /// Proceso de guardado
        /// </summary>
        IEnumerator Save();
    }
}
