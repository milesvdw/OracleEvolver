using System;
using System.Linq;

namespace League {

    #region types
    public interface Number {
    }
    public interface Bool {
    }
    public interface StringType {
    }

    public interface Leaf {
        string getValue();
    }
    #endregion

    public abstract class LeagueStatement : ICloneable, IEquatable<LeagueStatement> {
        public static string[] literalList = {"GOLD", "SILVER", "BRONZE", "UNRANKED"};
        public static string[] stringAccessorList = {"participants[0].highestAchievedSeasonTier",
                                                     "participants[1].highestAchievedSeasonTier",
                                                     "participants[2].highestAchievedSeasonTier",
                                                     "participants[3].highestAchievedSeasonTier",
                                                     "participants[4].highestAchievedSeasonTier",
                                                     "participants[5].highestAchievedSeasonTier",
                                                     "participants[6].highestAchievedSeasonTier",
                                                     "participants[7].highestAchievedSeasonTier",
                                                     "participants[8].highestAchievedSeasonTier",
                                                     "participants[9].highestAchievedSeasonTier"};
        public static string[] doubleAccessorList = {"team[0].teamId",
                                                     "team[1].teamId",
                                                     "teams[0].bans[0].pickTurn",
                                                     "teams[0].bans[0].championId",
                                                     "teams[0].bans[1].pickTurn",
                                                     "teams[0].bans[1].championId",
                                                     "teams[0].bans[2].pickTurn",
                                                     "teams[0].bans[2].championId",
                                                     "teams[0].bans[3].pickTurn",
                                                     "teams[0].bans[3].championId",
                                                     "teams[0].bans[4].pickTurn",
                                                     "teams[0].bans[4].championId",
                                                     "teams[1].bans[0].pickTurn",
                                                     "teams[1].bans[0].championId",
                                                     "teams[1].bans[1].pickTurn",
                                                     "teams[1].bans[1].championId",
                                                     "teams[1].bans[2].pickTurn",
                                                     "teams[1].bans[2].championId",
                                                     "teams[1].bans[3].pickTurn",
                                                     "teams[1].bans[3].championId",
                                                     "teams[1].bans[4].pickTurn",
                                                     "teams[1].bans[4].championId",
                                                     "participants[0].teamId",
                                                     "participants[1].teamId",
                                                     "participants[2].teamId",
                                                     "participants[3].teamId",
                                                     "participants[4].teamId",
                                                     "participants[5].teamId",
                                                     "participants[6].teamId",
                                                     "participants[7].teamId",
                                                     "participants[8].teamId",
                                                     "participants[9].teamId",
                                                     "participants[0].spell1Id",
                                                     "participants[1].spell1Id",
                                                     "participants[2].spell1Id",
                                                     "participants[3].spell1Id",
                                                     "participants[4].spell1Id",
                                                     "participants[5].spell1Id",
                                                     "participants[6].spell1Id",
                                                     "participants[7].spell1Id",
                                                     "participants[8].spell1Id",
                                                     "participants[9].spell1Id",
                                                     "participants[0].spell2Id",
                                                     "participants[1].spell2Id",
                                                     "participants[2].spell2Id",
                                                     "participants[3].spell2Id",
                                                     "participants[4].spell2Id",
                                                     "participants[5].spell2Id",
                                                     "participants[6].spell2Id",
                                                     "participants[7].spell2Id",
                                                     "participants[8].spell2Id",
                                                     "participants[9].spell2Id",
                                                     "participants[0].championId",
                                                     "participants[1].championId",
                                                     "participants[2].championId",
                                                     "participants[3].championId",
                                                     "participants[4].championId",
                                                     "participants[5].championId",
                                                     "participants[6].championId",
                                                     "participants[7].championId",
                                                     "participants[8].championId",
                                                     "participants[9].championId"};
        public static string[] boolAccessorList = {};

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
        public static int MIN_COEFFICIENT = -1;
        public static int MAX_COEFFICIENT = 500;
        public static double DELETION_CHANCE = 0;
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
                if(shouldDelete()) return new IntVal(1);
                return randomNumberType();
            }
            if(this is Bool) {
                if(shouldDelete()) return new True();
                return randomBoolType();
            }
            if(this is StringType) {
                if(shouldDelete()) return (LeagueStatement) this.Clone(); //can't delete a string, doesn't make sense. only delete the parent node.
                return randomStringType();
            }
            return null;
            
        }

        public LeagueStatement randomStringType() {
            if(stringAccessorList.Length > 0) {
                int roll = rand.Next(1, 3);
                switch(roll) {  //this is, for now, unfortunately, a maintained list of every possible LeagueStatement
                    case 1:
                        return new JsonString(randomAccessor(stringAccessorList));
                    case 2:
                        return new StringLit(randomStringLit());
                }
            }
            else {
                return this.randomNumberType(); //if no string types exist in the language, replace all strings with ints
            }
            throw new ArgumentException();
        }

        protected static string randomStringLit() {
            int roll = rand.Next(0, literalList.Length);
            return literalList[roll];
        }

        private static string randomAccessor(string[] list) {
            int roll = rand.Next(0, list.Length);
            return list[roll];
        }

        private LeagueStatement randomBoolType() {
            int roll = rand.Next(1, 9);
            switch(roll) {  //this is, for now, unfortunately, a maintained list of every possible LeagueStatement
                case 1:
                    return new And(children);
                case 2:
                    return new Or(children);
                case 3:
                    return new BoolIf(children);
                case 4:
                    return new StringEQ(children);
                case 5:
                    return new IntEQ(children);
                case 6:
                    return new GT(children);
                case 7:
                    return new LT(children);
                case 8:
                    return new Not(children);
                case 9:
                    //return new JsonBool(randomAccessor(boolAccessorList));
                default:
                    break;
            }
            return null;
        }

        private LeagueStatement randomNumberType() {
            int roll = rand.Next(1, 14);
            //note that this makes every kind of mutation equally likely...
            //whereas, ideally, mutations from one int to another should probably be the most likely
            //more investigation required.
            switch(roll) {  //this is, for now, unfortunately, a maintained list of every possible LeagueStatement
                case 1:
                    return new Add(children);
                case 2:
                    return new Multiply(children);
                case 3:
                    //return new Subtract(children);
                case 4:
                    //return new Divide(children);
                case 5:
                    return new IntVal(rand.Next(MIN_COEFFICIENT, MAX_COEFFICIENT+1));
                case 6:
                    return new IntIf(children);
                case 7:
                    return new JsonDouble(randomAccessor(doubleAccessorList));
                case 8:
                    return new IntIf(children);
                case 9:
                    return new IntIf(children);
                case 10:
                    return new IntIf(children);
                case 11:
                    return new IntIf(children);
                case 12:
                    return new IntIf(children);
                case 13:
                    return new IntIf(children);
                default:
                    break;
            }
            return null;
        }

        protected void init(LeagueStatement[] statements, int degree) {
            this.children = new LeagueStatement[degree];
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
            if(!(statements[0] is Number)) statements[0] = new IntVal(1);
            if(!(statements[1] is Number)) statements[1] = new IntVal(1);
            init(statements, 2);
        }
    }
    public class Add : BinaryMathOp, Number {
        public override object Clone() {
            return new Add(this.children);
        }
        public Add(LeagueStatement[] statements) : base(statements) {}
    }

    public class Subtract : BinaryMathOp, Number {
        public override object Clone() {
            return new Subtract(this.children);
        }
        public Subtract(LeagueStatement[] statements) : base(statements) {}
    }
    public class Divide : BinaryMathOp, Number {
        public override object Clone() {
            return new Divide(this.children);
        }
        public Divide(LeagueStatement[] statements) : base(statements) {}
    }
    public class Multiply : BinaryMathOp, Number {
        public override object Clone() {
            return new Multiply(this.children);
        }
        public Multiply(LeagueStatement[] statements) : base(statements) {}
    }

    public class IntVal : LeagueStatement, Number, Leaf {
        public new bool Equals(LeagueStatement other) {
            if(other is IntVal) return (other as IntVal).value == this.value;
            else return false;
        }
        public override object Clone() {
            return new IntVal(this.value);
        }
        public double value;
        public string getValue() {
            return this.value.ToString();
        }
        public IntVal(double value)  : base()
        {
            this.value = value;
        }
    }

    public class JsonDouble : LeagueStatement, Number, Leaf {
        public string accessor;
        public override object Clone() {
            return new JsonDouble(this.accessor);
        }

        public string getValue() {
            return this.accessor;
        }

        public JsonDouble(string accessor) {
            this.accessor = accessor;
        }
    }

    public class IntIf : LeagueStatement, Number {
        public override object Clone() {
            return new IntIf(this.children);
        }
        public IntIf(LeagueStatement[] old) : base()
        {
            LeagueStatement[] statements = copyFromOld(old, 3);
            if(!(statements[0] is Bool)) statements[0] = new True(); // default to true if not a boolean
            if(!(statements[1] is Number)) statements[1] = new IntVal(1);
            if(!(statements[2] is Number)) statements[2] = new IntVal(1);
            init(statements, 3);
        }
    }

    public class BoolIf : LeagueStatement, Bool {
        public override object Clone() {
            return new BoolIf(this.children);
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
    public class True : LeagueStatement, Bool, Leaf {
        
        public new bool Equals(LeagueStatement other) {
            return other is True;
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
        public new bool Equals(LeagueStatement other) {
            return other is False;
        }
        public override object Clone() {
            return new False();
        }
        public string getValue() {
            return "False";
        }
        public False() : base() {}
    }

    public class JsonBool : LeagueStatement, Bool, Leaf {
        public string accessor;
        public override object Clone() {
            return new JsonBool(this.accessor);
        }

        public string getValue() {
            return this.accessor;
        }

        public JsonBool(string accessor) {
            this.accessor = accessor;
        }
    }

    public class BinaryBool : LeagueStatement, Bool {
        public override object Clone() {
            return new BinaryBool(this.children);
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
            return new And(this.children);
        }
        public And(LeagueStatement[] statements) : base(statements) {}
    }

    public class Or : BinaryBool, Bool {
        public override object Clone() {
            return new Or(this.children);
        }
        public Or(LeagueStatement[] statements) : base(statements) {}
    }

    public class Not : LeagueStatement, Bool {
        public override object Clone() {
            return new Not(this.children);
        }
        public Not(LeagueStatement[] old)
        {
            LeagueStatement[] statements = copyFromOld(old, 1);
            
            if(!(statements[0] is Bool)) statements[0] = new True();
            init(statements, 1);
        }
    }

    public class StringEQ : LeagueStatement, Bool {
        public override object Clone() {
            return new StringEQ(this.children);
        }
        public StringEQ(LeagueStatement[] old) : base() {
            LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is StringType)) statements[0] = new StringLit(randomStringLit());
            if(!(statements[1] is StringType)) statements[1] = new StringLit(randomStringLit());
            init(statements, 2);
        }
    }
    public class IntEQ : LeagueStatement, Bool {
        public override object Clone() {
            return new IntEQ(this.children);
        }
        public IntEQ(LeagueStatement[] old) : base() {
;           LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is Number)) statements[0] = new IntVal(1);
            if(!(statements[1] is Number)) statements[1] = new IntVal(1);
            init(statements, 2);
        }
    }

    public class GT : LeagueStatement, Bool {
        public override object Clone() {
            return new GT(this.children);
        }
        public GT(LeagueStatement[] old) : base() {
;           LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is Number)) statements[0] = new IntVal(1);
            if(!(statements[1] is Number)) statements[1] = new IntVal(1);
            init(statements, 2);
        }
    }
    public class LT : LeagueStatement, Bool {
        public override object Clone() {
            return new LT(this.children);
        }
        public LT(LeagueStatement[] old) : base() {
;           LeagueStatement[] statements = copyFromOld(old, 2);
            if(!(statements[0] is Number)) statements[0] = new IntVal(1);
            if(!(statements[1] is Number)) statements[1] = new IntVal(1);
            init(statements, 2);
        }
    }

    public abstract class StringVal : LeagueStatement, StringType {
    }

    public class StringLit : StringVal, StringType, Leaf {
        public string value;
        public override object Clone() {
            return new StringLit(this.value);
        }

        public string getValue() {
            return this.value;
        }

        public StringLit(string litval) {
            this.value = litval;
        }
    }

    public class JsonString : StringVal, StringType, Leaf {
        public string accessor;
        public override object Clone() {
            return new JsonString(this.accessor);
        }

        public string getValue() {
            return this.accessor;
        }

        public JsonString(string accessor) {
            this.accessor = accessor;
        }
    }
}