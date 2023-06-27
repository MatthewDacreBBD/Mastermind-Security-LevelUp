const submitLogin = (event) => {
event.preventDefault();
fetch(
    'https://tgjpfnhlyqbg46ee34d7ttujp40uejnn.lambda-url.af-south-1.on.aws/api/User',
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
            losses: 0,
            authenticationToken: 'temp-token',
        }
    )

    }
).then((res) => {
    if (res.status !== 200) {
    alert("An error has occured, please try again.");
    }
    else {
    window.location.href = '/';
    }
}
)

}

const form = document.getElementById('form');
form.addEventListener('submit', submitLogin);