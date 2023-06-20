use MastermindDB

INSERT INTO [User] ([username], [password], [passwordSalt], [wins], [losses], [authenticationToken])
VALUES ('JohnDoe', 'password123', 'somesalt', 0, 0, 'token1234567890');

INSERT INTO [User] ([username], [password], [passwordSalt], [wins], [losses], [authenticationToken])
VALUES ('JaneSmith', 'secret456', 'anothersalt', 2, 1, 'token0987654321');

INSERT INTO [Game] ([userId], [gameStatus], [score])
VALUES (1, 'InProgress', 100);

INSERT INTO [Game] ([userId], [gameStatus], [score])
VALUES (2, 'Completed', 250);