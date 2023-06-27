# Mastermind - Security Level Up

## NB

Running the project locally requires you to check out the `local-setup` branch in the git repository. The main branch calls the remote API, so you will not be able to view the database yourself. All other functionality is unchanged.

The frontend expects the backend to be run under the IIS Express profile in visual studio. This runs the Game Service on port 44311 and the Identity server on port 44372.

## Setup

In order to run the project, first you will need to start a SQL Server instance. If it is not already running locally/on a cloud provider, see: https://www.prisma.io/dataguide/mssql/setting-up-a-local-sql-server-database

Once the SQL Server instance is up and running, run the scripts located in DB_Scripts to initialise the tables required, begining with the `TableCreation.sql` scripts. THe origional implementation uses two seperate databases for the identity and resource server, but a single database for local testing should work just as well.

Once that is up and running, open the two CSharp projects located in `Backend/MastermindService/MastermindService` and `Backend/MastermindGameService/MastermindGameService`. In order to run the projects, you will need .NET 6 installed and to set the following environment variables for *both* projects: 

Bash:

```sh
export DB_SERVER='your databse url'
export DB_USER='your db user'
export DB_NAME='MastermindDB'
export DB_PASSWORD='your DB password'
export JWT_KEY='random 32 bit key'
```

Powershell:

```ps
$env:DB_SERVER='your databse url'
$env:DB_USER='your db user'
$env:DB_NAME='MastermindDB'
$env:DB_PASSWORD='your DB password'
$env:JWT_KEY='random 32 bit key'

```

You should now be able to run the CSharp project, and it will connect to the database. You can verify that the projects are working using the swagger pages that will open.

The frontend needs to be served via https in order to communicate with the backend API. In order to do this, a private key and certificate need to be generated. This can be done as follows:

```sh
sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout ./selfsigned.key -out selfsigned.crt
```

Next, run the frontend by running these two commands: 

```bash
npm install
node server.js
```

The project should now be running and functional.