using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSequenceList : MonoBehaviour
{
    public Transform list;
    [SerializeField] GameObject buttonPerfab;
    public HashSet<Vector3> positions = new HashSet<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Scenario action in CoachController.scenarios.Values)
        {
            GameObject button = (GameObject)Instantiate(buttonPerfab);
            button.GetComponent<Text>().text = action.agentPosition.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Scenario action in CoachController.scenarios.Values)
        {
            if(!positions.Contains(action.agentPosition))
            {
                GameObject button = Object.Instantiate(buttonPerfab);
                button.GetComponentInChildren<Text>().text = action.agentPosition.ToString();
                button.transform.SetParent(list);
                positions.Add(action.agentPosition);
            }
            
        }
    }
    
}
