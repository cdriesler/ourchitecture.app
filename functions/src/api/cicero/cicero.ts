import * as express from "express";
import * as time from "../../services/timeService";

// This is the router which will be imported in our
// api hub (the index.ts which will be sent to Firebase Functions).
export let ciceroRouter = express.Router();

ciceroRouter.get("/", async function ciceroDesc(req: express.Request, res: express.Response) {

    let t = new Date();

    res.set('Access-Control-Allow-Origin', '*');
    //res.status(200).send("cicero descriptions");

    console.info(time.GetCurrentTime() + " : " + req.originalUrl);

    res.status(200).json({message: "TODO: Top-level information for cicero."})

});

ciceroRouter.get("/test", async function ciceroDescTest(req: express.Request, res: express.Response) {

    res.set('Access-Control-Allow-Origin', '*');

    let data = {}
    //res.status(200).send("cicero descriptions test");

    res.status(200).json({message: "cicero descriptions from api test"})

});

// Useful: Let's make sure we intercept un-matched routes and notify the client with a 404 status code
ciceroRouter.get("*", async (req: express.Request, res: express.Response) => {
	res.status(404).send("This route does not exist.");
});