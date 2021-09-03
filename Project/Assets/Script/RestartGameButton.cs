using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartGameButton : MonoBehaviour
{
    public Text purpleScore;
    public Text blueScore;
    public Transform SoccerStadium;
    public Button button;

    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SoccerStadium.GetComponent<SoccerEnvController>().ResetScene();
//        purpleScore.text = "0";
//        blueScore.text = "0";
    }
}
