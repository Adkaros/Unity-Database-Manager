using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using M1.Utilities;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject[] states;

    public static DatabaseEntry user = new DatabaseEntry();
    public event NextButtonHandler next;
    public delegate void NextButtonHandler();

    IEnumerator Start()
    { 
        yield return null;
        GoToStartState();
    }

    public static IState GetState(int _num)
    {
        return Instance.states[_num].GetComponent<IState>();
    }

    public static void NextState()
    {
        StateMachine.NextState();
    }

    public static void GoToStartState()
    {
        StateMachine.ChangeState(Instance.states[0].GetComponent<IState>());
    }

    public void Btn_Next()
    {
        if (next != null)
            next();
    }

}
