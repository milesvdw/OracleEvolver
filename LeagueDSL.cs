namespace League {

public enum StatementType {
    MathOp,
    BoolOP
}

public enum MathOpType {
    Add,
    Multiply,
    Subtract,
    Divide //do we even need this one? I suspect not but...
}

public class LeagueStatement {
    public StatementType LeagueStatementType;
}


public class MathOp : LeagueStatement {
}

public class Add : MathOp {
    public MathOp left;
    public MathOp right;
}

public class Subtract : MathOp {
    public MathOp left;
    public MathOp right;
}
public class Divide : MathOp {
    public MathOp left;
    public MathOp right;
}
public class Multiply : MathOp {
    public MathOp left;
    public MathOp right;
}

public class Const : MathOp { //not totally sure this is right
    public double value;
}

public class If : LeagueStatement {
    public Bool condition;
    public LeagueStatement left;
    public LeagueStatement right;
}

//this seems weird, is there a different way to classure booleans?
public class Bool {
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

public class AreEqual : Bool {
    public LeagueStatement right; //todo: these really shouldn't be just any kind of statement; rather they should be a value (not a boolean)
    public LeagueStatement left; 
}

}