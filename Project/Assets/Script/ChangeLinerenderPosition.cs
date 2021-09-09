using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLinerenderPosition : MonoBehaviour
{
    public Button button;
    public GameObject ActionConditionBoard;
    public Text text1;
    public Text text2;
    public Canvas canvas;
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        ActionConditionBoard.GetComponent<ActionConditionBoard>().lineRenderer.SetPosition(0, new Vector3(100, 100, 100));
        ActionConditionBoard.GetComponent<ActionConditionBoard>().lineRenderer.SetPosition(1, new Vector3(100, 100, 100));
        ActionConditionBoard.GetComponent<ActionConditionBoard>().circle1.positionCount = 0;
        ActionConditionBoard.GetComponent<ActionConditionBoard>().circle2.positionCount = 0;
        ActionConditionBoard.GetComponent<ActionConditionBoard>().targetMark.position = new Vector3(100, 100, 100);
        text1.text = "";
        text2.text = "";
        text1.transform.position = new Vector3(canvas.transform.position.x + 75, canvas.transform.position.y - 10, canvas.transform.position.z);
        text2.transform.position = new Vector3(canvas.transform.position.x + 75, canvas.transform.position.y - 10, canvas.transform.position.z);
    }
}
