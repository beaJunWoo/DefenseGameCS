using System.Collections;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class SR_FireBase : MonoBehaviour
{
    public Text text;
    public class User
    {
        public string username;
        public string email;

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }

    // Database ���� �� count ���� ����
    DatabaseReference reference;
    int count = 1;

    void Start()
    {
        // FIREBASE ���� ������ DatabaseReference�� �ν��Ͻ��� ����
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // ��ư Ŭ�� ��
    public void OnClickSave()
    {
        //writeNewUser �Լ��� user ����, username, email ����
        writeNewUser("personal infomation","goolgman","google@.com");
        count++;
    }

    // �ҷ����� �޼ҵ�
    public void LoadDataBtn()
    {
        readUser("personal infomation");
    }

    // �����ͺ��̽��� �� ����ڸ� ���� �޼ҵ�
    public void writeNewUser(string userId, string name, string email)
    {
        //Ŭ���� User�� ���� �����͸� ����� name, email�� ����
        User user = new User(name, email);
        //��ųʸ� �ڷ������� user ������ json ���·� ��ȯ
        string json = JsonUtility.ToJson(user);
        //Database�� child�� ���� ���� ����
        reference.Child(userId).Child("num" + count.ToString()).SetRawJsonValueAsync(json);
    }

    // ����ڸ� �о���� �޼ҵ�
    private void readUser(string userId)
    {
        //reference�� child���� ���� �񵿱������� ������
        reference.Child(userId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                // snapshot�� �ڽ� ���� �α�
                Debug.Log("Snapshot.ChildrenCount: " + snapshot.ChildrenCount);

                string s = "";
                //snapshot�� ���� �����͸� Dictionary ���·� �޾Ƽ� ���!
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary personInfo = (IDictionary)data.Value;
                    Debug.Log("email: " + personInfo["email"] + ", username: " + personInfo["username"] + ", num: " + personInfo["num"]);
                    s += "email: " + personInfo["email"] + ", username: " + personInfo["username"] + ", num: " + personInfo["num"];
                }
                text.text = s;
            }
        });
    }

}
