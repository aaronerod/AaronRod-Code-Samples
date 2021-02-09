using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Sistema para contabilizar distintas estadísticas dentro de un juego. Notifica cuando una estadística
/// en específico ha sido actualizada.
/// </summary>
public class StatisticsController : MonoBehaviour
{

    private Dictionary<string, double> statistics = new Dictionary<string, double>();
    /// <summary>
    /// Estadísticas globales
    /// </summary>
    public Dictionary<string, double> Statistics => statistics;

    /// <summary>
    /// Diccionario de notificadores
    /// </summary>
    private Dictionary<string, StatisticNotifier> StatisticNotifiers = new Dictionary<string, StatisticNotifier>();



    /// <summary>
    /// Reportar el progreso de una estadística
    /// </summary>
    /// <param name="key">Identificador de la estadística</param>
    /// <param name="value">Valor a sumar</param>
    /// <returns></returns>
    public double ReportStatistic(string key, double value)
    {
        Dictionary<string, double> selectedCollection =statistics;
       
        if (selectedCollection.ContainsKey(key))
        {
            selectedCollection[key] += value;
        }
        else
        {
            selectedCollection.Add(key, value);
        }
        double updatedValue = selectedCollection[key];
        Notify(key, updatedValue);

        return updatedValue;
    }


    /// <summary>
    /// Notificar a los listeners de la estadística dada
    /// </summary>
    /// <param name="key">Identificador de la estadística</param>
    /// <param name="value">Valor actual de la estadística</param>
    void Notify(string key, double value)
    {
        if (StatisticNotifiers.ContainsKey(key))
        {
            StatisticNotifiers[key].Notify(value);
        }
    }

    /// <summary>
    /// Suscribir un listener para recibir notificaciones cuando la estadística
    /// cambie su valor
    /// </summary>
    /// <param name="key">Identificador de la estadística</param>
    /// <param name="listener">Acción a ser llamada al recibir cambios la estadística dada</param>
    public void SubscribeToStatistic(string key, Action<string, double> listener)
    {
        if (StatisticNotifiers.ContainsKey(key))
        {
            StatisticNotifiers[key].StatisticChanged += listener;
        }
        else
        {
            StatisticNotifier notifier = new StatisticNotifier(key);
            notifier.StatisticChanged += listener;
            StatisticNotifiers.Add(key, notifier);
        }
        if (statistics.ContainsKey(key))
        {
            listener?.Invoke(key, statistics[key]);
        }
        else
        {
            listener?.Invoke(key, 0);
        }
    }

    /// <summary>
    /// Remover suscripción de un listener
    /// </summary>
    /// <param name="key">Identificador de la estadística</param>
    /// <param name="listener">Acción previamente suscrita</param>
    public void UnsubscribeFromStatistic(string key, Action<string,double> listener)
    {
        if (StatisticNotifiers.ContainsKey(key))
            StatisticNotifiers[key].StatisticChanged -= listener;
    }
}

/// <summary>
/// Contiene las suscripciones de listeners interesados en saber los cambios
/// de estado en una estadística
/// </summary>
public class StatisticNotifier
{
    /// <summary>
    /// Identificador de la estadística
    /// </summary>
    public string key;
    private event Action<string,double> statisticChanged;
    /// <summary>
    /// Evento que notifica el cambio de una estadística
    /// </summary>
    public event Action<string, double> StatisticChanged
    {
        add
        {
            statisticChanged = value;
        }
        remove
        {
            statisticChanged -= value;
        }
    }

    public StatisticNotifier(string key)
    {
        this.key = key;
    }
    /// <summary>
    /// Notifica a todos los listeners
    /// </summary>
    /// <param name="value"></param>
    public void Notify(double value)
    {
        statisticChanged?.Invoke(key, value);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StatisticsController))]
public class StatisticsControllerEditor : Editor
{
    [SerializeField]
    string statisticName;
    [SerializeField]
    double reportedValue;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            GUILayout.Label("Available Statistics");
            StatisticsController statisticsManager = (StatisticsController)target;
            Dictionary<string, double> lifetime = statisticsManager.Statistics;
            GUILayout.Label("Lifetime stats", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            foreach (KeyValuePair<string, double> statistic in lifetime)
            {
                GUILayout.Label(statistic.Key + " " + statistic.Value);
            }
            EditorGUI.indentLevel--;
            GUILayout.Label("Session stats", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel--;
            GUILayout.Label("Testing", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            statisticName = EditorGUILayout.TextField("Statistic name", statisticName);
            reportedValue = EditorGUILayout.DoubleField("Value", reportedValue);

            if (GUILayout.Button("Report"))
            {
                statisticsManager.ReportStatistic(statisticName, reportedValue);
            }
            EditorGUI.indentLevel--;
        }

    }
}
#endif