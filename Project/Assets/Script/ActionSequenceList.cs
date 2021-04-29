using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSequenceList : MonoBehaviour
{
    public Transform list;
    public GameObject buttonPerfab;
    public static LinkedList<GameObject> buttons = new LinkedList<GameObject>();
    public Button actionSequenceButton;
    public Button saveButton;
    int count = 0;
    void Start()
    {
        actionSequenceButton.onClick.AddListener(TaskOnClick);
        saveButton.onClick.AddListener(TaskOnClick);
    }
    // Update is called once per frame
    void Update()
    {
        if(count<1)
        {
            foreach (string sc in CoachController.scenarios.Keys)
            {
                GameObject button = Object.Instantiate(buttonPerfab);
                button.GetComponentInChildren<Text>().text = sc;
                button.transform.SetParent(list);
                buttons.AddLast(button);
            }
        }
        count++;
    }
    void TaskOnClick()
    {
        ClearBoard();
        count = 0;
    }
    void ClearBoard()
    {
        foreach(GameObject bt in buttons)
        {
            bt.SetActive(false);
        }
    }
}
