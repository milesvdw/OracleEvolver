using System;
using System.Collections.Generic;
using OracleEvolver;

namespace Selection {


    public static class ListwiseLocalCompetition {

        //for now, this simply halves (if positive) the fitness of each oracle
        //for each neighbor oracle with the same structure
        public static void AssignFitness(List<Oracle> population, double target) {
            List<Oracle> valid = new List<Oracle>();
            //special case for first element
            int err = population[0].testFitness(target);
            if(err != -1) valid.Add(population[0]);
            if(population.Count > 1 && population[0].Equals(population[1])) population[0].fitness = population[0].fitness - .5;
            for(int i = 1; i < population.Count-1; i++) {
                err = population[i].testFitness(target);
                if(population[i].Equals(population[i-1])) 
                    population[i].fitness = population[i].fitness - .5;
                if(population[i].Equals(population[i+1])) 
                    population[i].fitness = population[i].fitness - .5;
                if(err != -1) valid.Add(population[i]);
            }
            
            err = population[population.Count-1].testFitness(target);
            if(population[population.Count-2].Equals(population[population.Count-1])) population[population.Count-1].fitness = population[population.Count-1].fitness - .5;
            if(err != -1) valid.Add(population[population.Count-1]);
        }

    }



}