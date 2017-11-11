using System;

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
        public Add(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
        public MathOp left;
        public MathOp right;
    }

    public class Subtract : MathOp {
        public Subtract(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
        public MathOp left;
        public MathOp right;
    }
    public class Divide : MathOp {
        public Divide(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
        public MathOp left;
        public MathOp right;
    }
    public class Multiply : MathOp {
        public Multiply(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
        public MathOp left;
        public MathOp right;
    }

    public class Const : MathOp { //not totally sure this is right
        public Const(double value)
        {
            this.value = value;
        }
        public double value;
    }

    public class If : LeagueStatement {
        public If(Bool condition, LeagueStatement left, LeagueStatement right)
        {
            this.condition = condition;
            this.left = left;
            this.right = right;
        }
        public Bool condition;
        public LeagueStatement left;
        public LeagueStatement right;
    }

    //this seems weird, is there a different way to classure booleans?
    public class Bool {
    }

    public class And : Bool {
        public And(Bool left, Bool right)
        {
            this.left = left;
            this.right = right;
        }
        public Bool left;
        public Bool right;
    }

    public class Or : Bool {
        public Or(Bool left, Bool right)
        {
            this.left = left;
            this.right = right;
        }
        public Bool left;
        public Bool right;
    }

    public class Not : Bool {
        public Not(Bool inner)
        {
            this.inner = inner;
        }
        public Bool inner;
    }

    public class AreEqual : Bool {
        public AreEqual(LeagueStatement left, LeagueStatement right)
        {
            this.left = left;
            this.right = right;
        }
        public LeagueStatement right; //todo: these really shouldn't be just any kind of statement; rather they should be a value (not a boolean)
        public LeagueStatement left; 
    }
}