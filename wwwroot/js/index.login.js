const uri="/Login";
const form=document.getElementById('login');
const _name=document.getElementById('name');
const _password=document.getElementById('password');
const getToken=()=> {
    localStorage.clear();
    const user = {
        id: -1,
        name:_name.value.trim(),
        password:_password.value.trim(),
        userType:-1
    };
    fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .then(response =>{ 
            if(!response.ok)
            throw new Error('Unauthorized!');
            return response.json()})    
        .then((token) => {
                localStorage.setItem("token",token);
                window.location.href="tasks.html";
            })
        .catch(() => alert("user not found!"));
}
form.onsubmit=(event)=>{
    event.preventDefault();
    getToken();
}
