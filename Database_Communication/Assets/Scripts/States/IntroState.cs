using UnityEngine;
using System.Collections;
using M1.Utilities;
using System;

[Serializable]
public class IntroState : MonoBehaviour, IState
{
    public GameObject canvasObject;
    public GameObject nextState;

    public void Execute()
    {
    }

    public IEnumerator iEnter()
    {
        canvasObject.SetActive(true);
        GameManager.Instance.next += Next;

        yield return new WaitForSeconds(1);
    }

    

    public IEnumerator iExit()
    {
        yield return new WaitForSeconds(1);
        
        GameManager.Instance.next -= Next;
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

    private void Next()
    {
        GameManager.NextState();
    }
}