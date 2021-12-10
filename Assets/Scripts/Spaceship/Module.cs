using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        //This only grabs the sub-gameobjects with 'Connection' script.
        Component[] components = transform.GetComponentsInChildren(typeof(Connection));

        //Add it to list so 'contains' can be called in the overlap test.
        List<GameObject> connectors = new List<GameObject>();

        foreach (var component in components) {
            connectors.Add(component.gameObject);
        }

        foreach (var connection in components) {
            //Todo: Use same displacement vector in connectors and here.
            var connectionTransform = connection.transform;
            var moduleDisplacement = transform.position - connectionTransform.position;
            var displacementVector = connectionTransform.right;
            if (Vector3.Dot(moduleDisplacement.normalized, displacementVector) > 0) {
                displacementVector = -displacementVector;
            }

            //TODO: Make this dependent on the actual size of the module.
            displacementVector *= transform.lossyScale.x / 2;

            //TODO: Make the half-extents dependent on the actual size of the module.
            foreach (var currentCollider in Physics.OverlapBox(connection.transform.position + displacementVector,
                transform.lossyScale * 0.3f)) {
                //Check if collision occured with own connections and module.
                if (currentCollider.gameObject == gameObject || connectors.Contains(currentCollider.gameObject)) {
                    continue;
                }

                Destroy(connection.gameObject);

                //Tries to get the 'Connection' component to ensure that it is indeed a connection
                //and not another module.
                if (currentCollider.TryGetComponent<Connection>(out var component)) {
                    //'RemoveConnection' also deletes the connections' game object.
                    if (component.GetBoundModule() == null) {
                        component.GetParentModule().RemoveConnection(component);
                    }
                }
            }

            Connections.Add((Connection) connection);
            ((Connection) connection).SetParentModule(this);
        }
    }
}