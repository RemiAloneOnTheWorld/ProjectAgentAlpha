using UnityEngine;

[CreateAssetMenu(fileName = "NewModuleData", menuName = "ModuleData")]
public class ModuleData : ScriptableObject
{
    public new string name;
    public int health;
    public int price;
    public string description;
    public int destructionPrice;
}
