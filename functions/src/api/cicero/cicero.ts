import * as express from "express";

// This is the router which will be imported in our
// api hub (the index.ts which will be sent to Firebase Functions).
export let ciceroRouter = express.Router();

ciceroRouter.get("/", async function ciceroDesc(req: express.Request, res: express.Response) {

    res.status(200).send("cicero descriptions");

});

// Useful: Let's make sure we intercept un-matched routes and notify the client with a 404 status code
ciceroRouter.get("*", async (req: express.Request, res: express.Response) => {
	res.status(404).send("This route does not exist.");
});