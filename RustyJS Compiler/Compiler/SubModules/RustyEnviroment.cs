internal class RustyEnvironment {
    private Dictionary<string, RuntimeValueTypeNode> _variables;
    private RustyEnvironment? _parent;

    public RustyEnvironment(RustyEnvironment? parent) {
        _variables = new Dictionary<string, RuntimeValueTypeNode>();
        _parent = parent;
    }

    public RuntimeValueTypeNode DeclareVariableInScope(string name, RuntimeValueTypeNode value) {
        if (_variables.ContainsKey(name)) RustyErrorHandler.Error($"Cannot declare \"{name}\" variable in the same scope.", 2098);
        _variables[name] = value;
        return value;
    }

    public RuntimeValueTypeNode AssignVariableInScope(string name, RuntimeValueTypeNode value) {
        RustyEnvironment scope = ResolveEnvironment(name);
        scope._variables[name] = value;
        return value;
    }

    public RuntimeValueTypeNode GetVariable(string name) {
        RustyEnvironment scope = ResolveEnvironment(name);
        return scope._variables[name];
    }


    public RustyEnvironment ResolveEnvironment(string name) {
        if (_variables.ContainsKey(name)) return this;

        if (_parent == null) RustyErrorHandler.Error($"Cannot find variable: \"{name}\" in this scope.", 3000);

        return _parent.ResolveEnvironment(name);
    }


}

