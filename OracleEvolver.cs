using System;
using League;

namespace OracleEvolver
{
    public class OracleEvolver {
        private Const seed = new Const();
        
        public OracleEvolver() {
            seed.value = 1;
        }

        public static void Main() {
            Console.WriteLine("Beginning evolution!");
            //loop (forever?) and evolve oracles
        }
    }
}
