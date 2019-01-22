export class Language{
    name: string;
    version: string;
    description: string;
    dialects: Dialect[] = [];

    constructor(lang:Object) {
        this.name = lang["name"];
        this.version = lang["version"];
        this.description = lang["description"];

        for(let dialect of lang["dialects"]) {
            this.dialects.push(new Dialect(dialect));
        }
    }
}

class Dialect{
    name: string;
    version: string;
    description: string;

    constructor(dialect:Object) {
        this.name = dialect["name"];
        this.version = dialect["version"];
        this.description = dialect["description"];
    }
}