using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using League;
using Newtonsoft.Json.Linq;

namespace OracleEvolver
{
    public static class OracleEvolverProgram {
        private static List<Oracle> oracles = new List<Oracle>();
        private const string answerAccessor = "teams[0].win";
        private const int GENERATIONS = 5000;
        private const int REPRODUCTION_RATE = 3;
        private const int POPULATION = 10;
        private const bool USE_TRAINING_DATA = true;
        private static int current_generation;
        private const bool PRINT_VERBOSE = false;
        private const int PRINT_FREQUENCY = 1;
        private const MutationStrategy STRATEGY = MutationStrategy.Slow;
        public static void Main() {
            seedPopulation();
            evolve();

            //UNCOMMENT TO UNIT TEST: 
            //LeagueInterpreter.RunTests();
        }
        
        public static void seedPopulation() {
            for(int i = 0; i < POPULATION; i ++) {
                oracles.Add(new Oracle(new IntVal(1)));
            }
        }

        public static void printOracles() {
            Console.Write("\n Current Generation: ");
            Console.WriteLine(current_generation);
            Console.WriteLine("-------------------");
            foreach(Oracle oracle in oracles) {
                Console.Write("FS: ");
                Console.Write(oracle.fitness);
                Console.Write(" :=");
                oracle.TranscribeProphecy();
            }
        }

        public static void mutateOracles() {
            List<Oracle> newOracles = new List<Oracle>();
            for(int i = 0; i < REPRODUCTION_RATE; i++) { //todo reorder this loop to prevent cache misses?
                foreach(Oracle oracle in oracles) {
                    newOracles.Add(oracle.spawnMutant(STRATEGY));
                }
            }
            oracles.AddRange(newOracles);
        }

        //returns a batch of 200 matches
        private static List<JToken> getMatches() {
            List<JToken> matches = new List<JToken>();
            if(USE_TRAINING_DATA) {
                using (StreamReader r = new StreamReader("LeagueDSL/TrainingData/matches1.json"))
                {
                    string json = r.ReadToEnd();
                    return JObject.Parse(json).SelectTokens("matches").ToList(); //MILES TODO check that this is OK...
                }
            }
            return new List<JToken>();
        }

        public static void testOraclesFitness() {
            List<JToken> matches = getMatches();
            foreach(JToken match in matches[0].Children()) {
                LeagueInterpreter.match = match;
                double target = 0;
                JToken team1 = match.SelectToken("teams[0]"); //MILES TODO refactor this so it can be passed in as a delegate
                JToken team2 = match.SelectToken("teams[1]");
                if((int) team1.SelectToken("teamId") == 100) target = (string)team1.SelectToken("win") == "Win" ? 1 : 0;
                else target = (string)team2.SelectToken("win") == "Win" ? 1 : 0;
                Selection.ListwiseLocalCompetition.AssignFitness(oracles, target);
            }
            oracles.ForEach(o => o.normalizeFitness(matches.Count));
        }
        public static void pruneOracles() {
            oracles = oracles.OrderBy(o => o.fitness).ToList();
            oracles.RemoveRange(0, oracles.Count - POPULATION);
            oracles.RemoveRange(0, oracles.Count - POPULATION);
            oracles.ForEach(o => o.fitness = 0); // reset fitness for next generation
        }

        public static void evolve() {
            Console.WriteLine("Beginning evolution!");
            Console.WriteLine("Seed Population: ");
            Console.Write("\t");
            
            
            for(current_generation = 0; current_generation < GENERATIONS; current_generation ++) {
                if(current_generation % PRINT_FREQUENCY == 0) {
                    Console.Write("Generation: ");
                    Console.WriteLine(current_generation);
                    mutateOracles();
                    testOraclesFitness();
                    pruneOracles();
                    if(current_generation % 1 == 0) printOracles(); //only print every 10 generations
                    //printOracles();
                }
            }
        }
    }
}
