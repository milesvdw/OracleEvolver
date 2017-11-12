
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using League;

namespace OracleEvolver {
    public enum MutationStrategy {
        Aggressive,
        Normal,
        Slow,
        Equilibrium
    }

    //this will be our representation of a single evolved algorithm, calculator, or 'oracle'
    public class Oracle {
        private LeagueStatement prophecy;
        public double fitness; //for now, this will simply be the percent of games correctly predicted
        
        /**
        * Gets a random double for a uniform distribution (uses Crypto RNG)
         */
        public static double GetRandomDouble() 
        {
            using (RandomNumberGenerator gen = RandomNumberGenerator.Create()) 
            {
                byte[] bytes = new byte[8];
                gen.GetBytes(bytes);
                // start: bit shift 11 and 53 based on double's mantissa bits
                var ul = BitConverter.ToUInt64(bytes, 0) >> 11;
                Double d = ul / (Double)(1UL << 53);
                // end: bit shift logic
                return d;
            }
        }

        public Oracle (LeagueStatement prophecy) {
            this.prophecy = prophecy;
        }

        public double prophesize() {
            //in this function we will need to run through the prophecy instructions and compute our prediction
            return LeagueInterpreter.interpret(prophecy);
        }

        public void testFitness() {
            //todo: here we must repeatedly prophesize() and compare results with 
            //our learning dataset to evaluate the fitness of this algorithm
            this.fitness = GetRandomDouble();
        }

        //this function will mutate the oracle's list of prophecies, and create a new oracle
        public Oracle spawnMutant(MutationStrategy strategy) {
            //for now, we don't do any real mutation
            switch(strategy) {
                case MutationStrategy.Equilibrium:
                    return new Oracle(prophecy);
                case MutationStrategy.Aggressive:
                    return new Oracle(mutateExpressionTree(expression: this.prophecy, mutationChance: .75));
                case MutationStrategy.Normal:
                    return new Oracle(mutateExpressionTree(expression: this.prophecy, mutationChance: .5));
                case MutationStrategy.Slow:
                    return new Oracle(mutateExpressionTree(expression: this.prophecy, mutationChance: .25));
                default:
                    return null;
            }
        }

        private LeagueStatement mutateExpressionTree(LeagueStatement expression, double mutationChance) {
            //this is the biggest sticking point in terms of generalizing the approach. How do you
            //generalize mutation??
            double roll = GetRandomDouble();
            if(roll < mutationChance) {//mutate!
                expression = expression.mutate();
            }
            foreach(LeagueStatement s in expression.getChildren()) mutateExpressionTree(s, mutationChance);
            return expression;
        }
        
        public void TranscribeProphecy() {
            DSLPrinter.print(this.prophecy);
        }


    }

}