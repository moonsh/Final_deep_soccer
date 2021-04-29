using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSequenceButton : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        ActionConditionBoard.agentPosition= button.GetComponent<Button>().GetComponentInChildren<Text>().text;
        foreach (var b in ActionSequenceList.buttons)
        {
            b.SetActive(false);
        }
        CoachController.toActionCondition();
        GameObject.Find("ActionConditionBoard").GetComponent<ActionConditionBoard>().resetBoard();
        

    }
}
