public enum StatementType {
    MathOp
    BoolOP
}

public enum BoolOpType {
    And,
    Or,
    Not
}

public enum MathOpType {
    Add,
    Multiply,
    Subtract,
    Divide //do we even need this one? I suspect not but...
}

public struct LeagueStatement {
    public StatementType LeagueStatementType;
}

public struct Const : LeagueStatement {
    public double value;
}

public struct MathOp : LeagueStatement {
    public MathOpType type;
    public int left;
    public int right;
}

public struct If : LeagueStatement {
    public StatementType LeagueStatementType = StatementType.MathOp;
    public Bool condition;
    public LeagueStatement left;
    public LeagueStatement right;
}

//this seems weird, is there a different way to structure booleans?
public struct Bool {
}

public struct And : Bool {
    public Bool left;
    public Bool right;
}

public struct Or : Bool {
    public Bool left;
    public Bool right;
}

public class Not : Bool {
    public Bool inner;
}
