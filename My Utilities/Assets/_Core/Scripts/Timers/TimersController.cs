using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rod.Utilities.Timers
{
    /// <summary>
    /// Sistema para controlar timers que notifican al completarse
    /// </summary>
    public class TimersController : MonoBehaviour
    {
        /// <summary>
        /// Notifica que un timer ha sido iniciado
        /// </summary>
        public event Action<Timer> TimerAdded;
        /// <summary>
        /// Notifica que un timer ha sido completado
        /// </summary>
        public event Action<string> TimerCompleted;
        /// <summary>
        /// Notifica que la duración de un timer ha sido reducida
        /// </summary>
        public event Action<string> TimerReduced;
        /// <summary>
        /// Notifica que un timer ha sido cancelado
        /// </summary>
        public event Action<string> TimerCancelled;
        /// <summary>
        /// Referencia al connector del sistema
        /// </summary>
        [SerializeField]
        private TimersConnector connector;

        private Dictionary<string, Timer> activeTimers = new Dictionary<string, Timer>();
        /// <summary>
        /// Diccionario de timers activos
        /// </summary>
        public Dictionary<string, Timer> ActiveTimers => activeTimers;

        private List<Timer> completedTimers = new List<Timer>();
        /// <summary>
        /// Lista de timers que han sido completados
        /// </summary>
        public List<Timer> CompletedTimers { get => completedTimers; }

        private void Awake()
        {
            //Inicia la referencia del controlador para que otros sistemas puedan accesar externamente
            connector.Controller = this;
        }

        private void Update()
        {
            CompletedTimers.Clear();
            foreach (KeyValuePair<string, Timer> timer in activeTimers)
            {
                timer.Value.Update();
                if (timer.Value.IsTimerCompleted)
                    CompletedTimers.Add(timer.Value);
            }
            for (int i = 0; i < CompletedTimers.Count; i++)
            {
                activeTimers.Remove(CompletedTimers[i].Id);
                TimerCompleted?.Invoke(CompletedTimers[i].Id);
            }
        }

        /// <summary>
        /// Inicia la ejecución de un timer con una fecha de inicio específica
        /// </summary>
        /// <param name="timerId">Id para identificarlo</param>
        /// <param name="startTime">Fecha de inicio</param>
        /// <param name="durationInSeconds">Duración en segundos</param>
        /// <returns></returns>
        public Timer AddTimer(string timerId, DateTime startTime, float durationInSeconds)
        {
            Timer newTimer = new Timer(timerId, startTime, durationInSeconds);
            activeTimers.Add(timerId, newTimer);
            TimerAdded?.Invoke(newTimer);
            return newTimer;
        }

        /// <summary>
        /// Inicia la ejecución de un timer con fecha de inicio en el momento
        /// en que ha sido invocado el método
        /// </summary>
        /// <param name="timerId">Id para identificarlo</param>
        /// <param name="durationInSeconds">Duración en segundos</param>
        /// <returns></returns>
        public Timer AddTimer(string timerId, float durationInSeconds)
        {
            return AddTimer(timerId, DateTime.UtcNow, durationInSeconds);
        }

        /// <summary>
        /// Elimina un timer
        /// </summary>
        /// <param name="timerId">Identificador del timer</param>
        public void RemoveTimer(string timerId)
        {
            activeTimers.Remove(timerId);
        }

        /// <summary>
        /// Cancela un timer y notifica la cancelación
        /// </summary>
        /// <param name="id">Id del timer</param>
        public void CancelTimer(string id)
        {
            Timer timer = null;
            if (activeTimers.TryGetValue(id, out timer))
            {
                timer.Cancel();
                TimerCancelled?.Invoke(id);
                activeTimers.Remove(id);
            }
        }

        /// <summary>
        /// Finaliza un timer inmediatamente
        /// </summary>
        /// <param name="id">Id del timer</param>
        public void CompleteTimer(string id)
        {
            Timer timer = null;
            if (activeTimers.TryGetValue(id, out timer))
            {
                activeTimers.Remove(id);
                timer.Finish();
                activeTimers.Remove(id);
                TimerCompleted?.Invoke(id);

            }
        }

        /// <summary>
        /// Reduce la duración de un timer
        /// </summary>
        /// <param name="id">Id del timer</param>
        /// <param name="secondsToReduce">Segundos restados al timer</param>
        public void ReduceDuration(string id, float secondsToReduce)
        {
            Timer timer = null;
            if (activeTimers.TryGetValue(id, out timer))
            {
                timer.ReduceDuration(secondsToReduce);
                TimerReduced?.Invoke(id);
            }
        }
    }


    /// <summary>
    /// Contiene la información de un Timer en ejecución
    /// </summary>
    public class Timer
    {
        public struct UpdateArgs
        {
            public double durationInSeconds;
            public double elapsedInSeconds;
        }
        private UpdateArgs updateArgs;
        /// <summary>
        /// Notifica que el timer ha sido completado
        /// </summary>
        public event Action Completed;
        /// <summary>
        /// Notifica que el timer ha sido cancelado
        /// </summary>
        public event Action Cancelled;
        /// <summary>
        /// Notifica que el timer ha sido reducido
        /// </summary>
        public event Action Reduced;
        /// <summary>
        /// Notifica la actualización del estado del timer (cada frame)
        /// </summary>
        public event Action<UpdateArgs> Updated;
        string id;
        /// <summary>
        /// Id del timer
        /// </summary>
        public string Id => id;
        /// <summary>
        /// Fecha de inicio del timer
        /// </summary>
        DateTime startingTime;
        /// <summary>
        /// Duración del timer
        /// </summary>
        double durationInSeconds;
        double elapsedSeconds;
        /// <summary>
        /// Segundos transcurridos
        /// </summary>
        public double ElapsedSeconds => elapsedSeconds;
        /// <summary>
        /// Segundos restantes
        /// </summary>
        public double RemainingSeconds => durationInSeconds - elapsedSeconds;

        bool isTimerCompleted;
        /// <summary>
        /// Indica si el timer ha sido completado
        /// </summary>
        public bool IsTimerCompleted => isTimerCompleted;

        /// <summary>
        /// Crea un timer con fecha de inicio en el momento de invocación
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        public Timer(string id, float duration)
        {
            this.id = id;
            startingTime = DateTime.UtcNow;
            durationInSeconds = duration;
        }
        /// <summary>
        /// Crea un timer con una fecha de inicio específica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        public Timer(string id, DateTime startTime, double duration)
        {
            this.id = id;
            this.startingTime = startTime;
            durationInSeconds = duration;
        }

        /// <summary>
        /// Actualiza el estado del timer
        /// </summary>
        public void Update()
        {
            if (!isTimerCompleted)
            {
                double elapsedSeconds = DateTime.UtcNow.Subtract(startingTime).TotalSeconds;
                this.elapsedSeconds = elapsedSeconds;
                if (elapsedSeconds >= durationInSeconds)
                {
                    Finish();
                }
                else
                {
                    updateArgs.durationInSeconds = durationInSeconds;
                    updateArgs.elapsedInSeconds = elapsedSeconds;
                    Updated?.Invoke(updateArgs);
                }
            }
        }

        /// <summary>
        /// Reduce la duración del timer
        /// </summary>
        /// <param name="secondsToReduce"></param>
        public void ReduceDuration(float secondsToReduce)
        {
            durationInSeconds -= secondsToReduce;
            Update();
            Reduced?.Invoke();
        }

        /// <summary>
        /// Finaliza el timer
        /// </summary>
        public void Finish()
        {
            isTimerCompleted = true;
            Completed?.Invoke();
        }
        /// <summary>
        /// Cancela el timer
        /// </summary>
        public void Cancel()
        {
            Cancelled?.Invoke();
        }

    }
}