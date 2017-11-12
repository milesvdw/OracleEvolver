using System;

namespace League {
    
    //requirements: all leaf nodes of the dsl implement the getValue() function
    //              all non-leaf nodes of the dsl impelemnt the getChildren() function
    public static class DSLPrinter {

        public static void print(LeagueStatement statement) {
            printStatement(statement);
            Console.WriteLine("");
        }

        public static void printStatement(LeagueStatement statement) {
                Console.Write("(");
            Console.Write(statement.GetType());
            if(statement is Leaf) {
                Console.Write((statement as Leaf).getValue());
            } else {
                foreach(LeagueStatement s in statement.getChildren()) printStatement(s);
            }
            Console.Write(")");
        }
    }
}