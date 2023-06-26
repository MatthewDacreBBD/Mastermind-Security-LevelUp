const submitRegister = (event) => {
    event.preventDefault();
    let username = event.target.elements['username'].value;
    let password = event.target.elements['password'].value;
        fetch(
        'https://localhost:7184/api/User/Login',
        {
            method: 'POST',
            mode: 'cors',
            headers:
            {
            'Content-Type': 'application/JSON'
            },
            //TODO: Sanitize
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
            // sessionStorage.setItem('token', token);s
            window.location.href = `index.html?token=${token}`
            })
        }
        }
        )

}

const form = document.getElementById('form');
form.addEventListener('submit', submitRegister);