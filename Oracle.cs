
using League;

public enum MutationStrategy {
    Aggressive,
    Normal,
    Slow,
    Equilibrium
}

//this will be our representation of a single evolved algorithm, calculator, or 'oracle'
public class Oracle<ResultType, StatementType> {
    private LeagueStatement prophecy;

    public Oracle (LeagueStatement prophecy) {
        this.prophecy = prophecy;
    }

    public double prophesize() {
        //in this function we will need to run through the prophecy instructions and compute our prediction
        return LeagueInterpreter.interpret(prophecy);
    }

    //this function will mutate the oracle's list of prophecies, and create a new oracle
    public Oracle<ResultType, StatementType> spawnMutant(MutationStrategy strategy) {
        //for now, we don't do any real mutation
        switch(strategy) {
            case MutationStrategy.Equilibrium:
                return new Oracle<ResultType, StatementType>(prophecy);
        }
        return new Oracle<ResultType, StatementType>(prophecy);
            
        
    }


}