using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public enum Variable { CREW, FOOD };
    public static ResourceManager instance;

    public int storageMax;

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
            _food = value;
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

        crew = 270;
        food = 18;
        gold = 1600;
    }

    public void InitializeValues(int pCrew, int pGold, int pFood)
    {
        crew = pCrew;
        _gold = pGold;
        food = pFood;
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
                    return true;
                }

                break;
            case Variable.FOOD:
                if (val <= _foodMax)
                {
                    food = val;
                    return true;
                }

                break;
            default:
                return true;
        }

        return false;
    }
}

[System.Serializable]
public class Item
{
    private string _name;
    private int _cost;

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

    public Item(string pName, int pCost)
    {
        name = pName;
        cost = pCost;
    }
}
