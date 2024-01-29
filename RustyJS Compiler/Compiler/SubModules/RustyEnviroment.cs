internal class RustyEnvironment {
    private Dictionary<string, RustyVariable> _variables;
    private RustyEnvironment? _parent;

    public RustyEnvironment(RustyEnvironment? parent=null) {
        _variables = new Dictionary<string, RustyVariable>();
        _parent = parent;
    }

    public RuntimeValueTypeNode DeclareVariableInScope(string name, RuntimeValueTypeNode value, bool isMuttable, bool isConstant) {
        if (_variables.ContainsKey(name)) {
            RustyVariable var = _variables[name];

            if (isConstant) RustyErrorHandler.Error($"Cannot re-assign constant \"{name}\" variable in the same scope. Use: \"mut\" insted.", 2098);
            if (!var.Muttable) RustyErrorHandler.Error($"Cannot re-declare not mutable \"{name}\" variable in the same scope. Use: \"mut\" insted.", 2098);
        } 
        _variables[name] = new RustyVariable(isConstant ,isMuttable, value);
        return value;
    }

    public RuntimeValueTypeNode AssignVariableInScope(string name, RuntimeValueTypeNode value) {
        RustyEnvironment scope = ResolveEnvironment(name);
        scope._variables[name].Value = value;
        return value;
    }

    public RuntimeValueTypeNode GetVariable(string name) {
        RustyEnvironment scope = ResolveEnvironment(name);
        return scope._variables[name].Value;
    }


    public RustyEnvironment ResolveEnvironment(string name) {
        if (_variables.ContainsKey(name)) return this;

        if (_parent == null) RustyErrorHandler.Error($"Cannot find variable: \"{name}\" in this scope.", 3000);

        return _parent.ResolveEnvironment(name);
    }


}

