using UnityEngine;

public class Connection : MonoBehaviour {
    private Module _parentModule;
    private Module _boundModule;

    public void SetBoundModule(Module module, Module baseModule) {
        _boundModule = module;
        module.SetBaseModule(baseModule);
        module.SetParentModule(_parentModule);

        //Simple rotation application. Depends on how we want to mount the new module later on. 
        module.transform.rotation = transform.rotation;
    }

    public Module GetBoundModule() {
        return _boundModule;
    }

    public void RemoveModule() {
        _boundModule.SetParentModule(null);
        _boundModule = null;
    }

    public Module GetParentModule() {
        return _parentModule;
    }

    public void SetParentModule(Module parentModule) {
        _parentModule = parentModule;
    }
}