using System;

namespace League {
    public static class LeagueInterpreter {
        public static double interpret(LeagueStatement statement) {
            if(statement is MathOp) return interpretMath(statement as MathOp);
            if(statement is If) return interpretIf(statement as If);
            if(statement is Const) return (statement as Const).value;
            return 0;
        }

        private static double interpretMath(MathOp statement) {
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

        private static double interpretIf(If statement) {
            if(interpretBool(statement.condition)) return interpret(statement.left);
            else return interpret(statement.right);
        }

        private static bool interpretBool(Bool condition) {
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

        public static void RunTests() {
            RunMathTests();
        }

        public static void RunMathTests() {
            TestAdd();
            TestMultiply();
            TestSubtract();
            TestDivide();
        }

        //unit tests: expand these, or move to a framework perhaps?
        private static void TestAdd() {
            Add add = new Add(new Const(5), new Const(10));

            double result = interpret(add);
            if(result == 15) Console.WriteLine("Test Passed: TestAdd");
            else Console.WriteLine("Test Failed: TestAdd");
        }
        private static void TestSubtract() {
            Subtract sub  = new Subtract(new Const(5), new Const(10));

            double result = interpret(sub);
            if(result == -5) Console.WriteLine("Test Passed: TestSub");
            else Console.WriteLine("Test Failed: TestSub");
        }
        private static void TestMultiply() {
            Multiply mult = new Multiply(new Const(5), new Const(10));

            double result = interpret(mult);
            if(result == 50) Console.WriteLine("Test Passed: TestMult");
            else Console.WriteLine("Test Failed: TestMult");
        }
        private static void TestDivide() {
            Divide div = new Divide(new Const(5), new Const(10));

            double result = interpret(div);
            if(result == .5) Console.WriteLine("Test Passed: TestDivide");
            else Console.WriteLine("Test Failed: TestDivide");
        }
    }
}