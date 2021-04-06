// Anthony Tiongson (ast119)

using System;
using System.Collections.Generic;
using UnityEngine;

public class CoachController : MonoBehaviour
{
    public static bool coachMode;
    public static HashSet<AIComponent> agentsWithUserActions = new HashSet<AIComponent>();

    public GameObject userActionsGUI;
    public GameObject cancelUserActionsGUI;

    [SerializeField] private string agent = "purpleAgent";
    [SerializeField] private string field = "field";
    [SerializeField] private string ball = "ball";
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform[] selectedPlayer = new Transform[1];
    private Transform _selection;
    private bool coachMoveMode;

    public bool GetCoachMode()
    {
        return coachMode;
    }

    public void ToggleCoachMode()
    {
        coachMode = !coachMode;
    }

    public void ToggleCoachModeMove()
    {
        coachMoveMode = !coachMoveMode;
    }

    public void ClearAllUSerActions()
    {
        foreach (var agent in agentsWithUserActions)
        {
            agent.GetComponent<AIComponent>().ClearAllActions();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        userActionsGUI.SetActive(false);
        cancelUserActionsGUI.SetActive(false);
        coachMode = false;
        coachMoveMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agentsWithUserActions.Count == 0)
        {
            cancelUserActionsGUI.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            ToggleCoachMode();
        }

        if (coachMode) // coachMode enabled.
        {
            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();

                if (Array.IndexOf(selectedPlayer, _selection) != 0)
                {
                    selectionRenderer.material = defaultMaterial;
                }
                else
                {
                    selectionRenderer.material = selectedMaterial;
                }
                
                _selection = null;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (selectedPlayer[0] == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;

                    if (selection.CompareTag(agent))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (selectionRenderer != null)
                        {
                            if (Array.IndexOf(selectedPlayer, selection) != 0)
                            {
                                selectionRenderer.material = highlightMaterial;
                            }

                            if (Input.GetButtonDown("Fire1"))
                            {
                                if (selectedPlayer[0] == null)
                                {
                                    selectedPlayer[0] = selection;
                                    userActionsGUI.SetActive(true);
                                }
                            }
                        }

                        _selection = selection;
                    }
                }
            }
            else // if there is a selected player
            {

                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;

                    if (selectedPlayer[0] == selection)
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();

                        if (Input.GetButtonDown("Fire1"))
                        {
                            Array.Clear(selectedPlayer, 0, 1);
                            userActionsGUI.SetActive(false);
                        }

                        _selection = selection;
                    }
                }

                if (coachMoveMode)
                {
                    var selectedAgent = selectedPlayer[0].GetComponent<AIComponent>();

                    if (Physics.Raycast(ray, out hit))
                    {
                        var selection = hit.transform;

                        if (selection.CompareTag(field) || selection.CompareTag(ball))
                        {
                            if (Input.GetButtonDown("Fire1"))
                            {
                                //Debug.Log("Attempting to assign a new destination for the chosen agent.");

                                if (selection.CompareTag(ball))
                                {
                                    selectedAgent.AddAction("GoToBall");
                                }
                                else
                                {
                                    selectedAgent.AddAction("Move");
                                    selectedAgent.AddActionMove(hit.point);
                                }

                                agentsWithUserActions.Add(selectedAgent.GetComponent<AIComponent>());
                                cancelUserActionsGUI.SetActive(true);
                                userActionsGUI.SetActive(true);
                                coachMoveMode = !coachMoveMode;
                            }
                        }
                    }
                }
            }
        }
        else // coachMode disabled, reset all agent material that has been changed
        {

            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
            }

            _selection = null;

            if (selectedPlayer[0] != null)
            {
                var selectionRenderer = selectedPlayer[0].GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
                Array.Clear(selectedPlayer, 0, 1);
                userActionsGUI.SetActive(false);
            }
        }
    }
}
