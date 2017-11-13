using System;
using System.Linq;
using LeagueModels.MatchEndpoint;

namespace League {
    public static class LeagueInterpreter {
        public static Match match;
        public static double interpret(LeagueStatement statement) {
            if(statement is BinaryMathOp) return interpretBinaryMathOp(statement);
            if(statement is IntIf) return interpretNumIf(statement);
            if(statement is Int) return (statement as Int).value;
            return 0;
        }

        private static double interpretNumIf(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(interpretBool(arguments[0])) return interpret(arguments[1]);
            else return interpret(arguments[2]);
        }
        private static double interpretBinaryMathOp(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(statement is Add)
                    return interpret(arguments[0]) + interpret(arguments[1]);
            if(statement is Subtract)
                    return interpret(arguments[0]) - interpret(arguments[1]);
            if(statement is Multiply)
                    return interpret(arguments[0]) * interpret(arguments[1]);
            if(statement is Divide)
                    return interpret(arguments[0]) / interpret(arguments[1]);

            return 0; // throw exception?
        }

        private static double interpretIf(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(interpretBool(arguments[0])) return interpret(arguments[1]);
            else return interpret(arguments[2]);
        }

        private static bool interpretBool(LeagueStatement condition) {
            if(condition is BinaryBool) {
                return interpretBinaryBool(condition);
            }
            if(condition is Not) {
                return !interpretBool(condition);
            }
            if(condition is Win) {
                return interpretWin(condition as Win);
            }
            return false; //throw an error?
        }

        private static bool interpretBinaryBool(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(statement is And) {
                return interpretBool(arguments[0]) && interpretBool(arguments[1]);
            }
            if(statement is Or) {
                return interpretBool(arguments[0]) || interpretBool(arguments[1]);
            }
            if(statement is EQ) {
                return interpret(arguments[0]) == interpret(arguments[1]);
            }
            if(statement is GT) {
                return interpret(arguments[0]) < interpret(arguments[1]);
            }
            if(statement is LT) {
                return interpret(arguments[0]) > interpret(arguments[1]);
            }
            return false; //error state
        }

        private static bool interpretWin(Win win) {
            return match.Teams.FirstOrDefault(t => t.TeamId == win.team).Win == "Win";
        }

        public static void RunTests() {
            testData = new LeagueStatement[2];
            testData[0] = new Int(5);
            testData[1] = new Int(10);
            RunMathTests();
        }

        public static void RunMathTests() {
            TestAdd();
            TestMultiply();
            TestSubtract();
            TestDivide();
        }

        private static LeagueStatement[] testData;
        //unit tests: expand these, or move to a framework perhaps?
        private static void TestAdd() {
            Add add = new Add(testData);

            double result = interpret(add);
            if(result == 15) Console.WriteLine("Test Passed: TestAdd");
            else Console.WriteLine("Test Failed: TestAdd");
        }
        private static void TestSubtract() {
            Subtract sub  = new Subtract(testData);

            double result = interpret(sub);
            if(result == -5) Console.WriteLine("Test Passed: TestSub");
            else Console.WriteLine("Test Failed: TestSub");
        }
        private static void TestMultiply() {
            Multiply mult = new Multiply(testData);

            double result = interpret(mult);
            if(result == 50) Console.WriteLine("Test Passed: TestMult");
            else Console.WriteLine("Test Failed: TestMult");
        }
        private static void TestDivide() {
            Divide div = new Divide(testData);

            double result = interpret(div);
            if(result == .5) Console.WriteLine("Test Passed: TestDivide");
            else Console.WriteLine("Test Failed: TestDivide");
        }
    }
}