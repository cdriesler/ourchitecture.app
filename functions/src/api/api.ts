import * as express from "express";

// This is the router which will be imported in our
// api hub (the index.ts which will be sent to Firebase Functions).
export let apiRouter = express.Router();

apiRouter.get("/", async function ciceroDesc(req: express.Request, res: express.Response) {

    res.set('Access-Control-Allow-Origin', '*');

    console.info(req.originalUrl);

    res.status(200).json(
        {
            languages: 
            [
                {
                    name: "ersatz",
                    version: "0.1.0",
                    description: "off-brand architecture",
                    dialects: 
                    [
                        {
                            name: "cantley", 
                            version: "0.0.1",
                            description: "riffing off components of bryan cantley's drawings"
                        },
                    ]
                },
                {
                    name: "jargon",
                    version: "0.2.0",
                    description: "language based first year architecture machine",
                    dialects: 
                    [
                        {
                            name: "cicero", 
                            version: "1.0.1",
                            description: "affecting lines that pass through regions with embedded forces"
                        },
                        {
                            name: "theano", 
                            version: "0.2.0",
                            description: "something about a model"
                        },
                    ]
                },
                {
                    name: "sapun",
                    description: "on soap and certainty",
                    dialects:
                    [
                        {
                            name: "medina", 
                            version: "0.0.1",
                            description: "breakdown of the old suq"
                        },
                    ]
                }
            ]
        }
    )

});