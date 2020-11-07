using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardView : MonoBehaviour
{
    public Text nameTextUI = null;
    public Text costTextUI = null;
    public Image graphicUI = null;

    public void Display(AbilityCard abilityCard)
    {
        nameTextUI.text = abilityCard.Name;
        costTextUI.text = abilityCard.Cost.ToString();
        graphicUI.sprite = abilityCard.Graphic;
    }
}
