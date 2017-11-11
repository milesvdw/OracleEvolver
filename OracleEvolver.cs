using System;
using League;

namespace OracleEvolver
{
    public class OracleEvolver {
        private static Const seed = new Const(1);

        public static void Main() {
            evolve();
            LeagueInterpreter.RunTests();
        }

        public static void evolve() {
            seed.value = 5;
            Console.WriteLine("Beginning evolution!");
            Console.WriteLine("Seed Statement: ");
            Console.Write("\t");
            LeaguePrinter.print(new Add(new Multiply(new Const(4), new Const(2)), new Const(5)));
            //loop (forever?) and evolve oracles
        }
    }
}
