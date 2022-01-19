using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Module : MonoBehaviour {
    [SerializeField] private ModuleData moduleData;
    public int CurrentHealth { get; private set; }

    public int Price { get; private set; }

    private Module _parentModule;
    private Module _baseModule;

    public List<Connection> Connections { get; protected set; }

    public void SetParentModule(Module module) {
        _parentModule = module;
    }

    public void SetBaseModule(Module baseModule) {
        _baseModule = baseModule;
    }

    public string GetModuleName() {
        return moduleData.name;
    }

    public string GetModuleDescription() {
        return moduleData.description;
    }

    public int GetStartingHealth() {
        return moduleData.health;
    }

    public int ConnectionCount() {
        return Connections.Count(connection => connection.GetBoundModule() != null);
    }

    public Module GetParentModule() {
        return _parentModule;
    }

    public Module GetBaseModule() {
        return _baseModule;
    }

    public void RemoveConnection(Connection connection) {
        try {
            Connections.Remove(connection);
            Destroy(connection.gameObject);
        }
        catch (Exception e) {
            Debug.LogError("A non-existing connection was tried to be removed: " + e);
        }
    }

    //This method may seems redundant, since modules can be removed from the connections directly, yet
    //modules will probably be removed based on the module they're bound to, opposing the docking, where
    //the clicked connection is of importance.
    public void RemoveDockedModule(Module module) {
        for (int i = Connections.Count - 1; i >= 0; i--) {
            if (Connections.ElementAt(i).GetBoundModule() == module) {
                Connections.ElementAt(i).RemoveModule();
            }
        }
    }

    public virtual void DestroyModule() {
        //Destroy the game object for now.
        _parentModule.RemoveDockedModule(this);

        //TODO: Check if currency suffices.

        foreach (var module in Connections) {
            RemoveDockedModule(module.GetBoundModule());
        }

        Destroy(gameObject);
    }

    //Calls method on sub-modules recursively.
    public virtual void DestroyModuleWithSubs() {
        _parentModule.RemoveDockedModule(this);
        foreach (var connection in Connections) {
            if (connection.GetBoundModule() != null) {
                connection.GetBoundModule().DestroyModuleWithSubs();
            }
            //connection.GetBoundModule().DestroyModuleWithSubs();
        }

        Destroy(gameObject);
    }

    public virtual void ResetModule() {
        CurrentHealth = moduleData.health;
    }

    public virtual void ApplyDamage(in int amount) {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0) {
            Debug.Log($"Module: {gameObject.name} destroyed");
        }
    }

    protected virtual void Start() {
        CurrentHealth = moduleData.health;
        Price = moduleData.price;
        Connections = new List<Connection>();
        RemoveOverlapConnections();
    }


    private void RemoveOverlapConnections() {

        Connection[] connections = gameObject.GetComponentsInChildren<Connection>();
        List<GameObject> colliderObjects = new List<GameObject>();

        foreach (var connection in connections) {
            colliderObjects.Add(connection.gameObject);
            colliderObjects.Add(connection.GetComponentInChildren<BoxCollider>().gameObject);
        }

        foreach (var connection in connections) {
            bool hadOverlap = false;
            foreach (var overlap in Physics.OverlapBox(connection.transform.position,
                         connection.GetComponentInChildren<BoxCollider>().transform.lossyScale / 2)) {

                if (overlap.gameObject == gameObject || colliderObjects.Contains(overlap.gameObject) ||
                    overlap.gameObject == connection.GetComponentInChildren<BoxCollider>().gameObject) {
                    continue;
                }

                Destroy(connection.gameObject);
                hadOverlap = true;
                Debug.LogWarning("Overlap detected");
                break;

            }

            if (!hadOverlap) {
                Connections.Add(connection);
                connection.SetParentModule(this);
            }
        }
    }

    public int GetDestructionCost() {
        int costModules = moduleData.destructionPrice;
        foreach (var currentConnection in Connections) {
            Module module = currentConnection.GetBoundModule();
            if (module != null) {
                costModules += module.GetDestructionCost();
            }
        }

        return costModules;
    }
}