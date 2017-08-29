using UnityEngine;
using System.Collections;
using M1.Utilities;
using System;

[Serializable]
public class DeleteState : MonoBehaviour, IState
{
    public GameObject canvasObject;
    public GameObject nextState;

    public void Execute()
    {
    }

    public IEnumerator iEnter()
    {
        Debug.Log("1 Enter: " + Time.time);
        canvasObject.SetActive(true);

        yield return new WaitForSeconds(1);
    }

    public IEnumerator iExit()
    {
        Debug.Log("1 Exit: " + Time.time);
        yield return new WaitForSeconds(1);
        canvasObject.SetActive(false);
    }

    public void ButtonDown(int _num)
    {
        Debug.Log(_num + " ButtonDown: " + Time.time);
    }

    public void ButtonUp(int _num)
    {
        Debug.Log(_num + " ButtonDown: " + Time.time);
    }

    public IState GetNextState()
    {
        return nextState.GetComponent<IState>();
    }
}