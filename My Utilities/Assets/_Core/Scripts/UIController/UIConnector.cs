using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rod.Utilities.Patterns;
namespace Rod.Utilities.UI
{
    /// <summary>
    /// Puente para conectar con el sistema UIController
    /// </summary>
    [CreateAssetMenu(fileName = "UI Connector", menuName = "Rod/UI System/Connector")]
    public class UIConnector : SystemConnector<UIController>
    {
    }
}