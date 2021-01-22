using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rod.Utilities.Patterns;
namespace Rod.Utilities.Timers
{
    /// <summary>
    /// Puente para conectar con el sistema TimersController
    /// </summary>
    [CreateAssetMenu(fileName = "Timers Connector", menuName = "Rod/Timers/Connector")]
    public class TimersConnector : SystemConnector<TimersController>
    {
    }
}