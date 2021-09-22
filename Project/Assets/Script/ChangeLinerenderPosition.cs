using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLinerenderPosition : MonoBehaviour
{
    public Button button;
    public GameObject ActionConditionBoard;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        ActionConditionBoard.GetComponent<ActionConditionBoard>().lineRenderer.SetPosition(0, new Vector3(100, 100, 100));
        ActionConditionBoard.GetComponent<ActionConditionBoard>().lineRenderer.SetPosition(1, new Vector3(100, 100, 100));
        ActionConditionBoard.GetComponent<ActionConditionBoard>().targetMark.position = new Vector3(100, 100, 100);
    }
}
