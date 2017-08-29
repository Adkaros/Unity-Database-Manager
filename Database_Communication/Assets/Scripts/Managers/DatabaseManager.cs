using UnityEngine;
using System.Collections;
using M1.Utilities;
using System.Collections.Generic;

public class DatabaseManager : SingletonBehaviour<DatabaseManager>
{
    private static string serverPath = "http://databashing.com/demo/TestData/";

    private static string URL_Create { get { return serverPath + "CreateEntry.php?"; } }
    private static string URL_Read { get { return serverPath + "ReadEntry.php?"; } }
    private static string URL_Update { get { return serverPath + "UpdateEntry.php?"; } }
    private static string URL_Delete { get { return serverPath + "DeleteEntry.php?"; } }

    private string clientKey = "mySecretKey";

    public delegate void UserCallback(List<DatabaseEntry> _users);

    //CREATE
    public static void CreateEntry(DatabaseEntry _user)
    {
        Instance.StartCoroutine(Instance.iCreateEntry(_user));
    }

    private IEnumerator iCreateEntry(DatabaseEntry _user)
    {
        string hash = Md5Sum(clientKey);

        var urlParams = new Dictionary<string, string>
        {
            { "name", _user.name },
            { "age", _user.age },
            { "phone", _user.phone },
            { "hash", hash }
        };

        string post_url = EncodeURLContent(URL_Create, urlParams);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("php error: " + www.error);
        }
        else
        {
            //Store the current user's ID
            GameManager.user.id = www.text;
            Debug.Log("www.text: " + www.text);     
        }
    }

    //READ
    public static void ReadEntry(int _numEntries, UserCallback _callback)
    {
        Instance.StartCoroutine(Instance.iReadEntry(_numEntries, _callback));
    }

    private IEnumerator iReadEntry(int _numEntries, UserCallback _callback)
    {
        string hash = Md5Sum(clientKey);

        var urlParams = new Dictionary<string, string>
        {
            { "number_entries", _numEntries.ToString() },
            { "hash", hash }
        };

        string post_url = EncodeURLContent(URL_Read, urlParams);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("php error: " + www.error);
        }
        else
        {
            //Parse raw data from HTTP response
            string[] data = www.text.Split('\n');
            List<DatabaseEntry> entries = new List<DatabaseEntry>();

            //Add each entry to a list of entries
            foreach (string entry in data)
                entries.Add(new DatabaseEntry(entry));

            _callback(entries);
        }
        
    }

    //UPDATE
    public static void UpdateEntry(int _user_id, string _targetField, string _targetValue)
    {
        Instance.StartCoroutine(Instance.iUpdateEntry(_user_id, _targetField, _targetValue));
    }

    IEnumerator iUpdateEntry(int _user_id, string _targetField, string _targetValue)
    {
        string hash = Md5Sum(clientKey);

        var urlParams = new Dictionary<string, string>
        {
            { "id", _user_id.ToString() },
            { "target", _targetField },
            { "value", _targetValue },
            { "hash", hash }
        };

        string post_url = EncodeURLContent(URL_Update, urlParams);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("php error: " + www.error);
        }
        else
        {
            Debug.Log("www.text: " + www.text);
        }
    }


    //DELETE
    public static void DeleteEntry(int _user_id)
    {
        Instance.StartCoroutine(Instance.iDeleteEntry(_user_id));
    }

    private IEnumerator iDeleteEntry(int _user_id)
    {
        string hash = Md5Sum(clientKey);

        var urlParams = new Dictionary<string, string>
        {
            { "id", _user_id.ToString() },
            { "hash", hash }
        };

        string post_url = EncodeURLContent(URL_Read, urlParams);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("php error: " + www.error);
        }
        else
        {
            Debug.Log("www.text: " + www.text);
        }

    }

    private string EncodeURLContent(string _filePath, Dictionary<string, string> _params)
    {
        foreach (KeyValuePair<string, string> p in _params)
        {
            _filePath += "&" + p.Key + "=" + WWW.EscapeURL(p.Value);
        }

        return _filePath;
    }

    private static string Md5Sum(string _strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(_strToEncrypt);

        // Encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }
}

public class DatabaseEntry
{
    private const int NUM_FIELDS = 5;

    public string id;
    public string date;
    public string name;
    public string age;
    public string phone;

    public DatabaseEntry() { }

    public DatabaseEntry(string _data)
    {
        //Tokenize retrieved data
        string[] data = _data.Split('|'); 

        if (data.Length >= NUM_FIELDS)
        {
            id = data[0];
            date = data[1];
            name = data[2];
            age = data[3];
            phone = data[4];
        }
    }

    public string GetDebugInfo()
    {
        string s = "";
        s += "id: " + id + "\n";
        s += "date: " + date + "\n";
        s += "name: " + name + "\n";
        s += "age: " + age + "\n";
        s += "phonenumber: " + phone;
        return s;
    }
}
