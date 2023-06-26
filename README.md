# Mastermind - Security Level Up

In order to run the project, first you will need to start a SQL Server instance. If it is not already running locally/on a cloud provider, see: https://www.prisma.io/dataguide/mssql/setting-up-a-local-sql-server-database

Once the SQL Server instance is up and running, run the scripts located in DB_Scripts to initialise the tables required, begining with the `TableCreation.sql` scripts.

Once that is up and running, open the CSharp project located in `Backend/MastermindService/MastermindService`. In order to run the project, you will need .NET 6 installed and to set the following environment variables: 

Bash:

```sh
export DB_SERVER='your databse url'
export DB_USER='your db user'
export DB_NAME='MastermindDB'
export DB_PASSWORD='your DB password'
```

Powershell:

```ps
$env:DB_SERVER='your databse url'
$env:DB_USER='your db user'
$env:DB_NAME='MastermindDB'
$env:DB_PASSWORD='your DB password'
```

You should now be able to run the CSharp project, and it will connect to the database.

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