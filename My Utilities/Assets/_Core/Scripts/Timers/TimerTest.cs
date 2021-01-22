using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Rod.Utilities.Timers
{
    /// <summary>
    /// Implementación de uso de Timers
    /// </summary>
    public class TimerTest : MonoBehaviour
    {
        [SerializeField]
        TimersConnector timerConnector;
        /// <summary>
        /// Referencia al connector de TimerssController
        /// </summary>
        public TimersConnector Connector => timerConnector;
        /// <summary>
        /// Id del timer a crear
        /// </summary>
        [SerializeField]
        string timerId;
        /// <summary>
        /// Duración del timer
        /// </summary>
        [SerializeField]
        float duration;

        /// <summary>
        /// Crear un timer
        /// </summary>
        public void AddTimer()
        {
            //Agregar un timer con el id y duración especificados
            Timer timer = timerConnector.Controller.AddTimer(timerId, duration);
            //Suscribirse al evento de completado
            timer.Completed += () => Debug.Log("Timer completed " + timer.Id);

        }

        /// <summary>
        /// Reducir duración del timer especificado
        /// </summary>
        public void ReduceDuration()
        {
            timerConnector.Controller.ReduceDuration(timerId, duration);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TimerTest))]
    public class TimerTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                TimerTest test = (TimerTest)target;
                if (GUILayout.Button("Add Timer"))
                {
                    test.AddTimer();
                }
                if (GUILayout.Button("Reduce duration"))
                {
                    test.ReduceDuration();
                }
                TimersConnector connector = test.Connector;
                foreach (KeyValuePair<string, Timer> timer in connector.Controller.ActiveTimers)
                {
                    Timer timerValue = timer.Value;
                    GUILayout.Label(timerValue.Id + " " + timerValue.RemainingSeconds);
                }
            }
        }
    }
#endif
}