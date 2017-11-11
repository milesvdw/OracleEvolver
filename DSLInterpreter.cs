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

public class DSLInterpreter<ResultType, Statement> {
    public virtual ResultType interpret(Statement statement);
}

public class LeagueStatement {

}


public class MathOp : LeagueStatement {
    public StatementType LeagueStatementType = StatementType.MathOp;
    public MathOpType type;
    public int left;
    public int right;

    public MathOp() {};
}

public class If : LeagueStatement {
    public StatementType LeagueStatementType = StatementType.MathOp;
    public BoolOpType type;
    public boolean condition;
    public 
}

public class Bool {
    public BoolOpType type;
}

public class And : Bool {
    public Bool left;
    public Bool right;
}

public class Or : Bool {
    public Bool left;
    public Bool right;
}

public class Not : Bool {
    public Bool inner;
}

public class LeagueInterpreter : DSLInterpreter<int, LeagueStatement> {
    public int interpret(LeagueStatement statement) {
        if(statement is basic math) {
            //return result
        }
        if(statement is Bool) {
            //evaluate conditional, then return appropriate path
        }
    }
}