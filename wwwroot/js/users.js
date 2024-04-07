const uri = '/User';
let users = [];
let token = localStorage.getItem("token");
console.log(token);
var myHeaders = new Headers();
//myHeaders.append("Authorization", "Bearer " + JSON.parse(token));
myHeaders.append("Authorization", "Bearer " + token);
myHeaders.append("Content-Type", "application/json");

const getUsers = (token) => {
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => {
            _displayUsers(data);
            console.log("users:" + data);
        })
        .catch(error => console.error('Unable to get Users.', error));
}
getUsers(token);

const addUser = () => {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox=document.getElementById('add-password');
    const addTypeCheckBox=document.getElementById('add-isManager');
    const user = {
        id:-1,
        name:addNameTextbox.value.trim(),
        password:addPasswordTextbox.value.trim(),
        userType:addTypeCheckBox.checked==false?0:1
    };

    fetch(uri, {
            method: 'POST',
            headers: myHeaders,
            body: JSON.stringify(user)
        })
        //.then(response => response.json())
        .then(() => {
            getUsers(token);
            addNameTextbox.value = '';
            addTypeCheckBox.checked=false;
            addPasswordTextbox.value='';
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: myHeaders
        })
        .then(() => getUsers(token))
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(user => user.id === id);
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-isManager').checked = user.userType;
    document.getElementById('edit-password').value=user.password;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id').value;
    const user = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password:document.getElementById('edit-password').value.trim(),
        userType: document.getElementById('edit-isManager').checked==false?0:1,
        
    };

    fetch(`${uri}/${userId}`, {
            method: 'PUT',
            // headers: {
            //     'Accept': 'application/json',
            //     'Content-Type': 'application/json'
            // },
            headers: myHeaders,
            body: JSON.stringify(user)
        })
        .then(() => getUsers(token))
        .catch(error => console.error('Unable to update user.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(userCount) {
    const description = (userCount === 1) ? 'user' : 'users';

    document.getElementById('counter').innerText = `${userCount} ${description}`;
}

function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(user => {
        let isManagerCheckbox = document.createElement('input');
        isManagerCheckbox.type = 'checkbox';
        isManagerCheckbox.disabled = true;
        isManagerCheckbox.checked = user.userType;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isManagerCheckbox);

        let td2 = tr.insertCell(1);
        let textNodeName = document.createTextNode(user.name);
        td2.appendChild(textNodeName);

        let td3 = tr.insertCell(2);
        let textNodePassword = document.createTextNode(user.password);
        td3.appendChild(textNodePassword);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    users = data;
}