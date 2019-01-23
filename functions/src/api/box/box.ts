import * as express from "express";

import { CantleyManifest } from './ersatz/cantley/cantley';

const mappings = {
    ersatz_cantley: CantleyManifest,
}

export let boxRouter = express.Router();

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