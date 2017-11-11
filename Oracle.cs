
using System;
using League;

namespace OracleEvolver {
    public enum MutationStrategy {
        Aggressive,
        Normal,
        Slow,
        Equilibrium
    }

    //this will be our representation of a single evolved algorithm, calculator, or 'oracle'
    public class Oracle<StatementType> {
        private LeagueStatement prophecy;
        public double fitness; //for now, this will simply be the percent of games correctly predicted
        private Random rand = new Random();

        public Oracle (LeagueStatement prophecy) {
            this.prophecy = prophecy;
        }

        public double prophesize() {
            //in this function we will need to run through the prophecy instructions and compute our prediction
            return LeagueInterpreter.interpret(prophecy);
        }

        public void testFitness() {
            this.fitness = ((double)rand.Next(0, 100))/100;
        }

        //this function will mutate the oracle's list of prophecies, and create a new oracle
        public Oracle<StatementType> spawnMutant(MutationStrategy strategy) {
            //for now, we don't do any real mutation
            switch(strategy) {
                case MutationStrategy.Equilibrium:
                    return new Oracle<StatementType>(prophecy);
            }
            return new Oracle<StatementType>(prophecy);
                
            
        }

        public void TranscribeProphecy() {
            LeaguePrinter.print(this.prophecy);
        }


    }

}