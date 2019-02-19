
import * as express from "express";

//motley


//ersatz
import { CantleyManifest } from './ersatz/cantley/cantley';

//jargon
import { CiceroManifest } from './jargon/cicero';
import { TheanoManifest } from './jargon/theano';

//medina
import { AleppoManifest } from './medina/aleppo';
import { SwerveManifest } from './medina/swerve';


//TODO: Find a better way 
const mappings = {
    ersatz_cantley: CantleyManifest,

    jargon_cicero: CiceroManifest,
    jargon_theano: TheanoManifest,

    medina_aleppo: AleppoManifest,
    medina_swerve: SwerveManifest,
}

export let boxRouter = express.Router();

boxRouter.get("/", async function allDialects(req: express.Request, res: express.Response) {
    res.status(200).json(mappings);
})

boxRouter.get("/:language/:dialect", async function dialectManifest(req: express.Request, res: express.Response) {

    res.set('Access-Control-Allow-Origin', '*');

    let manifest = mappings[req.params.language + "_" + req.params.dialect];

    if (manifest != undefined) {
        res.status(200).json(manifest);
    }
    else {
        res.status(400).json({error: "Dialect manifest not found."})
    }
    
});