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
}