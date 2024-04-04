using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{

    public Slider progressBar;
    public Text loadText;

    public Image tipImg;
    public Sprite[] tipSprite;

    float waitTime = 2.0f;

    private void Start()
    {
        StartCoroutine(LoadScene());
        progressBar.value = 0;

        int randomIdx = Random.Range(0, tipSprite.Length);
        tipImg.sprite = tipSprite[randomIdx];
    }
    private void Update()
    {
        if (waitTime > 0)
        {
            progressBar.value = Mathf.Lerp(progressBar.value, 1.0f, Time.deltaTime / waitTime);
        }
        waitTime -= Time.deltaTime;
    }
    IEnumerator LoadScene()
    {
        string scene = GameObject.Find("GameManager").GetComponent<SR_GameManager>().GetSceneName();

        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
           
            if (progressBar.value >= 1f)
            {
                loadText.text = "LoadComplete..!!";
            }

            if (operation.progress >= 0.9f&& waitTime < 0f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
