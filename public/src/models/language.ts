import { HttpClient } from '@angular/common/http';
import { DialectManifest } from './dialect_manifest';

export class Language{
    name: string;
    version: string;
    description: string;
    dialectNames: string[] = [];
    dialects: DialectManifest[] = [];

    constructor(lang:Object) {
        this.name = lang["name"];
        this.version = lang["version"];
        this.description = lang["description"];

        for(let dialect of lang["dialects"]) {
            this.dialectNames.push(dialect["name"]);
        }
    }
}

export class Dialect{
    name: string;
    version: string;
    description: string;

    constructor(dialect:Object) {
        this.name = dialect["name"];
        this.version = dialect["version"];
        this.description = dialect["description"];
    }
}