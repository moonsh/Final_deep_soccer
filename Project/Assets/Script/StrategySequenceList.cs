using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategySequenceList : MonoBehaviour
{
    public Transform list;
    public Text blueStrategy;
    public Text purpleStrategy;
    public GameObject buttonPerfab;
    public static LinkedList<GameObject> buttons = new LinkedList<GameObject>();
    public Button strategySequenceButton;
    public Button applyButton;
    public Button deleteButton;
    public Button add_strategySequence;
    int count = 0;
    void Start()
    {
        strategySequenceButton.onClick.AddListener(TaskOnClick);
        applyButton.onClick.AddListener(TaskOnClick);
        deleteButton.onClick.AddListener(TaskOnClick);
        add_strategySequence.onClick.AddListener(TaskOnClick);
    }
    // Update is called once per frame
    void Update()
    {
        if (count < 1)
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
        foreach (GameObject bt in buttons)
        {
            Destroy(bt);
        }
    }
    public void updateBoard()
    {
        ClearBoard();
       
        if (int.Parse(SoccerEnvController.blueScore1.text) == int.Parse(SoccerEnvController.purpleScore1.text))
        {
            
            blueStrategy.text = "Draw";     //  NoStrategy
            purpleStrategy.text = "Draw";  //  NoStrategy
        }
        else if(int.Parse(SoccerEnvController.blueScore1.text) > int.Parse(SoccerEnvController.purpleScore1.text))
        {
            
            blueStrategy.text = "Win";   //  DeActiveStrategy
            purpleStrategy.text = "Lose";   //  ActiveStrategy
        }
        else
        {
            blueStrategy.text = "Lose";  //  ActiveStrategy
            purpleStrategy.text = "Win";  //  DeActiveStrategy
        }
       
        foreach (string sc in CoachController.strategySequence)
        {
            GameObject button = Object.Instantiate(buttonPerfab);
            button.GetComponentInChildren<Text>().text = sc;
            button.transform.SetParent(list);
            buttons.AddLast(button);
        }
    }
}
