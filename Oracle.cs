
using System;
using System.Collections.Generic;
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
        private static MathOp[] mathOps = {new Add(new Const(1), new Const(1)),
                                            new Subtract(new Const(1), new Const(1)),
                                            new Multiply(new Const(1), new Const(1)),
                                            new Divide(new Const(1), new Const(1))};
        public double fitness; //for now, this will simply be the percent of games correctly predicted
        private Random rand = new Random();
        private double DELETION_CHANCE = .2;

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
            this.fitness = ((double)rand.Next(0, 100))/100;
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
            }
            return new Oracle(prophecy);
        }

        private LeagueStatement mutateExpressionTree(LeagueStatement expression, double mutationChance) {]
            //this is the biggest sticking point in terms of generalizing the approach. How do you
            //generalize mutation??
            double roll = rand.Next(0, 100)/100;
            if(roll < mutationChance) {//mutate! {
                return mutateExpression(expression);
            }
            else return expression;
        }

        //it might be the case that all these mutations should really live in the DSL itself. hard to say.
        LeagueStatement mutateExpression(LeagueStatement expression) {
            double roll = rand.Next(0, 100)/100;
            if(roll < DELETION_CHANCE) return new Const(1); // note that a constant of 1 is equivalent to terminating an expression tree
            if(expression is MathOp) return mutateMathExpression(expression as MathOp);
            if(expression is If) return mutateIfExpression(expression as If);
            return expression; //unsupported expression type?
        }

        LeagueStatement mutateMathExpression(LeagueStatement expression) {
            double roll = rand.Next(0, 100) / 100;

            MathOp newExpression = mathOps[(int)(roll*mathOps.Length)];
            if(newExpression is BinaryMathOp) {
                if(expression is BinaryMathOp) {
                    (newExpression as BinaryMathOp).left = (expression as BinaryMathOp).left;
                    (newExpression as BinaryMathOp).right = (expression as BinaryMathOp).right;
                } else {
                    roll = rand.Next(0, 1);
                    if(roll == 1) (newExpression as BinaryMathOp).left = (expression as UnaryMathOp).inner;
                    else (newExpression as BinaryMathOp).right = (expression as UnaryMathOp).inner;
                }
            } else {
                if(expression is UnaryMathOp) (newExpression as UnaryMathOp).inner = (expression as UnaryMathOp).inner;
                else {
                    roll = rand.Next(0, 1);
                    if(roll == 1) (newExpression as UnaryMathOp).inner = (expression as BinaryMathOp).left;
                    else (newExpression as UnaryMathOp).inner = (expression as BinaryMathOp).right;
                }
            }
            return newExpression;

        }

        private LeagueStatement mutateIfExpression(LeagueStatement expression) {
            return expression; //todo mutate
        }


        public void TranscribeProphecy() {
            LeaguePrinter.print(this.prophecy);
        }


    }

}