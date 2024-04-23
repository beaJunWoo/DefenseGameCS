using UnityEngine;
using UnityEngine.UI;

public class SR_ChatMessage : MonoBehaviour
{
    public GameObject ChatBox;

    Color nickNameColor;
    Color messageColor;
    private void Start()
    {
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);
      
        nickNameColor = new Color(r, g, b);
        messageColor = new Color(r + 0.1f, g + 0.1f, b + 0.1f);
    }

    public void SpawnChat(string nickName, string message)
    {
        GameObject newChatBox = Instantiate(ChatBox,transform);
        Text Txt_nickName = newChatBox.GetComponent<Text>();
        Text Txt_message = newChatBox.transform.GetChild(0).transform.GetComponent<Text>();

        Txt_nickName.text = nickName + ":";
        Txt_message.text = message;

        Txt_nickName.color = nickNameColor;
        Txt_message.color = messageColor;

        newChatBox.GetComponent<RectTransform>().sizeDelta = new Vector2(13 * nickName.Length + 3, 30);
        newChatBox.transform.GetChild(0).transform.GetComponent<RectTransform>().sizeDelta = new Vector2(15 * message.Length, 30);
        Destroy(newChatBox, 10.0f);
    }
}
