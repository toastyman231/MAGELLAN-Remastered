using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChangeInputFieldValue : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_InputField otherInput;
    public ResourceManager.Variable var;
    public TextMeshProUGUI goldText;
    public string itemName;
    public int itemCost;
    public int initialValue;
    public bool validateInputs;
    private ResourceManager resources;
    private Item item;
    //private TextMeshProUGUI text;

    private void OnEnable()
    {
        //Debug.Log("Initialized!");
        //text = input.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //text = input.gameObject.GetComponent<TextMeshProUGUI>();
        //text.text = initialValue.ToString();
        input.text = initialValue.ToString();
        resources = ResourceManager.instance;
        item = new Item(itemName, itemCost);
    }

    public void IncreaseValue(int amount = 1)
    {
        int value = Convert.ToInt32(input.text);

        if (validateInputs)
        {
            if(resources.ValidateChange(var, value + amount)) 
            { 
                input.text = (value + amount).ToString();
                otherInput.text = resources.crewMax.ToString();
            }
        } else
        {
            input.text = (value + amount).ToString();
        }
        
    }

    public void DecreaseValue(int amount = 1)
    {
        int value = Convert.ToInt32(input.text);

        if (validateInputs)
        {
            if (resources.ValidateChange(var, value - amount)) 
            { 
                input.text = (value - amount).ToString();
                otherInput.text = resources.crewMax.ToString();
            }
        }
        else
        {
            input.text = (value - amount).ToString();
        }
    }

    public void IncreaseShopValue(int amount = 1)
    {
        int value = Convert.ToInt32(input.text);

        if (validateInputs)
        {
            if(resources.gold >= item.cost)
            {
                input.text = (value + amount).ToString();
                resources.gold -= item.cost;
                goldText.text = "Gold:\n" + resources.gold;
            }
        }
        else
        {
            input.text = (value + amount).ToString();
        }
    }

    public void DecreaseShopValue(int amount = 1)
    {
        int value = Convert.ToInt32(input.text);

        if (validateInputs)
        {
            if (value - amount >= 0)
            {
                input.text = (value - amount).ToString();
                resources.gold += item.cost;
                goldText.text = "Gold:\n" + resources.gold;
            }
        }
        else
        {
            input.text = (value - amount).ToString();
        }
    }
}
