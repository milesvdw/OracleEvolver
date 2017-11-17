using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace League {

    public class LeagueReturn {
    }

    public class RunTimeException : LeagueReturn {
    }

    public class TypeCheckException : LeagueReturn {
    }

    public class ValidReturn : LeagueReturn {
        public double value;
        public ValidReturn(double retVal) {
            this.value = retVal;
        }
    }

    public static class LeagueInterpreter {
        public static JToken match;
        public static LeagueReturn interpret(LeagueStatement statement) {
            try {
                object ret = interpretDouble(statement);
                if(ret is double)
                    return new ValidReturn(interpretDouble(statement));
                else return new TypeCheckException();
            }
            catch (Exception) {
                return new RunTimeException();
            }

        }

        public static double interpretDouble(LeagueStatement statement) {
            if(statement is BinaryMathOp) return interpretBinaryMathOp(statement);
            if(statement is IntIf) return interpretNumIf(statement);
            if(statement is IntVal) return (statement as IntVal).value;
            if(statement is JsonDouble) return interpretJsonDouble(statement as JsonDouble);
            throw new ArgumentException();
        }

        private static double interpretNumIf(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(interpretBool(arguments[0])) return interpretDouble(arguments[1]);
            else return interpretDouble(arguments[2]);
        }
        private static double interpretBinaryMathOp(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(statement is Add)
                return interpretDouble(arguments[0]) + interpretDouble(arguments[1]);
            if(statement is Subtract)
                return interpretDouble(arguments[0]) - interpretDouble(arguments[1]);
            if(statement is Multiply)
                return interpretDouble(arguments[0]) * interpretDouble(arguments[1]);
            if(statement is Divide) {
                return interpretDouble(arguments[0]) / interpretDouble(arguments[1]);
            }

            return 0; // throw exception?
        }

        private static double interpretIf(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(interpretBool(arguments[0])) return interpretDouble(arguments[1]);
            else return interpretDouble(arguments[2]);
        }

        private static bool interpretBool(LeagueStatement condition) {
            LeagueStatement[] arguments = condition.getChildren();
            if(condition is BinaryBool) {
                return interpretBinaryBool(condition);
            }
            if(condition is Not) {
                return !interpretBool(arguments[0]);
            }
            if(condition is JsonBool) {
                return interpretJsonBool(condition as JsonBool);
            }
            if(condition is StringEQ) {
                return interpretStringVal(arguments[0] as StringVal).Equals(interpretStringVal(arguments[1] as StringVal));
            }
            if(condition is IntEQ) {
                return interpretDouble(arguments[0]) == interpretDouble(arguments[1]);
            }
            if(condition is GT) {
                return interpretDouble(arguments[0]) > interpretDouble(arguments[1]);
            }
            if(condition is LT) {
                return interpretDouble(arguments[0]) < interpretDouble(arguments[1]);
            }
            if(condition is True) return true;
            if(condition is False) return false;
            if(condition is BoolIf) {
                if(interpretBool(arguments[0])) return interpretBool(arguments[1]);
                else return interpretBool(arguments[2]);
            }
            throw new ArgumentException();
        }

        private static bool interpretBinaryBool(LeagueStatement statement) {
            LeagueStatement[] arguments = statement.getChildren();
            if(statement is And) {
                return interpretBool(arguments[0]) && interpretBool(arguments[1]);
            }
            if(statement is Or) {
                return interpretBool(arguments[0]) || interpretBool(arguments[1]);
            }
            throw new System.ArgumentException();
        }

        private static string interpretStringVal(StringVal statement) {
            if(statement is StringLit) return (statement as StringLit).value;
            if(statement is JsonString) return interpretJsonString(statement as JsonString);
            throw new ArgumentException();
        }

        private static string interpretJsonString(JsonString statement) {
            return (string)match.SelectToken(statement.accessor);
        }

        private static double interpretJsonDouble(JsonDouble statement) {
            return (double)match.SelectToken(statement.accessor);
        }

        private static bool interpretJsonBool(JsonBool statement) {
            return (bool)match.SelectToken(statement.accessor);
        }

        public static void RunTests() {
            testData = new LeagueStatement[2];
            testData[0] = new IntVal(5);
            testData[1] = new IntVal(10);
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

            double result = interpretDouble(add);
            if(result == 15) Console.WriteLine("Test Passed: TestAdd");
            else Console.WriteLine("Test Failed: TestAdd");
        }
        private static void TestSubtract() {
            Subtract sub  = new Subtract(testData);

            double result = interpretDouble(sub);
            if(result == -5) Console.WriteLine("Test Passed: TestSub");
            else Console.WriteLine("Test Failed: TestSub");
        }
        private static void TestMultiply() {
            Multiply mult = new Multiply(testData);

            double result = interpretDouble(mult);
            if(result == 50) Console.WriteLine("Test Passed: TestMult");
            else Console.WriteLine("Test Failed: TestMult");
        }
        private static void TestDivide() {
            Divide div = new Divide(testData);

            double result = interpretDouble(div);
            if(result == .5) Console.WriteLine("Test Passed: TestDivide");
            else Console.WriteLine("Test Failed: TestDivide");
        }
    }
}