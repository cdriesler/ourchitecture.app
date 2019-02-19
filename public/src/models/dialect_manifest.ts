export class DialectManifest {
    name: string;
    version: string;
    description: string;
    summary: string;
    inputSteps: DialectInputStep[] = [];

    constructor(input:any) {
        this.name = input["name"];
        this.version = input["version"];
        this.description = input["description"];
        this.summary = input["summary"];

        for (let step of input["inputSteps"]) {
            this.inputSteps.push(new DialectInputStep(step));
        }
    }
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