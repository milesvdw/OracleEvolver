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
                return interpret(statement.left) + interpret(statement.right);
                break;
            case MathOpType.Subtract:
                return interpret(statement.left) - interpret(statement.right);
                break;
            case MathOpType.Multiply:
                return interpret(statement.lef) * interpret(statement.right);
                break;
            case MathOpType.Divide:
                return interpret(statement.left) / interpret(statement.right);
                break;
        }
    }

    public int interpretIf(If statement) {
        if(interpretBool(statement.condition)) return interpret(statement.left);
        else return interpret(statement.right);
    }

    public bool interpretBool(BoolOp condition) {
        if(condition is Not) {
            return !interpret(condition.inner);
        }
        if(condition is And) {
            return interpret(condition.left) && interpret(condition.right);
        }
        if(condition is Or) {
            return interpret(condition.left) || interpret(condition.right);
        }
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