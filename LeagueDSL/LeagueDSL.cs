using System;

namespace League {
    public enum StatementType {
        MathOp,
        BoolOP
    }
    public class LeagueStatement {
    }


    public class MathOp : LeagueStatement {
    }

    public class BinaryMathOp : MathOp {
        public MathOp left;
        public MathOp right;
    }
    public class UnaryMathOp : MathOp {
        public MathOp inner;
    }

    public class Add : BinaryMathOp {
        public Add(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
    }

    public class Subtract : BinaryMathOp {
        public Subtract(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
    }
    public class Divide : BinaryMathOp {
        public Divide(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
    }
    public class Multiply : BinaryMathOp {
        public Multiply(MathOp left, MathOp right)
        {
            this.left = left;
            this.right = right;
        }
    }

    public class Const : UnaryMathOp { //not totally sure this is right
        public Const(double value)
        {
            this.inner = inner;
        }
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