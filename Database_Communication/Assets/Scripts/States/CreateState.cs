using UnityEngine;
using System.Collections;
using M1.Utilities;
using System;
using UnityEngine.UI;

[Serializable]
public class CreateState : MonoBehaviour, IState
{
    public GameObject canvasObject;
    public GameObject nextState;

    public InputField nameField, ageField, phoneField;

    public void Execute()
    {
    }

    public IEnumerator iEnter()
    {
        Debug.Log("1 Enter: " + Time.time);
        canvasObject.SetActive(true);

        nameField.text = "";
        ageField.text = "";
        phoneField.text = "";

        GameManager.Instance.next += Next;

        yield return new WaitForSeconds(1);
    }

    public IEnumerator iExit()
    {
        Debug.Log("1 Exit: " + Time.time);
        yield return new WaitForSeconds(1);
                      

        canvasObject.SetActive(false);
        GameManager.Instance.next -= Next;
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
        GameManager.user.name = nameField.text;
        GameManager.user.age = ageField.text;
        GameManager.user.phone = phoneField.text;

        DatabaseManager.CreateEntry(GameManager.user);
        GameManager.NextState();
    }
}