using System;

namespace League {
    
    public static class LeaguePrinter {

        public static void print(LeagueStatement statement) {
            printStatement(statement);
            Console.WriteLine("");
        }

        public static void printStatement(LeagueStatement statement) {
            Console.Write("<");
            if(statement is MathOp) {
                printMath(statement as MathOp);
            }
            if(statement is If) printIf(statement as If);
            Console.Write(">");
        }

        private static void printMath(MathOp statement) {
            if(statement is Add)
                    printBinaryOp("Add", (statement as Add).left, (statement as Add).right);
            if(statement is Subtract)
                    printBinaryOp("Subtract", (statement as Subtract).left, (statement as Subtract).right);
            if(statement is Multiply)
                    printBinaryOp("Multiply", (statement as Multiply).left, (statement as Multiply).right);
            if(statement is Divide)
                    printBinaryOp("Divide", (statement as Divide).left, (statement as Divide).right);
            if(statement is Const)
                Console.Write((statement as Const).value.ToString());
        }

        private static void printBinaryOp(string op, LeagueStatement left, LeagueStatement right) {
            Console.Write(op + " ");
            printStatement(left);
            Console.Write(" ");
            printStatement(right);
        }

        private static void printIf(If statement) {
            Console.Write("If");
            printStatement(statement.left);
            printStatement(statement.right);
        }

    }
}