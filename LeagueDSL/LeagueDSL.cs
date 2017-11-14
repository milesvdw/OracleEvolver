using System;
using System.Linq;

namespace League {

    #region types
    public interface Number {
    }
    public interface Bool {
    }
    public interface Leaf {
        bool Equals(Leaf other);
        string getValue();
    }
    #endregion

    public abstract class LeagueStatement : ICloneable, IEquatable<LeagueStatement> {

        public void setChildren(LeagueStatement[] newChildren) {
            this.children = newChildren;
        }
        public static LeagueStatement[] copyFromOld(LeagueStatement[] old, int quantity) {
            LeagueStatement[] statements = new LeagueStatement[quantity];
            for(int i = 0; i < statements.Length; i++) statements[i] = (old.Length > i) ? old[i] : new Default();
            return statements;
        }

        public bool Equals(LeagueStatement other) {
            return this.GetType() == other.GetType() && this.children.Zip(other.children, (me, it) => me.Equals(it)).All(x => x);
        }

        protected static Random rand = new Random();
        public static int MIN_COEFFICIENT = -5;
        public static int MAX_COEFFICIENT = 5;
        public static double DELETION_CHANCE = .05;
        protected LeagueStatement[] children; // this is the array of child nodes
        public LeagueStatement[] getChildren() {
            return children;
        }

        public abstract object Clone();

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
            int roll = rand.Next(1, 8);
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
                case 7:
                    return new Win(rand.Next(1, 2)*100);
                default:
                    break;
            }
            return null;
        }

        private LeagueStatement randomNumberType() {
            int roll = rand.Next(1, 7);
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
                    return new Int(rand.Next(MIN_COEFFICIENT, MAX_COEFFICIENT+1));
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

    public class Default : LeagueStatement { //it makes me cry inside that this has to exist...
        public override object Clone() {
            return new Default();
        }
    }

    public abstract class BinaryMathOp : LeagueStatement {
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
        public override object Clone() {
            return new Add(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Add(LeagueStatement[] statements) : base(statements) {}
    }

    public class Subtract : BinaryMathOp, Number {
        public override object Clone() {
            return new Subtract(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Subtract(LeagueStatement[] statements) : base(statements) {}
    }
    public class Divide : BinaryMathOp, Number {
        public override object Clone() {
            return new Divide(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Divide(LeagueStatement[] statements) : base(statements) {}
    }
    public class Multiply : BinaryMathOp, Number {
        public override object Clone() {
            return new Multiply(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Multiply(LeagueStatement[] statements) : base(statements) {}
    }

    public class Int : LeagueStatement, Number, Leaf {
        public bool Equals(Leaf other) {
            Console.Write("OMG OVERRIDE WORKING"); //todo lol
            return other.getValue().Equals(this.value);
        }
        public override object Clone() {
            return new Int(this.value);
        }
        public double value;
        public string getValue() {
            return this.value.ToString();
        }
        public Int(double value)  : base()
        {
            this.value = value;
        }
    }

    public class IntIf : LeagueStatement, Number {
        public override object Clone() {
            return new IntIf(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
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
        public override object Clone() {
            return new BoolIf(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public BoolIf(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 3);
            if(!(statements[0] is Bool)) statements[0] = new True(); // default to true if not a boolean
            if(!(statements[1] is Bool)) statements[1] = new True();
            if(!(statements[2] is Bool)) statements[2] = new True();
            init(statements, 3);
        }
    }

    public class Win : LeagueStatement, Bool, Leaf {
        public bool Equals(Leaf other) {
            Console.Write("OMG OVERRIDE WORKING"); //todo lol
            return other.getValue().Equals(this.team);
        }
        public override object Clone() {
            return new Win(this.team);
        }
        public int team; //either 100 or 200
        public string getValue() {
            return "Win" + team.ToString();
        }
        public Win(int team) : base() {
            this.team = team;
        }
    }
    public class True : LeagueStatement, Bool, Leaf {
        
        public bool Equals(Leaf other) {
            return base.Equals(other);
        }
        public override object Clone() {
            return new True();
        }
        public string getValue() {
            return "True";
        }
        public True() : base() {}
    }
    public class False : LeagueStatement, Bool, Leaf {
        public bool Equals(Leaf other) {
            return base.Equals(other);
        }
        public override object Clone() {
            return new False();
        }
        public string getValue() {
            return "False";
        }
        public False() : base() {}
    }

    public class BinaryBool : LeagueStatement, Bool {
        public override object Clone() {
            return new False();
        }
        public BinaryBool(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is Bool)) statements[0] = new True();
            if(!(statements[1] is Bool)) statements[1] = new True();
            init(statements, 2);
        }
    }
    public class And : BinaryBool, Bool {
        public override object Clone() {
            return new And(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public And(LeagueStatement[] statements) : base(statements) {}
    }

    public class Or : BinaryBool, Bool {
        public override object Clone() {
            return new Or(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Or(LeagueStatement[] statements) : base(statements) {}
    }

    public class Not : LeagueStatement, Bool {
        public override object Clone() {
            return new Not(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public Not(LeagueStatement[] old)
        {
            LeagueStatement[] statements = copyFromOld(old, 1);
            
            if(!(statements[0] is Bool)) statements[0] = new True();
            init(statements, 1);
        }
    }

    public class EQ : BinaryBool, Bool {
        public override object Clone() {
            return new EQ(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public EQ(LeagueStatement[] statements) : base(statements) {}
    }
    public class GT : BinaryBool, Bool {
        public override object Clone() {
            return new GT(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public GT(LeagueStatement[] statements) : base(statements) {}
    }
    public class LT : BinaryBool, Bool {
        public override object Clone() {
            return new LT(this.children.Select(c => (LeagueStatement) c.Clone()).ToArray());
        }
        public LT(LeagueStatement[] statements) : base(statements) {}
    }
}