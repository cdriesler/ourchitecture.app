export class DialectManifest {
    name: string;
    version: string;
    description: string;
    summary: string;
    inputSteps: DialectInputStep[] = [];
}

export class DialectInputStep {
    number: number;
    inputType: string;
    instructions: string; 
}