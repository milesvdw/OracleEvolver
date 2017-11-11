public class DSLInterpreter<ResultType, Statement> {
    public virtual ResultType interpret(Statement statement);
}


public class LeagueInterpreter : DSLInterpreter<int, LeagueStatement> {
    public int interpret(LeagueStatement statement) {
        if(statement is MathOp) return interpretMath(statement);
        if(statement is If) return interpretIf(statement);
    }

    public double interpretMath(MathOp statement) {
        switch(statement.type) {
            case MathOpType.Add:
                return statement.left + statement.right;
                break;
            case MathOpType.Subtract:
                return statement.left - statement.right;
                break;
            case MathOpType.Multiply:
                return statement.left * statement.right;
                break;
            case MathOpType.Divide:
                return statement.left / statement.right;
                break;
        }
    }

    public int interpretIf(statement) {
        return 0;
    }

    //unit tests: expand these, or move to a framework perhaps?
    private TestAdd() {
        LeagueStatement add = new MathOp();
        add.left = 5;
        add.right = 10;

        int result = interpret(add);
        if(result == 15) Console.WriteLine("Test Passed: TestAdd");
        else Console.WriteLine("Test Failed: TestAdd");
    }


}