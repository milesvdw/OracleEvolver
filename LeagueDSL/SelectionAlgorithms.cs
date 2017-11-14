using System.Collections.Generic;
using OracleEvolver;

namespace Selection {


    public static class ListwiseLocalCompetition {

        //for now, this simply halves (if positive) the fitness of each oracle
        //for each neighbor oracle with the same structure
        public static List<Oracle> Select(List<Oracle> population, double target) {
            Oracle[] list = population.ToArray();

            //special case for first element
            list[0].testFitness(target);
            if(list.Length > 1 && list[0].Equals(list[1])) list[0].fitness = list[0].fitness / 2;
            for(int i = 1; i < list.Length-1; i++) {
                list[i].testFitness(target);
                if(list[i].Equals(list[i-1])) list[i].fitness = list[i].fitness / 2;
                if(list[i].Equals(list[i+1])) list[i].fitness = list[i].fitness / 2;
            }
            
            list[list.Length-1].testFitness(target);
            if(list[list.Length-2].Equals(list[list.Length-1])) list[list.Length-1].fitness = list[list.Length-1].fitness / 2;

            return new List<Oracle>(list);
        }

    }



}