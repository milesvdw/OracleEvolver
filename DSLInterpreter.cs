using System;
using League;
public abstract class DSLInterpreter<StatementType> { //this does nothing right now, but I feel like there's a way to make this stuff generic
    public abstract int interpret(StatementType statement);
}

public class LeagueInterpreter {
    public double interpret(LeagueStatement statement) {
        if(statement is MathOp) return interpretMath(statement as League.MathOp);
        if(statement is If) return interpretIf(statement as League.If);
        if(statement is Const) return (statement as League.Const).value;
        return 0;
    }

    public double interpretMath(MathOp statement) {
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

    public double interpretIf(If statement) {
        if(interpretBool(statement.condition)) return interpret(statement.left);
        else return interpret(statement.right);
    }

    public bool interpretBool(Bool condition) {
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