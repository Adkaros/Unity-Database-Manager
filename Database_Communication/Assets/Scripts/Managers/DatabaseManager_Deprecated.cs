using UnityEngine;
using System.Collections;
using M1.Utilities;
using System.Collections.Generic;
using System;
using System.IO;

public class DatabaseEntryOld
{
    public int id;
    public string date;
    public string name;
    public string age;
    public string email;
    public string phone;
    public string song_id;
    public string theme_id;
    public string image_id;
    public string video_id;
    public string download_url;
    public string finished_performance = "0";
    public string consented = "0";
    public string parent_phone = "0";
    public string parent_name = "0";

    public DatabaseEntryOld() { }

    public DatabaseEntryOld(string _data)
    {
        string[] data = _data.Split('|');
        if (data.Length > 13)
        {
            id = int.Parse(data[0]);
            date = data[1];
            name = data[2];
            age = data[3];
            email = data[4];
            phone = data[5];
            song_id = data[6];
            theme_id = data[7];
            image_id = data[8];
            video_id = data[9];
            download_url = data[10];

            consented = data[11];
            parent_phone = data[12];
            parent_name = data[13];
        }
    }

    public string Print()
    {
        string s = "";
        s += "id: " + id + "\n";
        s += "date: " + date + "\n";
        s += "name: " + name + "\n";
        s += "age: " + age + "\n";
        s += "email: " + email + "\n";
        s += "phonenumber: " + phone + "\n";
        s += "song_name: " + song_id + "\n";
        s += "song_name: " + theme_id + "\n";
        s += "image_id: " + image_id + "\n";
        s += "video_id: " + video_id + "\n";
        s += "download_url: " + download_url;
        s += "finishede: " + finished_performance + "\n";
        return s;
    }
}

public enum SONG_TABLE
{
    dance,
    listen,
    sing
}

public class DatabaseManagerReference : SingletonBehaviour<DatabaseManagerReference>
{
    private static string ip = "127.0.0.1:8080";
    private static string activation = "listen";
    [HideInInspector]
    public static string selectedGroup = "1";

    //private string registrationURL_RECEIVE = "http://databashing.com/demo/Coke/ReceiveRegistrationData.php?";
    //private string registrationURL_SEND = "http://databashing.com/demo/Coke/SendRegistrationData.php?";
    //private string songListURL_RECEIVE = "http://databashing.com/demo/Coke/GetSongList.php?";
    //private string userListURL_RECEIVE = "http://databashing.com/demo/Coke/GetUserList.php?";
    //static string registrationURL_UPDATE = "http://databashing.com/demo/Coke/UpdateRegistrationData.php?";

    static string URL_add_local { get { return "http://" + ip + "/AddUser.php?"; } }
    static string URL_get_local { get { return "http://" + ip + "/GetUser.php?"; } }
    static string URL_getUserList_local { get { return "http://" + ip + "/GetUserList.php?"; } }
    static string URL_getSongList_local { get { return "http://" + ip + "/GetSongList.php?"; } }
    static string URL_update_local { get { return "http://" + ip + "/UpdateUser.php?"; } }
    static string URL_SyncDatabase { get { return "http://" + ip + "/SyncTable.php?table=" + activation + "_" + selectedGroup; } }

    private string secretKey = "mySecretKey";

    public delegate void UserListCallback(List<DatabaseEntryOld> _userList);
    public delegate void UserCallback(DatabaseEntryOld _userList);
    public delegate void SongListCallback(string[] _songList);

    // test data
    public string personname = "test name";
    public string age = "99";
    public string email = "email";
    public string phone_number = "12345678";
    public string song_id = "1";
    public string theme_id = "2";

    void Awake()
    {
        //if (Config.HasKey(CONFIG_KEYS.group))
        //{
        //    selectedGroup = Config.Read(CONFIG_KEYS.group);
        //}
        //if (Config.HasKey(CONFIG_KEYS.ip))
        //{
        //    ip = Config.Read(CONFIG_KEYS.ip);
        //}

    }

    void Callback(List<DatabaseEntryOld> _userList)
    {
        foreach (DatabaseEntryOld d in _userList)
        {
            //UIDebug.Log(d.Print());
        }
    }

    void Callback(DatabaseEntryOld _user)
    {
        //UIDebug.Log(_user.Print());
    }

    void Callback(string[] _songList)
    {
        foreach (string s in _songList)
        {
            //UIDebug.Log(s);
        }
    }

    public static void ReceiveRegistration(int _groupNum, int _user_ID, UserCallback _callback)
    {
        Instance.StartCoroutine(Instance.iReceiveRegistration(_groupNum, _user_ID, _callback));
    }
    private IEnumerator iReceiveRegistration(int _groupNum, int _user_ID, UserCallback _callback)
    {
        string key = Md5Sum(secretKey);
        string post_url = URL_add_local + "&groupNum=" + WWW.EscapeURL(_groupNum.ToString()) +
                                          "&id=" + _user_ID;
        WWW www = new WWW(post_url);
        yield return www;
        _callback(new DatabaseEntryOld(www.text));
    }

