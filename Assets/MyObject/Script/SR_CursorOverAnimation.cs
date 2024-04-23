using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SR_CursorOverAnimation : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    public enum AnimationType { left, right };

    public AnimationType aniType;

    public Image img;

    string DefaultId;
    string OffId;
    private void Start()
    {
        if (aniType == AnimationType.left)
        {
            DefaultId = "L_DefaultOn";
            OffId = "L_DefaultOff";
        }
        else if (aniType == AnimationType.right)
        {
            DefaultId = "R_DefaultOn";
            OffId = "R_DefaultOff";
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.Restart(DefaultId);

        Color newColor = new Color(0.8f, 0.8f, 0.8f);
        img.color = newColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DOTween.Restart(OffId);
        Color newColor = new Color(1f, 1f, 1f);
        newColor.a = 0.8f;
        img.color = newColor;
    }
}