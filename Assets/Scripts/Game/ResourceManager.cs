using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public enum Variable { CREW, FOOD };
    public static ResourceManager instance;

    public int storageMax;
    public int startingCrew;

    public Item[] defaultItems;
    public ArrayList items;
    
    private int _crew; 
    private int _crewMax;

    private int _gold;

    private int _food;
    private int _foodMax;
    
    public int crew
    {
        get => _crew;
        set
        {
            _crew = value;
            RecalculateMaxes();
        }
    }

    public int gold
    {
        get => _gold;
        set => _gold = value;
    }

    public int crewMax
    {
        get => _crewMax;
        set => _crewMax = value;
    }

    public int foodMax
    {
        get => _foodMax;
        set => _foodMax = value;
    }

    public int food
    {
        get => _food;
        set
        {
            _food = value <= foodMax ? value : foodMax;
            RecalculateMaxes();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }

        items = new ArrayList();
        defaultItems = new[]
        {
            new Item("Weapons", 100, 1),
            new Item("Ammo", 100, 1),
            new Item("Boards", 100, 1),
            new Item("Spices", 100, 1)
        };

        _crewMax = 270;
        _foodMax = 36;
        _gold = 300;
    }

    public void InitializeValues(int pCrew, int pGold, int pFood)
    {
        _crew = pCrew;
        _gold = pGold;
        _food = pFood;
    }

    public void RecalculateMaxes()
    {
        _crewMax = Mathf.FloorToInt(_food * 7.5f);
        _foodMax = storageMax - _crew;
    }

    public bool ValidateChange(Variable varToCheck, int val)
    {
        switch (varToCheck)
        {
            case Variable.CREW:
                if(val <= _crewMax)
                {
                    crew = val;
                    _crewMax = Mathf.FloorToInt(_foodMax * 7.5f);
                    return true;
                }

                break;
            case Variable.FOOD:
                if (val <= _foodMax)
                {
                    food = val;
                    _foodMax = storageMax - _crewMax;
                    return true;
                }

                break;
            default:
                return true;
        }

        return false;
    }
}

[Serializable]
public class Item
{
    private string _name;
    private int _cost;
    private int _modifier;

    public int modifier
    {
        get => _modifier;
        set => _modifier = value;
    }
    public string name
    {
        get => _name;
        set => _name = value;
    }

    public int cost
    {
        get => _cost;
        set => _cost = value;
    }

    // purchaseFunc should only give the player their item, gold handling is done in Purchase itself
    public IEnumerator Purchase()
    {
        if (ResourceManager.instance.gold < cost)
        {
            DialogueController.instance.AcceptInput("Not enough gold for this item!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            yield break;
        }

        ResourceManager.instance.gold -= cost;
        ResourceManager.instance.StartCoroutine(UpdateResources());
    }

    private IEnumerator UpdateResources()
    {
        int count = name.IndexOf(" ");
        switch (name.Remove(0, count > 0 ? count + 1 : 0))
        {
            default:
                // TODO: Check if there's room for this item
                ResourceManager.instance.items.Add(this);
                DialogueController.instance.AcceptInput("You purchased " + name + "!");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                yield break;
            case "Food":
                if (ResourceManager.instance.food + modifier > ResourceManager.instance.foodMax)
                {
                    DialogueController.instance.AcceptInput("You don't have room for any more food!");
                    yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                    yield break;
                }
                ResourceManager.instance.food += modifier;
                DialogueController.instance.AcceptInput("You purchased 1 Food!");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                yield break;
            case "Crew":
                if (ResourceManager.instance.crew + modifier > ResourceManager.instance.crewMax)
                {
                    DialogueController.instance.AcceptInput("You don't have room for any more crew members!");
                    yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                    yield break;
                }
                ResourceManager.instance.crew += modifier;
                DialogueController.instance.AcceptInput("You recruited 40 new crew members!");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                yield break;
        }
    }

    public Item(string pName, int pCost)
    {
        name = pName;
        cost = pCost;
        modifier = 1;
    }
    
    public Item(string pName, int pCost, int pMod)
    {
        name = pName;
        cost = pCost;
        modifier = pMod;
    }
}
