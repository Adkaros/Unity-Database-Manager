using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserLabel : MonoBehaviour
{
    public Text idText, dateText, nameText, ageText, phoneText;

    public void Set(string _id, string _date, string _name, string _age, string _phone)
    {
        idText.text = _id;
        dateText.text = _date;
        nameText.text = _name;
        ageText.text = _age;
        phoneText.text = _phone;
    }
}
