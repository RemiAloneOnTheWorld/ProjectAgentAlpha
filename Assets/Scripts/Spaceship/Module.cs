using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Module : MonoBehaviour {
    [SerializeField] protected int health;
    public int Health { get; protected set; }

    private Module _parentModule;
    private Module _baseModule;

    public List<Connection> Connections { get; protected set; }

    public void SetParentModule(Module module) {
        _parentModule = module;
    }

    public void SetBaseModule(Module baseModule) {
        _baseModule = baseModule;
    }

    public Module GetBaseModule() {
        return _baseModule;
    }

    //This method may seems redundant, since modules can be removed from the connections directly, yet
    //modules will probably be removed based on the module they're bound to, opposing the docking, where
    //the clicked connection is of importance.
    public void RemoveDockedModule(Module module) {
        for (int i = Connections.Count; i >= 0; i--) {
            if (Connections.ElementAt(i).GetBoundModule() == module) {
                Connections.ElementAt(i).RemoveModule();
            }
        }
    }

    public virtual void DestroyModule() {
        //Destroy the game object for now.
        _parentModule.RemoveDockedModule(this);
        foreach (var module in Connections) {
            RemoveDockedModule(module.GetBoundModule());
        }
        Destroy(gameObject);
    }

    public virtual void ResetModule() {
        Health = health;
        //Connections.Clear();
    }

    public virtual void ApplyDamage(in int amount) {
        Health -= amount;
        if (Health <= 0) {
            Debug.Log($"Module: {gameObject.name} destroyed");
        }
    }

    protected virtual void Start() {
        Health = health;
        Connections = new List<Connection>();
        foreach (var connection in transform.GetComponentsInChildren(typeof(Connection))) {
            Debug.Log("Found connection");
            Connections.Add((Connection) connection);
            ((Connection) connection).SetParentModule(this);
        }
    }
}