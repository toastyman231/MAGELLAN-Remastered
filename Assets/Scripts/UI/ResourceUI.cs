using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI crewText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldText;

    // Update is called once per frame
    void Update()
    {
        crewText.text = ResourceManager.instance.crew + "/" + ResourceManager.instance.crewMax;
        foodText.text = ResourceManager.instance.food + "/" + ResourceManager.instance.foodMax;
        goldText.text = ResourceManager.instance.gold.ToString();
    }
}
