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
                    description: "off-brand architecture",
                    dialects: 
                    [
                        "cantley",
                    ]
                },
                {
                    name: "hermes",
                    description: "line and language games",
                    dialects: 
                    [
                        "cicero",
                        "theano",
                    ]
                },
                {
                    name: "prelim",
                    description: "debug utilities!",
                    dialects:
                    [
                        "rhinos",
                    ]
                }
            ]
        }
    )

});