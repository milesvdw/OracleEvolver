using System;
using League;

namespace OracleEvolver
{
    public class OracleEvolver {
        private static Const seed = new Const();

        public static void Main() {
            seed.value = 5;
            Console.WriteLine("Beginning evolution!");
            Console.WriteLine("Seed Statement: ");
            Console.Write("\t");
            LeaguePrinter.print(seed);
            //loop (forever?) and evolve oracles
        }
    }
}
