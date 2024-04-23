using UnityEngine;
using UnityEngine.UI;

public class SR_User : MonoBehaviour
{
    public Text userName;
    private void Start()
    {
        userName.text = SR_GameManager.instance.nickName;
    }
}
