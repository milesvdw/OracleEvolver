using System;
using League;

public abstract class DSL {

}

public abstract class DSLInterpreter<StatementType> { //this does nothing right now, but I feel like there's a way to make this stuff generic
    public abstract int interpret(StatementType statement);
}