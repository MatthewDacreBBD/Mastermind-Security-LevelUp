const submitRegister = (event) => {
    event.preventDefault();
    let username = event.target.elements['username'].value;
    let password = event.target.elements['password'].value;
    fetch(
        'https://tgjpfnhlyqbg46ee34d7ttujp40uejnn.lambda-url.af-south-1.on.aws/api/User/Login',
        {
            method: 'POST',
            mode: 'cors',
            headers:
            {
                'Content-Type': 'application/JSON'
            },
            body: JSON.stringify(
                {
                    username,
                    password
                }
            )

        }
    ).then((res) => {
        if (res.status !== 200) {
            alert("An error has occured, please try again.");
        }
        else {
            res.json().then((j) => {
                let token = j['authenticationToken'];
                window.location.href = `index.html?token=${token}`;
            });
        }
    }
    )

}

const form = document.getElementById('form');
form.addEventListener('submit', submitRegister);