import * as express from "express";
import * as functions from "firebase-functions";
import * as boxApi from "./api/box/box";
import * as rootApi from "./api/api";

const app = express();
app.disable("x-powered-by");

app.use("/api", rootApi.apiRouter);
app.use("/api/box", boxApi.boxRouter);

/*
app.get("*", async (req: express.Request, res: express.Response) => {
	res.status(404).send("This route does not exist.");
});
*/

exports.api = functions.https.onRequest(app);