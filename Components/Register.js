const submitLogin = (event) => {
event.preventDefault();
fetch(
    'https://localhost:7184/api/User/Register',
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
        username: event.target.elements['username'].value,
        password: event.target.elements['password'].value,
        wins: 0,
        losses: 0
        }
    )

    }
).then((res) => {
    if (res.status !== 200) {
    alert("An error has occured, please try again.");
    }
    else {
    window.location.href = '/login';
    }
}
)

}

const form = document.getElementById('form');
form.addEventListener('submit', submitLogin);