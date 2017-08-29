using UnityEngine;
using System.Collections;
using M1.Utilities;
using System;

[Serializable]
public class UpdateState : MonoBehaviour, IState
{
    public GameObject canvasObject;
    public GameObject nextState;

    public UserLabel preUpdateLabel;
    public UserLabel postUpdateLabel;

    public void Execute()
    {
    }

    public IEnumerator iEnter()
    {
        Debug.Log("1 Enter: " + Time.time);
        canvasObject.SetActive(true);

        preUpdateLabel.Set(GameManager.user.id, GameManager.user.date, GameManager.user.name, GameManager.user.age, GameManager.user.phone);
        postUpdateLabel.Set(GameManager.user.id, GameManager.user.date, GameManager.user.name, GameManager.user.age, GameManager.user.phone);
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