using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EntryDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText, _usernameText, _scoreText;

    public void SetEntry(int rank, string nickName, int score,bool mine)
    {
        _rankText.text = rank.ToString();
        _usernameText.text = nickName;
        _scoreText.text = score.ToString();

        GetComponent<Image>().color = mine ? Color.yellow : Color.white;
    }
}