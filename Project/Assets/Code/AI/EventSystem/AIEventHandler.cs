using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class AIEventHandler
{
    [SerializeField] bool canHandleEvents = true;
    Animator animatorController;
    NavMeshAgent navAgent;
    AIComponent aiComponent;


    AIEventSystem eventSystem;

    public void Initialize(AIComponent _aiComponent, Animator _animator, NavMeshAgent _navAgent)
    {
        if (canHandleEvents)
        {
            aiComponent = _aiComponent;
            animatorController = _animator;
            navAgent = _navAgent;
            eventSystem = AIEventSystem.GetInstance();
            eventSystem.aiGroupEvent += OnEvent;
        }
    }

    internal void Update()
    {


    }

    void ProcessHits()
    {
        animatorController.SetTrigger("Hurt");
        aiComponent.currentState = AIState.HOSTILE;
        navAgent.ResetPath();


    }


    void OnEvent(AIEventData _event)
    {
        if (IsValidEvent(_event))
        {
            switch (_event.stimType)
            {
                case StimType.HURT:
                case StimType.THREATENING_SOUND:
                    if (aiComponent.currentState != AIState.HOSTILE)
                    {
                        aiComponent.currentState = AIState.HOSTILE;
                        aiComponent.currentTarget = _event.eventInstigator;
                        navAgent.ResetPath();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private bool IsValidEvent(AIEventData _event)
    {
        bool isValid = false;

        if (_event.sourceAgent != aiComponent as IEventSource)
        {
            switch (_event.stimType)
            {
                case StimType.HURT:
                    isValid = aiComponent.sensorySystem.IsEventSourceVisible(_event.eventInstigator);
                    break;
                default:
                    isValid = (_event.sourcePosition - aiComponent.transform.position).sqrMagnitude < _event.radius * _event.radius;
                    break;
            }
        }

        return isValid;
    }

    internal void OnDestroy()
    {
        if (eventSystem != null)
        {
            eventSystem.aiGroupEvent -= OnEvent;
        }
    }
}
