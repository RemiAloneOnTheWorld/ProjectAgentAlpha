using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Module : MonoBehaviour {
    [SerializeField] protected int health;
    public int Health { get; protected set; }

    [SerializeField] private int price;

    public int Price {
        get => price;
        private set => price = value;
    }

    private Module _parentModule;
    private Module _baseModule;

    public List<Connection> Connections { get; protected set; }

    public void SetParentModule(Module module) {
        _parentModule = module;
    }

    public void SetBaseModule(Module baseModule) {
        _baseModule = baseModule;
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
    }

    public virtual void ApplyDamage(in int amount) {
        Health -= amount;
        if (Health <= 0) {
            Debug.Log($"Module: {gameObject.name} destroyed");
        }
    }

    protected virtual void Start() {
        Health = health;
        Price = price;
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
                         connection.GetComponentInChildren<BoxCollider>().transform.localScale / 2)) {
                if (overlap.gameObject == gameObject || colliderObjects.Contains(overlap.gameObject) ||
                    overlap.gameObject == connection.GetComponentInChildren<BoxCollider>().gameObject) {
                    continue;
                }
                
                Destroy(connection.gameObject);
                hadOverlap = true;
                break;
                
            }

            if (!hadOverlap) {
                Connections.Add(connection);
                connection.SetParentModule(this);
            }
        }
    }
}