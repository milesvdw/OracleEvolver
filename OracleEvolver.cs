using System;
using System.Collections.Generic;
using League;

namespace OracleEvolver
{
    public static class OracleEvolverProgram {
        private static Const seed = new Const(1);
        private static List<Oracle<LeagueStatement>> oracles = new List<Oracle<LeagueStatement>>();
        private const int GENERATIONS = 100;
        private const int POPULATION = 100;
        private static int current_generation;
        private const bool PRINT_VERBOSE = false;
        private const int PRINT_FREQUENCY = 1;
        private const MutationStrategy STRATEGY = MutationStrategy.Normal;
        public static void Main() {
            seedPopulation();
            evolve();

            //UNCOMMENT TO UNIT TEST: 
            //LeagueInterpreter.RunTests();
        }
        
        public static void seedPopulation() {
            for(int i = 0; i < POPULATION; i ++) {
                oracles.Add(new Oracle<LeagueStatement>(new Const(1)));
            }
        }

        public static void printOracles() {
            Console.Write("\n Current Generation: ");
            Console.WriteLine(current_generation);
            Console.WriteLine("-------------------");
            foreach(Oracle<LeagueStatement> oracle in oracles) {
                Console.Write("FS: ");
                Console.Write(oracle.fitness);
                Console.Write(" :=");
                oracle.TranscribeProphecy();
            }
        }

        public static void mutateOracles() {
            List<Oracle<LeagueStatement>> newOracles = new List<Oracle<LeagueStatement>>();
            foreach(Oracle<LeagueStatement> oracle in oracles) {
                newOracles.Add(oracle.spawnMutant(STRATEGY));
            }
            oracles.AddRange(newOracles);
        }

        public static void testOraclesFitness() {
            foreach(Oracle<LeagueStatement> oracle in oracles) {
                oracle.testFitness();
            }
        }

        public static void pruneOracles() {
            oracles.Sort(delegate(Oracle<LeagueStatement> oracle1, Oracle<LeagueStatement> oracle2) {
                return oracle1.fitness.CompareTo(oracle2.fitness);
            });
            oracles.RemoveRange(0, 100);
        }

        public static void evolve() {
            Console.WriteLine("Beginning evolution!");
            Console.WriteLine("Seed Population: ");
            Console.Write("\t");
            
            
            for(current_generation = 0; current_generation < GENERATIONS; current_generation ++) {
                if(current_generation % PRINT_FREQUENCY == 0) {
                    printOracles();
                    mutateOracles();
                    testOraclesFitness();
                    pruneOracles();
                }
            }
            
            
            //loop (forever?) and evolve oracles
        }
    }
}
