const incrementColour = (colour) => {
    switch (colour) {
        case 'red':
            return 'blue';
        case 'blue':
            return 'green';
        case 'green':
            return 'yellow';
        case 'yellow':
            return 'red';
        default:
            return 'red';
    }
}

const getWinningRow = () => {
    const row = [];
    const colourOptions = ['red', 'blue', 'green', 'yellow'];
    for (let i = 0; i < 4; i++) {
        row.push(colourOptions[Math.floor(Math.random() * colourOptions.length)]);
    }

    return row;
}

const evaluateRow = (submittedRow, winningRow) => {
    const results = [0, 0, 0, 0];
    submittedRow.forEach((colour, index) => {
        if (winningRow[index] === colour) {
            results[index] = 2;
        }
        else if (winningRow.includes(colour)) {
            results[index] = 1;
        }
    })
    return results;
}
const submitRow = (event, winningRow, index, gameBoard) => {
    const colourResults = ['orange', 'yellow', 'green'];
    let rowColours = [];
    event.target.parentElement.childNodes.forEach(element => {
        rowColours.push(element.style.background)
    });
    rowColours = rowColours.slice(0, -1);
    if (rowColours.join(',') === winningRow.join(',')) {
        alert('You have won the game. Congratulations');
        // TODO: Redirect to leaderboard page with user ID
        // window.location(/leaderboard)
    }
    else {
       if (index !== gameBoard.childNodes.length - 1) {
        gameBoard.childNodes[index + 1].childNodes[0].hidden = false;
        gameBoard.childNodes[index].childNodes[0].childNodes[4].hidden = true;
        gameBoard.childNodes[index].childNodes[0].childNodes.forEach((button) => button.disabled = true);
       }
       let result = evaluateRow(rowColours, winningRow);
       for (let i = 0; i < result.length; i++) {
            event.target.parentElement.childNodes[i].style.border = `5px dashed ${colourResults[result[i]]}`;
       }
    }
}

const gameBoard = document.getElementById('game-board');
const winningRow = getWinningRow();

for (let rowIndex = 0; rowIndex < 4; rowIndex++) {
    const rowButtonsList = document.createElement('ul');
    rowButtonsList.classList.add('game-row');
    rowButtonsList.hidden = rowIndex !== 0
    for (let buttonIndex = 0; buttonIndex < 4; buttonIndex++) {
        let button = document.createElement('button');
        button.classList.add('game-item');
        button.style.background = 'white';
        button.addEventListener('click', (event) => {
            event.target.style.background = incrementColour(event.target.style.background);
        });
        rowButtonsList.appendChild(button);
    }
    const submitButton = document.createElement('button');
    submitButton.innerText = 'Submit';
    submitButton.classList.add('submit')
    submitButton.addEventListener('click', event => submitRow(event, winningRow, rowIndex, gameBoard));
    rowButtonsList.appendChild(submitButton);
    const rowItem = document.createElement('li');
    rowItem.appendChild(rowButtonsList);
    gameBoard.appendChild(rowItem);
}

console.log(winningRow);