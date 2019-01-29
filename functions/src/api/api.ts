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
                    name: "medina",
                    version: "0.1.0",
                    description: "investigating the marketplace",
                    dialects:
                    [
                        { name: "aleppo", },
                        { name: "swerve", }
                    ]
                },
                {
                    name: "jargon",
                    version: "0.2.0",
                    description: "language based first year architecture machine",
                    dialects: 
                    [
                        { name: "cicero", },
                        { name: "theano", },
                    ]
                },
                {
                    name: "ersatz",
                    version: "0.1.0",
                    description: "off-brand architecture",
                    dialects: 
                    [
                        { name: "cantley", },
                    ]
                },
            ]
        }
    )

});