public enum StatementType {
    MathOp
}

public class DSLInterpreter<ResultType, Statement> {
    public virtual ResultType interpret(Statement statement);
}

public class LeagueStatement {

}


public class MathOp : LeagueStatement {
    public StatementType LeagueStatementType = StatementType.MathOp
    public MathOpType type;
    public int left;
    public int right;

    public MathOp() {};


}

public class LeagueInterpreter : DSLInterpreter<int, LeagueStatement> {
    public int interpret(LeagueStatement statement) {
        if(statement is basic math) {
            //return result
        }
        if(statement is conditional) {
            //evaluate conditional, then return appropriate path
        }
    }
}