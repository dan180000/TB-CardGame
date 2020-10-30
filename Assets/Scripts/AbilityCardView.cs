using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardView : MonoBehaviour
{
    [SerializeField] Text nameTextUI = null;
    [SerializeField] Text costTextUI = null;
    [SerializeField] Image graphicUI = null;

    public void Display(AbilityCard abilityCard)
    {
        nameTextUI.text = abilityCard.Name;
        costTextUI.text = abilityCard.Cost.ToString();
        graphicUI.sprite = abilityCard.Graphic;
    }
}
