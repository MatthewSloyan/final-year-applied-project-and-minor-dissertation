// Global setup
var express = require('express');
var app = express();
var bodyParser = require("body-parser");

// Here we are configuring express to use body-parser as middle-ware
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// Coors setup
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT, OPTIONS")
    res.header("Access-Control-Allow-Headers","Origin, X-Requested-With, Content-Type, Accept");
    next();
});

var server = app.listen(3000, function () {
    var host = server.address().address
    var port = server.address().port
  
    console.log("GroupNotesApp listening at http://%s:%s", host, port)
})

app.get('/', function (req, res) {

    // Using the spawn method built into node, create an instance of the python script and run it.
    // Code apapted and improved from: https://stackoverflow.com/questions/23450534/how-to-call-a-python-function-from-node-js
    
    const { spawn } = require('child_process');
    const chatbot_script = spawn('python', ['chatbot_Server.py']);

    // Wait for output of script
    chatbot_script.stdout.on('data', function(data) {

        console.log(data.toString());
        res.write(data);
        res.end('end');
    });
})