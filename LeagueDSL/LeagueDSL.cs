using System;

namespace League {

    #region types
    public interface Number {
    }
    public interface Bool {
    }
    public interface Leaf {
        string getValue();
    }
    #endregion

    public class LeagueStatement {
        public static LeagueStatement[] copyFromOld(LeagueStatement[] old, int quantity) {
            LeagueStatement[] statements = new LeagueStatement[quantity];
            for(int i = 0; i < statements.Length; i++) statements[i] = (old.Length > i) ? old[i] : new LeagueStatement();
            return statements;
        }
        protected static Random rand = new Random();
        public static int MIN_COEFFICIENT = -5;
        public static int MAX_COEFFICIENT = 5;
        public static double DELETION_CHANCE = .2;
        private LeagueStatement[] children; // this is the array of child nodes
        public LeagueStatement[] getChildren() {
            return children;
        }
        public LeagueStatement() {
            children = new LeagueStatement[0];
        }

        private bool shouldDelete() {
            return ((double)rand.Next(0, 100)) / 100 < DELETION_CHANCE;
        }
        public LeagueStatement mutate() {

            if(this is Number) {
                if(shouldDelete()) return new Int(1);
                return randomNumberType();
            }
            if(this is Bool) {
                if(shouldDelete()) return new True();
                return randomBoolType();
            }
            return null;
            
        }

        private LeagueStatement randomBoolType() {
            int roll = rand.Next(1, 5);
            switch(roll) {  //this is, for now, unfortunately, a maintained list of every possible LeagueStatement
                case 1:
                    return new And(children);
                case 2:
                    return new Or(children);
                case 3:
                    return new BoolIf(children);
                case 4:
                    return new EQ(children);
                case 5:
                    return new GT(children);
                case 6:
                    return new LT(children);
                default:
                    break;
            }
            return null;
        }

        private LeagueStatement randomNumberType() {
            int roll = rand.Next(1, 5);
            //note that this makes every kind of mutation equally likely...
            //whereas, ideally, mutations from one int to another should probably be the most likely
            //more investigation required.
            switch(roll) {  //this is, for now, unfortunately, a maintained list of every possible LeagueStatement
                case 1:
                    return new Add(children);
                case 2:
                    return new Multiply(children);
                case 3:
                    return new Subtract(children);
                case 4:
                    return new Divide(children);
                case 5:
                    return new Int(rand.Next(MIN_COEFFICIENT, MAX_COEFFICIENT));
                case 6:
                    return new IntIf(children);
                default:
                    break;
            }
            return null;
        }

        protected void init(LeagueStatement[] statements, int degree) {
            children = new LeagueStatement[degree];
            Array.Copy(statements, children, degree);
        }
    }

    public class BinaryMathOp : LeagueStatement {
        public BinaryMathOp(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 2);
            // if there is a type mismatch, then simply replace the offending type with unit
            if(!(statements[0] is Number)) statements[0] = new Int(1);
            if(!(statements[1] is Number)) statements[1] = new Int(1);
            init(statements, 2);
        }
    }
    public class Add : BinaryMathOp, Number {
        public Add(LeagueStatement[] statements) : base(statements) {}
    }

    public class Subtract : BinaryMathOp, Number {
        public Subtract(LeagueStatement[] statements) : base(statements) {}
    }
    public class Divide : BinaryMathOp, Number {
        public Divide(LeagueStatement[] statements) : base(statements) {}
    }
    public class Multiply : BinaryMathOp, Number {
        public Multiply(LeagueStatement[] statements) : base(statements) {}
    }

    public class Int : LeagueStatement, Number, Leaf {
        public string getValue() {
            return this.value.ToString();
        }
        public Int(double value)  : base()
        {
            this.value = value;
        }
        public double value;
    }

    public class IntIf : LeagueStatement, Number {
        public IntIf(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 3);
            if(!(statements[0] is Bool)) statements[0] = new True(); // default to true if not a boolean
            if(!(statements[1] is Number)) statements[1] = new Int(1);
            if(!(statements[2] is Number)) statements[2] = new Int(1);
            init(statements, 3);
        }
    }

    public class BoolIf : LeagueStatement, Bool {
        public BoolIf(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 3);
            if(!(statements[0] is Bool)) statements[0] = new True(); // default to true if not a boolean
            if(!(statements[1] is Bool)) statements[1] = new True();
            if(!(statements[2] is Bool)) statements[2] = new True();
            init(statements, 3);
        }
    }

    public class True : LeagueStatement, Bool, Leaf {
        public string getValue() {
            return "True";
        }
        public True() : base() {}
    }
    public class False : LeagueStatement, Bool, Leaf {
        public string getValue() {
            return "False";
        }
        public False() : base() {}
    }

    public class BinaryBool : LeagueStatement, Bool {
        public BinaryBool(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is Bool)) statements[0] = new True();
            if(!(statements[1] is Bool)) statements[1] = new True();
            init(statements, 2);
        }
    }
    public class And : BinaryBool, Bool {
        public And(LeagueStatement[] statements) : base(statements) {}
    }

    public class Or : BinaryBool, Bool {
        public Or(LeagueStatement[] statements) : base(statements) {}
    }

    public class Not : LeagueStatement, Bool {
        public Not(LeagueStatement[] old)
        {
            LeagueStatement[] statements = copyFromOld(old, 1);
            
            if(!(statements[0] is Bool)) statements[0] = new True();
            init(statements, 1);
        }
    }

    public class EQ : BinaryBool, Bool {
        public EQ(LeagueStatement[] statements) : base(statements) {}
    }
    public class GT : BinaryBool, Bool {
        public GT(LeagueStatement[] statements) : base(statements) {}
    }
    public class LT : BinaryBool, Bool {
        public LT(LeagueStatement[] statements) : base(statements) {}
    }
}