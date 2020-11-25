using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSet : MonoBehaviour
{
    public Text Cost;
    public Image image;
    public Text Name;

    public void ResetSettings()
    {
        Text AC = Cost.GetComponent(typeof(Text)) as Text;
        Image AI = image.GetComponent(typeof(Image)) as Image;
        Text AN = Name.GetComponent(typeof(Text)) as Text;

        AC.text = null;
        AI.sprite = null;
        AN.text = null;
    }
}
