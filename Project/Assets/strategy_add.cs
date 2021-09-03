using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class strategy_add : MonoBehaviour
{
    public Button button;
    public GameObject coach;


    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);

    }
    void TaskOnClick()
    {
        coach.GetComponent<CoachController>().add_strategy();
        //        purpleScore.text = "0";
        //        blueScore.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
