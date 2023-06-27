const express = require('express');
const path = require('path');
var fs = require('fs');
var https = require('https');
var certificate = fs.readFileSync('selfsigned.crt', 'utf8');
var privateKey  = fs.readFileSync('selfsigned.key', 'utf8');

var credentials = {key: privateKey, cert: certificate};
const app = express();
app.use(express.static(__dirname))
var httpsServer = https.createServer(credentials, app);
const port = process.env.PORT || 8443;

app.get('/login', function(req, res) {
  res.sendFile('login.html', {root: './Components'});
});
app.get('/register', function(req, res) {
  res.sendFile('register.html', { root: './Components'});
})

let server = https.createServer(credentials, app);
server.listen(port, () => {
console.log('Server started at https://localhost:' + port + '/login');
})
