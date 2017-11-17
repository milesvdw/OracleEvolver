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
    public class Oracle : IEquatable<Oracle> {
        private LeagueStatement prophecy;
        public bool perfect = false;
        public const double STRING_EVOLUTION_PROBABILITY = .5;
        public double fitness; //for now, this will simply be the percent of games correctly predicted
        public HashSet<double> answers = new HashSet<double>();
        private static int[] tmp = {0, 1};
        private static List<int> acceptableValues = new List<int>(tmp); //todo make this less terrifyingly bad code

        public bool Equals(Oracle other) 
        {
            return this.prophecy.Equals(other.prophecy);
        }
        /**
        * Gets a random double for a uniform distribution (uses Crypto RNG)
         */
        public static double GetRandomPercent() 
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

        public LeagueReturn prophesize() {
            //in this function we will need to run through the prophecy instructions and compute our prediction
            return LeagueInterpreter.interpret(prophecy);
        }


        //if this returns -1, that indicates a runtime error in the prophecy
        public int testFitness(double target) {
            //todo: here we must repeatedly prophesize() and compare results with 
            //our learning dataset to evaluate the fitness of this algorithm

            LeagueReturn prophesization = prophesize();
            if (prophesization is ValidReturn) {
                double ret = (prophesization as ValidReturn).value;
                int answer =  ret > 1 ? 1 : ret < 0 ? 0 : ret > .5 ? 1 : 0; //an unfortunately complex way of getting around overflow errors
                if(answer == target) {
                    this.fitness += 1;
                }
                answers.Add(ret);
                return 1;
            }
            else return -1; //an error occured
        }

        public void normalizeFitness(double maximum_possible_fitness) {
            if(fitness == maximum_possible_fitness) perfect = true;
            if(answers.Count <= 1) {//less points for producing the same answer every time (e.g. just guessing the same team won over and over)
                this.fitness = (this.fitness / 5) / maximum_possible_fitness;
            } else {
                this.fitness = this.fitness/maximum_possible_fitness;
            }
            answers.Clear();
        }

        //this function will mutate the oracle's list of prophecies, and create a new oracle
        public Oracle spawnMutant(MutationStrategy strategy) {
            //for now, we don't do any real mutation
            switch(strategy) {
                case MutationStrategy.Equilibrium:
                    return new Oracle(prophecy);
                case MutationStrategy.Aggressive:
                    return new Oracle(mutateExpressionTree(expression: (LeagueStatement) this.prophecy, mutationChance: .75));
                case MutationStrategy.Normal:
                    return new Oracle(mutateExpressionTree(expression: (LeagueStatement) this.prophecy, mutationChance: .5));
                case MutationStrategy.Slow:
                    return new Oracle(mutateExpressionTree(expression: (LeagueStatement) this.prophecy, mutationChance: .25));
                default:
                    return null;
            }
        }

        //mutateExpressionTree :: LeagueStatement -> Different LeagueStatement
        private LeagueStatement mutateExpressionTree(LeagueStatement expression, double mutationChance) {
            //this is the biggest sticking point in terms of generalizing the approach. How do you
            //generalize mutation??
            double roll = GetRandomPercent();
            List<LeagueStatement> newChildren = new List<LeagueStatement>();
            foreach(LeagueStatement child in expression.getChildren()) {
                newChildren.Add(mutateExpressionTree(child, mutationChance));
            }
            LeagueStatement newExpression = (LeagueStatement) expression.Clone();
            newExpression.setChildren(newChildren.ToArray());
            if(roll < mutationChance) {//mutate!
                newExpression = newExpression.mutate();
            }
            
            return newExpression;
        }

        public void TranscribeProphecy() {
            DSLPrinter.print(this.prophecy);
        }


    }

}