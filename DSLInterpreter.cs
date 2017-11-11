public class DSLInterpreter<ResultType, Statement> {
    public virtual ResultType interpret(Statement statement);
}


public class LeagueInterpreter : DSLInterpreter<int, LeagueStatement> {
    public int interpret(LeagueStatement statement) {
        if(statement is basic math) {
            //return result
        }
        if(statement istype Bool) {
            //evaluate conditional, then return appropriate path
        }
    }
}