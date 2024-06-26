const uri = '/Tasks';
let tasks = [];
let token = localStorage.getItem("token");
console.log(token);
var myHeaders = new Headers();
//myHeaders.append("Authorization", "Bearer " + JSON.parse(token));
myHeaders.append("Authorization", "Bearer " + token);
myHeaders.append("Content-Type", "application/json");


const authorizateUserPage=()=>{
    linkToUsersPage=document.getElementById("linkToUsersPage");
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(uri+"/Current", requestOptions)
        .then(response => response.json())
        .then(user => {
            if(user.userType==0){
                linkToUsersPage.display=false;
            }
        })
        .catch(error => 
            {console.error('Unable to get User.', error);
            alert("you first have to login in order to enter this page");
            window.location.href="login.html";
        });
}
authorizateUserPage();
const getItems = (token) => {
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => {
            _displayItems(data);
            console.log("tasks:" + data);
        })
        .catch(error => console.error('Unable to get items.', error));
}
getItems(token);

const addItem = () => {
    const addNameTextbox = document.getElementById('add-description');

    const item = {
        isDone: false,
        description: addNameTextbox.value.trim(),
    };

    fetch(uri, {
            method: 'POST',
            headers: myHeaders,
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: myHeaders
        })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);

    document.getElementById('edit-description').value = item.description;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        description: document.getElementById('edit-description').value.trim(),
        ownerId: -1
    };

    fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            // headers: {
            //     'Accept': 'application/json',
            //     'Content-Type': 'application/json'
            // },
            headers: myHeaders,
            body: JSON.stringify(item)
        })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const description = (itemCount === 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${itemCount} ${description}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.description);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}