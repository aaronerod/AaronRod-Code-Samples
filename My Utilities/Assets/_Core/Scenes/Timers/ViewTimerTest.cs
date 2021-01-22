using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rod.Utilities.Timers;
using TMPro;
using UnityEngine.UI;
public class ViewTimerTest : MonoBehaviour
{
    [SerializeField]
    TimersConnector timersConnector;
    [SerializeField]
    TextMeshProUGUI txtTimer;
    [SerializeField]
    Image timerProgress;
    // Start is called before the first frame update
    void Start()
    {
        timersConnector.ControllerAssigned += (controller) =>
        {
            controller.TimerAdded += OnTimerAdded;
        };
    }

    void OnTimerAdded(Timer timer)
    {
        timer.Updated += (updateArgs) =>
        {
            txtTimer.text= string.Format("{0} / {1}", updateArgs.elapsedInSeconds, updateArgs.durationInSeconds);
            timerProgress.fillAmount = (float)(updateArgs.elapsedInSeconds / updateArgs.durationInSeconds);
        };
    }


}
