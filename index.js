const incrementColour = (colour) => {
  switch (colour) {
    case "red":
      return "blue";
    case "blue":
      return "green";
    case "green":
      return "yellow";
    case "yellow":
      return "red";
    default:
      return "red";
  }
};

const getWinningRow = () => {
  const row = [];
  const colourOptions = ["red", "blue", "green", "yellow"];
  for (let i = 0; i < 4; i++) {
    row.push(colourOptions[Math.floor(Math.random() * colourOptions.length)]);
  }

  return row;
};

const evaluateRow = (submittedRow, winningRow) => {
  const results = [0, 0, 0, 0];
  submittedRow.forEach((colour, index) => {
    if (winningRow[index] === colour) {
      results[index] = 2;
    } else if (winningRow.includes(colour)) {
      results[index] = 1;
    }
  });
  return results;
};

const getIdFromToken = (token) => {
  const [headerEncoded, payloadEncoded, signatureEncoded] = token.split(".");

  const header = atob(headerEncoded.replace(/-/g, "+").replace(/_/g, "/"));

  const payload = atob(payloadEncoded.replace(/-/g, "+").replace(/_/g, "/"));

  const headerData = JSON.parse(header);

  const payloadData = JSON.parse(payload);

  return payloadData.Id;
};

const submitRow = (event, winningRow, index, gameBoard) => {
  let rowColours = [];
  event.target.parentElement.childNodes.forEach((element) => {
    rowColours.push(element.style.background);
  });
  rowColours = rowColours.slice(0, -1);
  rowColours = rowColours.slice(0, 4);

  const results = evaluateRow(rowColours, winningRow);

  for (let resultIndex = 4; resultIndex < results.length + 4; resultIndex++) {
    if (results[resultIndex - 4] === 1) {
      gameBoard.childNodes[index].childNodes[0].childNodes[
        resultIndex
      ].style.background = "pink";
    }
    if (results[resultIndex - 4] === 2) {
      gameBoard.childNodes[index].childNodes[0].childNodes[
        resultIndex
      ].style.background = "black";
    }
  }

  if (rowColours.join(",") === winningRow.join(",")) {
    alert("Congratulations");
    let userId = Number.parseInt(getIdFromToken(token));
    let payload = JSON.stringify({
      userId: userId,
      gameStatus: "won",
      score: 5 - index,
    });
    fetch(
      "https://qzzmhm4uyv34m26zfz6njjjyye0djusp.lambda-url.af-south-1.on.aws/api/Game",
      {
        method: "POST",
        mode: "cors",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/JSON",
        },
        body: payload,
      }
    ).then((res) => {
      if (res.status !== 200) {
        alert("An error has occured, please try again.");
      } else {
        window.location.href = `index.html?token=${token}`;
      }
    });
  } else {
    if (index !== gameBoard.childNodes.length - 1) {
      gameBoard.childNodes[index + 1].childNodes[0].hidden = false;
      //gameBoard.childNodes[index].childNodes[0].childNodes[4].hidden = true;
      for (let gameItemIndex = 0; gameItemIndex < 4; gameItemIndex++) {
        gameBoard.childNodes[index].childNodes[0].childNodes[
          gameItemIndex
        ].disabled = true;
      }
    }
  }
};

const gameBoard = document.getElementById("game-board");
const winningRow = getWinningRow();

for (let rowIndex = 0; rowIndex < 4; rowIndex++) {
  const rowButtonsList = document.createElement("ul");
  rowButtonsList.classList.add("game-row");
  rowButtonsList.hidden = rowIndex !== 0;
  for (let buttonIndex = 0; buttonIndex < 4; buttonIndex++) {
    let button = document.createElement("button");
    button.classList.add("game-item");
    button.style.background = "white";
    button.addEventListener("click", (event) => {
      event.target.style.background = incrementColour(
        event.target.style.background
      );
    });
    rowButtonsList.appendChild(button);
  }

  for (let indicatorIndex = 0; indicatorIndex < 4; indicatorIndex++) {
    let indicator = document.createElement("button");
    indicator.classList.add("game-item-indicator");
    indicator.style.background = "white";
    const disableButton = () => {
      indicator.disabled = true;
    };
    indicator.addEventListener("click", disableButton);
    rowButtonsList.appendChild(indicator);
  }

  const submitButton = document.createElement("button");
  submitButton.innerText = "Submit";
  submitButton.classList.add("submit");
  submitButton.addEventListener("click", (event) =>
    submitRow(event, winningRow, rowIndex, gameBoard)
  );
  rowButtonsList.appendChild(submitButton);
  const rowItem = document.createElement("li");
  rowItem.appendChild(rowButtonsList);
  gameBoard.appendChild(rowItem);
}

// Mock data
const leaderboardData = [
  { position: 1, username: "John", score: 5000 },
  { position: 2, username: "Alice", score: 4500 },
  { position: 3, username: "Bob", score: 4000 },
  { position: 4, username: "William", score: 3500 },
  { position: 5, username: "Mary", score: 3000 },
  { position: 6, username: "Samantha", score: 2500 },
];

const urlParams = new URLSearchParams(window.location.search);
const token = urlParams.get("token");

const populateLeaderboard = (token) => {
  const leaderboardList = document.getElementById("leaderboard-list");

  fetch(
    "https://tgjpfnhlyqbg46ee34d7ttujp40uejnn.lambda-url.af-south-1.on.aws/api/User",
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  )
    .then((res) => {
      return res.json();
    })
    .then((users) => {
      fetch(
        "https://qzzmhm4uyv34m26zfz6njjjyye0djusp.lambda-url.af-south-1.on.aws/api/Game",
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
        .then((res) => {
          return res.json();
        })
        .then((games) => {
          let leaderboard = [];

          for (const user of users) {
            for (const game of games) {
              if (user.userId === game.userId) {
                leaderboard.push({
                  username: user.userName,
                  score: game.score,
                });
              }
            }
          }

          leaderboard.sort((a, b) => b.score - a.score);

          let leaderboardIndex = 0;

          leaderboard.forEach((record) => {
            leaderboardIndex++;
            const listItem = document.createElement("li");
            listItem.innerHTML = `
                    ${
                      (leaderboardIndex === 1 &&
                        '<i class="fas fa-medal" style="color: gold; margin-left: 8px; margin-right: 8px;"></i>') ||
                      (leaderboardIndex === 2 &&
                        '<i class="fas fa-medal" style="color: silver; margin-left: 8px; margin-right: 8px;"></i>') ||
                      (leaderboardIndex === 3 &&
                        '<i class="fas fa-medal" style="color: #CD7F32; margin-left: 8px; margin-right: 8px;"></i>') ||
                      `<span class="position">${leaderboardIndex}</span>`
                    }
                    <span class="username">${record.username}</span>
                    <span class="score">${record.score}</span>
                  `;
            leaderboardList.appendChild(listItem);
          });
        })
        .catch((error) => {
          console.log(error);
        });
    })
    .catch((error) => console.log(error));
};

populateLeaderboard(token);
