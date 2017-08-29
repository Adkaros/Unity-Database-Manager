using UnityEngine;
using System.Collections;
using M1.Utilities;
using System;
using System.Collections.Generic;

[Serializable]
public class ReadState : MonoBehaviour, IState
{
    public GameObject canvasObject;
    public GameObject nextState;

    public List<DatabaseEntry> entries;
    public GameObject user;

    public Transform userList;

    public void Execute()
    {
    }

    public IEnumerator iEnter()
    {
        canvasObject.SetActive(true);

        //Get the last 5 entries from the database
        DatabaseManager.ReadEntry(5, UpdateUserList);

        GameManager.Instance.next += Next;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator iExit()
    {
        yield return new WaitForSeconds(1);
        canvasObject.SetActive(false);

        GameManager.Instance.next -= Next;
    }

    //Populate UI Table with entries
    private void CreateUserLabels()
    {
        foreach (DatabaseEntry e in entries)
        {
            GameObject go = GameObject.Instantiate(user);
            UserLabel ul = go.GetComponent<UserLabel>();

            ul.Set(e.id, e.date, e.name, e.age, e.phone);                       
            ul.transform.SetParent(userList);
            ul.transform.localPosition = Vector3.zero;
            ul.transform.localScale = Vector3.one;
        }
    }

    private void UpdateUserList(List<DatabaseEntry> _entries)
    {
        entries = _entries;
        CreateUserLabels();
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