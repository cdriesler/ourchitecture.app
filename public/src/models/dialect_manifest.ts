export class DialectManifest {
    name: string;
    version: string;
    description: string;
    inputSteps: DialectInputStep[] = [];
}

export class DialectInputStep {
    number: number;
    inputType: string;
    instructions: string; 

    //Interface-only values
    completed: boolean;

    constructor(input:any) {
        this.number = input["number"];
        this.inputType = input["inputType"];
        this.instructions = input["instructions"]
    }
}