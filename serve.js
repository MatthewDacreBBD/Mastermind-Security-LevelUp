const express = require('express');
const path = require('path');
var fs = require('fs');
var https = require('https');
var certificate = fs.readFileSync('selfsigned.crt', 'utf8');
var privateKey  = fs.readFileSync('selfsigned.key', 'utf8');

var credentials = {key: privateKey, cert: certificate};
const app = express();
var httpsServer = https.createServer(credentials, app);
const port = process.env.PORT || 8443;

// sendFile will go here
app.get('/login', function(req, res) {
  res.sendFile('./Components/login.html', {root: __dirname});
});
app.get('/register', function(req, res) {
  res.sendFile('./Components/register.html', {root: __dirname});
})

app.listen(port);
console.log('Server started at http://localhost:' + port);