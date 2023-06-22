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

    let rowColours = [];
    event.target.parentElement.childNodes.forEach(element => {
        rowColours.push(element.style.background);
    });
    rowColours = rowColours.slice(0, -1);
    rowColours = rowColours.slice(0, 4);

    const results = evaluateRow(rowColours, winningRow);
    console.log(results);

    for (let resultIndex = 4; resultIndex < results.length + 4; resultIndex++) {
        if (results[resultIndex - 4] === 1) {
            gameBoard.childNodes[index].childNodes[0].childNodes[resultIndex].style.background = 'pink';
        }
        if (results[resultIndex - 4] === 2) {
            gameBoard.childNodes[index].childNodes[0].childNodes[resultIndex].style.background = 'black';
        }
    }

    if (rowColours.join(',') === winningRow.join(',')) {
        console.log('You have won the game');
    }
    else {
        if (index !== gameBoard.childNodes.length - 1) {
            gameBoard.childNodes[index + 1].childNodes[0].hidden = false;
            //gameBoard.childNodes[index].childNodes[0].childNodes[4].hidden = true;
            for (let gameItemIndex = 0; gameItemIndex < 4; gameItemIndex++) {
                gameBoard.childNodes[index].childNodes[0].childNodes[gameItemIndex].disabled = true;
            }
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

    for (let indicatorIndex = 0; indicatorIndex < 4; indicatorIndex++) {
        let indicator = document.createElement('button');
        indicator.classList.add('game-item-indicator');
        indicator.style.background = 'white';
        const disableButton = () => {
            indicator.disabled = true;
        };
        indicator.addEventListener('click', disableButton);
        rowButtonsList.appendChild(indicator);
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