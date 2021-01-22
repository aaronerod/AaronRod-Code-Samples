using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rod.Utilities.Patterns;
namespace Rod.Utilities.MusicController
{
    /// <summary>
    /// Puente para conectar con MusicController sin usar referencias directas ni singletons
    /// </summary>
    [CreateAssetMenu(fileName = "Music Connector", menuName = "Rod/Music Controller/Connector", order =-1)]
    public class MusicConnector : SystemConnector<MusicController>
    {
    }
}