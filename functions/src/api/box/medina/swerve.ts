import { DialectManifest } from './../../../models/dialect_manifest';

export const SwerveManifest: DialectManifest = {
    name: "swerve",
    version: "0.0.1",
    description: "a novel market spine to interfere with the old one",
    summary: "",
    inputSteps: 
    [
        {
            number: 0,
            inputType: "",
            instructions: "draw axis",
        },
        {
            number: 1,
            inputType: "",
            instructions: "draw cell"
        },
        {
            number: 2,
            inputType: "",
            instructions: "express density"
        },        
    ]
}