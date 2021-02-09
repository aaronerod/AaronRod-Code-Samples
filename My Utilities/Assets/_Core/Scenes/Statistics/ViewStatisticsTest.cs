using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ViewStatisticsTest : MonoBehaviour
{
    [SerializeField]
    StatisticsController statisticsController;

    [SerializeField]
    private TextMeshProUGUI txtStatisticValue;
    [SerializeField]
    private TMP_InputField inputListenToId;
    [SerializeField]
    private TMP_InputField inputReportTo;
    [SerializeField]
    private Button buttonReport;

    private string reportToId;
    private string listeningToId;
    // Start is called before the first frame update
    void Start()
    {
        inputListenToId.onEndEdit.AddListener(OnIdListenerChanged);
        inputReportTo.onEndEdit.AddListener(OnIdReporterChanged);
        buttonReport.onClick.AddListener(() =>
        {
            statisticsController.ReportStatistic(reportToId, 5);
        });
    }

    public void OnIdListenerChanged(string value)
    {
        if (!string.IsNullOrEmpty(listeningToId))
            statisticsController.UnsubscribeFromStatistic(listeningToId, OnStatisticUpdated);
        listeningToId = value;
        statisticsController.SubscribeToStatistic(value, OnStatisticUpdated);
    }
    public void OnIdReporterChanged(string value)
    {
        reportToId = value;
    }

    public void OnStatisticUpdated(string key, double value)
    {
        txtStatisticValue.text = string.Format( "Statistic {0}:\n {1}",key, value.ToString());
    }
}
