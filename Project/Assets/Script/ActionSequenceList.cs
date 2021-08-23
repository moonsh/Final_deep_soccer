using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSequenceList : MonoBehaviour
{
    public Transform list;
    public Text boardName;
    public GameObject buttonPerfab;
    public static LinkedList<GameObject> buttons = new LinkedList<GameObject>();
    public Button saveButton;
    public Button deleteButton;
    public static string key;
    private int count = 0;
    private int ScenarioCount = 0;
    void Start()
    {
        saveButton.onClick.AddListener(TaskOnClick);
        deleteButton.onClick.AddListener(TaskOnClick);
    }
    // Update is called once per frame
    void Update()
    {
        if(count<1 || ScenarioCount != CoachController.scenarios.Count)
        {
            updateBoard();
        }
        count++;
    }
    void TaskOnClick()
    {
        ClearBoard();
        count = 0;
    }
    public void ClearBoard()
    {
        foreach(GameObject bt in buttons)
        {
            Destroy(bt);
        }
    }
    public void updateBoard()
    {
        ClearBoard();
        boardName.text = key + " Actions";
        ScenarioCount = CoachController.scenarios.Count;
        foreach (KeyValuePair<string, Scenario> entry in CoachController.scenarios)
        {
            string sc = entry.Key;
            if(entry.Value.strategy == key)
            {
                GameObject button = Object.Instantiate(buttonPerfab);
                button.GetComponentInChildren<Text>().text = sc;
                button.transform.SetParent(list);
                buttons.AddLast(button);
                
            }
            
        }
    }
    
}
