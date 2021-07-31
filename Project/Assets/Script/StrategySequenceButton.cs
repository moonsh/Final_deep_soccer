using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategySequenceButton : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        ActionSequenceList.key= button.GetComponent<Button>().GetComponentInChildren<Text>().text;
        foreach (var b in ActionSequenceList.buttons)
        {
            Destroy(b);
        }
        CoachController.toActionSequence();
        GameObject.Find("ActionSequenceList").GetComponent<ActionSequenceList>().updateBoard();

    }
}
