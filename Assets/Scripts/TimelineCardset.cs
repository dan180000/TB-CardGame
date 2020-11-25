using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineCardset : MonoBehaviour
{
    public Text AppliedCost;
    public Image Appliedimage;
    public Text AppliedName;

    [Header("Settings to apply to")]
    public Text Cost;
    public Image image;
    public Text Name;

    private void Update()
    {
        ApplySettings();
    }

    void ApplySettings()
    {
        Text timelineCostText = Cost.GetComponent(typeof(Text)) as Text;
        Image timelineImage = image.GetComponent(typeof(Image)) as Image;
        Text timelineNameText = Name.GetComponent(typeof(Text)) as Text;

        timelineCostText.text = AppliedCost.text;
        timelineImage.sprite = Appliedimage.sprite;
        timelineNameText.text = AppliedName.text;
    }
}
