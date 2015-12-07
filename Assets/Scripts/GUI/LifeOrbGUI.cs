using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class LifeOrbGUI : MonoBehaviour
{
    public Image background;
    public Image inside;
    public Image shine;

    public void ChangeHealth(float percent)
    {
        //Updates this lifeOrb's health.
        inside.rectTransform.localScale = new Vector2(percent, percent);
    }
}
