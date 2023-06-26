use MastermindDB

INSERT INTO [AppUser] ([username], [password], [passwordSalt], [wins], [losses], [authenticationToken])
VALUES ('JohnDoe', 'password123', 0, 0);

INSERT INTO [AppUser] ([username], [password], [passwordSalt], [wins], [losses], [authenticationToken])
VALUES ('JaneSmith', 'secret456', 2, 1);

INSERT INTO [Game] ([userId], [gameStatus], [score])
VALUES (1, 'InProgress', 100);

INSERT INTO [Game] ([userId], [gameStatus], [score])
VALUES (2, 'Completed', 250);


