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

    // Database 참조 및 count 변수 선언
    DatabaseReference reference;
    int count = 1;

    void Start()
    {
        // FIREBASE 시작 시점에 DatabaseReference의 인스턴스를 참조
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // 버튼 클릭 시
    public void OnClickSave()
    {
        //writeNewUser 함수로 user 정보, username, email 전송
        writeNewUser("personal infomation","goolgman","google@.com");
        count++;
    }

    // 불러오는 메소드
    public void LoadDataBtn()
    {
        readUser("personal infomation");
    }

    // 데이터베이스에 새 사용자를 쓰는 메소드
    public void writeNewUser(string userId, string name, string email)
    {
        //클래스 User를 통해 데이터를 만들어 name, email을 던짐
        User user = new User(name, email);
        //딕셔너리 자료형으로 user 정보를 json 형태로 변환
        string json = JsonUtility.ToJson(user);
        //Database의 child를 통해 값을 설정
        reference.Child(userId).Child("num" + count.ToString()).SetRawJsonValueAsync(json);
    }

    // 사용자를 읽어오는 메소드
    private void readUser(string userId)
    {
        //reference의 child에서 값을 비동기적으로 가져옴
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
                // snapshot의 자식 수를 로깅
                Debug.Log("Snapshot.ChildrenCount: " + snapshot.ChildrenCount);

                string s = "";
                //snapshot을 통해 데이터를 Dictionary 형태로 받아서 출력!
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
