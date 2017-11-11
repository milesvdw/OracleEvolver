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

public static class LeaguePrinter {
    public static void print(LeagueStatement statement) {
        Console.Write("<");
        if(statement is MathOp) {
            printMath(statement as MathOp);
        }
        if(statement is If) printIf(statement as If);
        Console.Write(">");
    }

    private static void printMath(MathOp statement) {
        if(statement is Add)
                printBinaryOp("Add", (statement as Add).left.ToString(), (statement as Add).right.ToString());
        if(statement is Subtract)
                printBinaryOp("Subtract", (statement as Subtract).left.ToString(), (statement as Subtract).right.ToString());
        if(statement is Multiply)
                printBinaryOp("Multiply", (statement as Multiply).left.ToString(), (statement as Multiply).right.ToString());
        if(statement is Divide)
                printBinaryOp("Divide", (statement as Divide).left.ToString(), (statement as Divide).right.ToString());
        if(statement is Const)
            Console.Write((statement as Const).value.ToString());
    }

    private static void printBinaryOp(string op, string left, string right) {
        Console.Write("Add <");
        Console.Write(left);
        Console.Write(">, <");
        Console.Write(right);
        Console.Write(">");
    }

    private static void printIf(If statement) {
        Console.Write("If");
        print(statement.left);
        print(statement.right);
    }

}

public class LeagueInterpreter {
    public double interpret(LeagueStatement statement) {
        if(statement is MathOp) return interpretMath(statement as MathOp);
        if(statement is If) return interpretIf(statement as If);
        if(statement is Const) return (statement as Const).value;
        return 0;
    }

    private double interpretMath(MathOp statement) {
        if(statement is Add)
                return interpret((statement as Add).left) + interpret((statement as Add).right);
        if(statement is Subtract)
                return interpret((statement as Subtract).left) - interpret((statement as Subtract).right);
        if(statement is Multiply)
                return interpret((statement as Multiply).left) * interpret((statement as Multiply).right);
        if(statement is Divide)
                return interpret((statement as Divide).left) / interpret((statement as Divide).right);
        if(statement is Const)
                return (statement as Const).value;

        return 0; // throw exception?
    }

    private double interpretIf(If statement) {
        if(interpretBool(statement.condition)) return interpret(statement.left);
        else return interpret(statement.right);
    }

    private bool interpretBool(Bool condition) {
        if(condition is Not) {
            return !interpretBool((condition as Not).inner);
        }
        if(condition is And) {
            return interpretBool((condition as And).left) && interpretBool((condition as And).right);
        }
        if(condition is Or) {
            return interpretBool((condition as Or).left) || interpretBool((condition as Or).right);
        }
        if(condition is AreEqual) {
            return interpret((condition as AreEqual).left) == interpret((condition as AreEqual).right);
        }
        return false; //throw an error?
    }

    //unit tests: expand these, or move to a framework perhaps?
    private void TestAdd() {
        Add add = new Add();
        Const five = new Const();
        five.value = 5;
        Const ten = new Const();
        ten.value = 10;
        add.left = five;
        add.right = ten;

        double result = interpret(add);
        if(result == 15) Console.WriteLine("Test Passed: TestAdd");
        else Console.WriteLine("Test Failed: TestAdd");
    }
    private void TestSubtract() {
        Subtract sub  = new Subtract();
        Const five = new Const();
        five.value = 5;
        Const ten = new Const();
        ten.value = 10;
        sub.left = five;
        sub.right = ten;

        double result = interpret(sub);
        if(result == -5) Console.WriteLine("Test Passed: TestSub");
        else Console.WriteLine("Test Failed: TestSub");
    }
    private void TestMultiply() {
        Multiply mult = new Multiply();
        Const five = new Const();
        five.value = 5;
        Const ten = new Const();
        ten.value = 10;
        mult.left = five;
        mult.right = ten;

        double result = interpret(mult);
        if(result == 50) Console.WriteLine("Test Passed: TestMult");
        else Console.WriteLine("Test Failed: TestMult");
    }
    private void TestDivide() {
        Divide div = new Divide();
        Const five = new Const();
        five.value = 5;
        Const ten = new Const();
        ten.value = 10;
        div.left = five;
        div.right = ten;

        double result = interpret(div);
        if(result == .5) Console.WriteLine("Test Passed: TestDivide");
        else Console.WriteLine("Test Failed: TestDivide");
    }
}
}