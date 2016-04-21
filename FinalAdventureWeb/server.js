var express = require('express');
var bodyParser = require("body-parser");
var http = require('http');

var port = 8070;

var app = express();
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

var router = express.Router();
require('./routes')(router);
app.use('/', router);

app.listen(port);
console.log("Listening on port - " + port);
