
public enum MutationStrategy {
    Aggressive,
    Normal,
    Slow,
    Equilibrium
}

//this will be our representation of a single evolved algorithm, calculator, or 'oracle'
public class Oracle<ResultType> {
    private Statement prophecy;
    private DSLInterpreter calculator;

    public Oracle (Statement prophecy, DSLInterpreter calculator) {
        this.prophecy = prophecy;
        this.calculator = calculator;
    }

    public ResultType prophesize() {
        //in this function we will need to run through the prophecy instructions and compute our prediction
        calculator.interpret(prophecy);
    }

    //this function will mutate the oracle's list of prophecies, and create a new oracle
    public Oracle<ResultType> spawnMutant(MutationStrategy strategy) {
        //for now, we don't do any real mutation
        switch(strategy) {
            case MutationStrategy.Equilibrium:
                return new Oracle(prophecy, calculator);
        }
        return new Oracle(prophecy, calculator);
            
        
    }


}