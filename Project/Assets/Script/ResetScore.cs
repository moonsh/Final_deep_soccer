using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScore : MonoBehaviour
{
    public Text purpleScore;
    public Text blueScore;
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        purpleScore.text = "0";
        blueScore.text = "0";
    }
}
