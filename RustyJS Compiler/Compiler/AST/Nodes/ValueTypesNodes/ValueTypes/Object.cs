internal class Object : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.Object;
    public Dictionary<string, RuntimeValueTypeNode> Props { get; }
    
    public Object(Dictionary<string, RuntimeValueTypeNode> props) {
        Props = props;
    }
}

