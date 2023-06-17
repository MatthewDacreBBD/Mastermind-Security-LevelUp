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

const submitRow = (event, winningRow) => {
    let rowColours = [];
    event.target.parentElement.childNodes.forEach(element => {
        rowColours.push(element.style.background)
    });
    rowColours = rowColours.slice(0, -1);
    if (rowColours.join(',') === winningRow.join(',')) {
        console.log('You have won the game');
    }
    else {
       console.log(rowColours)
    }
}

const gameBoard = document.getElementById('game-board');
const winningRow = getWinningRow();

for (let rowIndex = 0; rowIndex < 4; rowIndex++) {
    const rowButtonsList = document.createElement('ul');
    rowButtonsList.classList.add('game-row');
    for (let buttonIndex = 0; buttonIndex < 4; buttonIndex++) {
        let button = document.createElement('button');
        button.classList.add('game-item');
        button.addEventListener('click', (event) => {
            event.target.style.background = incrementColour(event.target.style.background);
        });
        rowButtonsList.appendChild(button);
    }
    const submitButton = document.createElement('button');
    submitButton.innerText = 'Submit';
    submitButton.addEventListener('click', event => submitRow(event, winningRow));
    rowButtonsList.appendChild(submitButton);
    const rowItem = document.createElement('li');
    rowItem.appendChild(rowButtonsList);
    gameBoard.appendChild(rowItem);
}

console.log(winningRow);