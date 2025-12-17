using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM  
{
     public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
    private IState currentState;
    
}
