using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDebugView : MonoBehaviour
{
    public KeyCode activationKey;

    List<BTContextData> allContextsList;
    BehaviourTreeManager behaviourTreeManager;
    Camera activeCamera;
    Rect debugViewRect = new Rect();

    protected bool viewActive;
    protected BTContextData activeViewContext;
    protected GUIStyle debugStyle = new GUIStyle();
    protected float debugWidth = 10;
    protected float debugHeight = 50;

    static int activeContextIndex = 0;
    static AIDebugView activeView;

    private void Start()
    {
        behaviourTreeManager = BehaviourTreeManager.GetInstance();
        activeCamera = CameraFollow.activeCamera;
        debugStyle.fontSize = 30;
        debugStyle.normal.textColor = Color.green;
    }

    private void Update()
    {
        OnDebugViewUpdate();
        HandleInputs();

        if (viewActive)
        {
            UpdateDebugView();
        }
    }

    private void UpdateDebugView()
    {
        if (activeViewContext != allContextsList[activeContextIndex])
        {
            activeViewContext = allContextsList[activeContextIndex];
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(activationKey))
        {
            ToggleView();
        }

        if (viewActive)
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ++activeContextIndex;
                if (activeContextIndex > allContextsList.Count - 1) { activeContextIndex = 0; }
            }
            else if (Input.GetKeyDown(KeyCode.Comma))
            {
                --activeContextIndex;
                if (activeContextIndex < 0) { activeContextIndex = allContextsList.Count - 1; }
            }
        } 
    }

    void ToggleView()
    {
        viewActive = !viewActive;
        if (viewActive)
        {
            if (activeView != null)
            {
                activeView.Deactivate();
                debugStyle.normal.textColor = Color.green;
            }

            activeView = this;
            allContextsList = behaviourTreeManager.GetAllContextData();

            if (allContextsList.Count > 0)
            {
                if (activeContextIndex > allContextsList.Count - 1)
                {
                    activeContextIndex = 0;
                }

                viewActive = true;
            }
        }
    }

    protected virtual void OnGUI()
    {
        if (!viewActive) { return; }

        Vector3 agentPosition = activeViewContext.owningContext.contextOwner.transform.position;

        Vector3 viewportPoint = activeCamera.WorldToViewportPoint(agentPosition);
        bool isInViewport = Mathf.Min(viewportPoint.x, viewportPoint.y, viewportPoint.z) > 0;
        
        if (isInViewport)
        {
            Vector2 screenPos = activeCamera.WorldToScreenPoint(agentPosition);
            screenPos.y = Screen.height - screenPos.y;
            debugViewRect.Set(screenPos.x, screenPos.y, debugWidth, debugHeight);
            GUI.Label(debugViewRect, GetDebugText(), debugStyle);
        }
    }

    public void Deactivate() { viewActive = false; }

    protected virtual void OnDebugViewUpdate() { }
    protected abstract string GetDebugText();
}

