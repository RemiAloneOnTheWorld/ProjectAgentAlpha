using UnityEngine;

public class ModuleDataWrapper : MonoBehaviour
{
    [SerializeField] private ModuleData moduleData;

    public string GetName()
    {
        return moduleData.name;
    }

    public int GetHealth()
    {
        return moduleData.health;
    }

    public string GetDescription()
    {
        return moduleData.description;
    }

    public int GetPrice() => moduleData.price;
}