    public static void SendRegistration(DatabaseEntryOld _user)
    {
        Instance.StartCoroutine(Instance.iSendRegistration(_user));
    }
    private IEnumerator iSendRegistration(DatabaseEntryOld _user)
    {
        string hash = Md5Sum(secretKey);
        string post_url = URL_add_local + "&name=" + WWW.EscapeURL(_user.name) +
                                          "&age=" + WWW.EscapeURL(_user.age) +
                                          "&email=" + WWW.EscapeURL(_user.email) +
                                          "&phone=" + WWW.EscapeURL(_user.phone) +
                                          "&song_id=" + WWW.EscapeURL(_user.song_id) +
                                          "&theme_id=" + WWW.EscapeURL(_user.theme_id) +
                                          "&consented=" + WWW.EscapeURL(_user.consented) +
                                          "&parent_phone=" + WWW.EscapeURL(_user.parent_phone) +
                                          "&parent_name=" + WWW.EscapeURL(_user.parent_name);
        WWW www = new WWW(post_url);
        //UIDebug.Log(post_url);
        yield return www;

        if (www.error != null)
        {
            //UIDebug.Log("php error: " + www.error);
        }
        else
        {
            //UIDebug.Log("www.text: " + www.text);

            //Set ID here
            //GameManager.Instance.newUser.id = www.text.SafeParse();            
        }
    }

    public static void GetSongList(SONG_TABLE _tableName, SongListCallback _callback)
    {
        Instance.StartCoroutine(Instance.iGetSongList(_tableName, _callback));
    }
    private IEnumerator iGetSongList(SONG_TABLE _tableName, SongListCallback _callback)
    {
        Debug.Log(WWW.EscapeURL(_tableName.ToString()));
        WWW www = new WWW(URL_getSongList_local + "&table=" + "song_list_" + WWW.EscapeURL(_tableName.ToString()));
        yield return www;

        if (www.error != null) print("php error: " + www.error);
        else
        {
           // UIDebug.Log(www.text);
            string[] data = www.text.Split('\n');
            _callback(data);
        }
    }

    public static void GetUserList(UserListCallback _callback)
    {
        Instance.StartCoroutine(Instance.iGetUserList(_callback));
    }
    private IEnumerator iGetUserList(UserListCallback _callback)
    {
        string post_url = URL_getUserList_local;

        print(post_url);

        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            //UIDebug.Log("php error: " + www.error);

            if (_callback != null)
                _callback(null);
        }
        else
        {
            //UIDebug.Log("www.text: " + www.text);
            if (www.text != "")
            {
                string[] data = www.text.Split('\n');
                List<DatabaseEntryOld> userList = new List<DatabaseEntryOld>();
                foreach (string s in data)
                {
                    userList.Add(new DatabaseEntryOld(s));
                }

                // send userList to selection screen
                if (_callback != null)
                    _callback(userList);
            }
            else
            {
                if (_callback != null)
                    _callback(null);
            }
        }
    }

    public static void UserFinishedPerformance(DatabaseEntryOld _user)
    {
        Instance.StartCoroutine(Instance.iUserFinishedPerformance(_user));
    }

    IEnumerator iUserFinishedPerformance(DatabaseEntryOld _user)
    {
        string hash = Md5Sum(secretKey);
        string post_url = URL_update_local +
                          "&id=" + _user.id +
                          "&target=" + "finished" +
                          "&value=" + "1";

        Debug.Log(post_url);
        WWW www = new WWW(post_url);
        yield return www;
        Debug.Log(www.text);
    }



    public static void SendPhotoToServer(int id, Texture2D tex)
    {
        Instance.StartCoroutine(Instance.iSendPhotoToServer(id, tex));
    }

    private IEnumerator iSendPhotoToServer(int id, Texture2D tex)
    {
        if (id <= 0) yield break;
        //if (!GameManager.Instance.agreedToWaiver) yield break;
        

        string key = Md5Sum("75idf.jnjKel");
        //string uploadURL = "http://share-a-coke.net-server.us/api/?";


        byte[] bytes = tex.EncodeToJPG();

        //WWWForm form = new WWWForm();

        string timeStamp = System.DateTime.Now.ToLongTimeString().Remove(System.DateTime.Now.ToLongTimeString().Length - 3);
        string dateStamp = System.DateTime.Now.ToShortDateString();

        dateStamp = dateStamp.Replace("/", "");
        timeStamp = timeStamp.Replace(":", "");

        //string photoFileName = "(" + id.ToString() + ")" + "image_" + dateStamp + "-" + timeStamp + "_" + GetFileTag(GameManager.Instance.newUser.age.SafeParse()) + ".jpg";
        Debug.Log("before print");
        //PrinterPlugin.print(tex, false, PrinterPlugin.PrintScaleMode.FILL_PAGE); 
        //File.WriteAllBytes("C://CokePhotos/" + photoFileName, bytes);
        
        yield return null;
        //form.AddBinaryData("file", bytes, photoFileName, "image/jpg");
        //
        //WWW w = new WWW(uploadURL + "action=" + "save_photo" + "&hash=" + key, form);
        //yield return w;
        //
        //if (w.error != null)
        //{
        //    Debug.Log(w.error);
        //    Application.ExternalCall("debug", w.error);
        //}
        //else
        //{
        //    Debug.Log(w.text);
        //    Debug.Log("Finished Uploading Screenshot");
        //    Application.ExternalCall("debug", "Finished Uploading Screenshot");
        //
        //
        //    GameManager.Instance.newUser.image_id = photoFileName;
        //    SendRegistration(GameManager.Instance.newUser);
        //    //SendUserToServer(groupNum, photoFileName);
        //}

    }

    private static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

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

    public string GetFileTag(int age)
    {
        string returnVal = "";

        switch (selectedGroup)
        {
            case "1":
                returnVal = "GROUP1-";
                break;
            case "2":
                returnVal = "GROUP2-";
                break;
            case "3":
                returnVal = "GROUP3-";
                break;
        }

        if (age < 13)
        {
            returnVal += "UNDERTHIRTEEN";
        }
        else if (age > 12 && age < 18)
        {
            returnVal += "THIRTEENTOSEVENTEEN";
        }
        else if (age > 17)
        {
            returnVal += "OVEREIGHTEEN";
        }

        return returnVal;
    }

}
