const saveFlashButton = document.querySelector('#btnPress');
const questionInput = document.querySelector('#title');
const answerInput = document.querySelector('#description');
const flashContainer = document.querySelector('#Notes_container');
const deleteButton = document.querySelector('#btnPress2');
const showButton = document.querySelector('#btnPress3');

function clearField() {
    questionInput.value = '';
    answerInput.value = '';
    deleteButton.classList.add('hidden');
    showButton.classList.add('hidden');
    
}

function displayNoteInform(note) {
    questionInput.value = note.question;
    answerInput.value = note.answer; 
    deleteButton.classList.remove('hidden');
    showButton.classList.remove('hidden');
    deleteButton.setAttribute('data-id', note.id);
    saveFlashButton.setAttribute('data-id', note.id);
    showButton.setAttribute('data-id',note.id);
}

function fetchNoteById(id) {
    fetch(`https://localhost:7284/api/FlashNote/${id}`) //keyword to connect to API and other languages
        .then(response => response.json())
        .then(data => displayNoteInform(data)) 
        .catch(error => console.error('Error fetching note by ID:', error));
}

function populateForm(id) {
    fetchNoteById(id);
}

function addFlashNote(question, answer) {
    const body = {
        question: question,
        answer: answer,
        IsVisible: true,
        correctAnswer: true,
        incorrectAnswer: false,
    };

    fetch('https://localhost:7284/api/FlashNote', { //Remember, keyword to connect to API and other languages
        method: 'POST', //define what you want to do in the CRUD element
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log('Flash note added:', data);
            clearField();
            getFlashNotes(); 
        })
        .catch(error => console.error('Error adding flash note:', error));
}

function getFlashNotes() {
    fetch('https://localhost:7284/api/FlashNote') //Remember, keyword to connect to API and other languages
        .then(response => response.json())
        .then(data => displayNotes(data))
        .catch(error => console.error('Error fetching flash notes:', error));
}

function displayNotes(flashNotes) {
    flashContainer.innerHTML = '';
    flashNotes.forEach(note => {
        const flashCardElement = `
            <div class="Flash_note" data-id="${note.id}"> 
                <h3>${note.question}</h3>
                <p class="answer hidden">${note.answer} hidden</p>
            </div>
        `;
        flashContainer.innerHTML += flashCardElement;
    });

    document.querySelectorAll('.Flash_note').forEach(note => {
        note.addEventListener('click', function() {
            populateForm(note.dataset.id);
        });
    });
}

getFlashNotes();

function updateNote(id, question, answer){
    const body = {
        question: question,
        answer: answer,
        IsVisible: true,
        correctAnswer: true,
        incorrectAnswer: false,
    };

    fetch(`https://localhost:7284/api/FlashNote/${id}`, {
        method: 'PUT', //!! reminder!! PUT element, for the CRUD. Along with the fetch on top. 
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log('Note updated:', data);
            clearField();
            getFlashNotes(); 
        })
}

saveFlashButton.addEventListener('click', function() {
    const id = saveFlashButton.dataset.id;
    if(id){
        updateNote(id, questionInput.value, answerInput.value);
    } else{
        addFlashNote(questionInput.value, answerInput.value);
    }
   
    
});

function deleteFlashCard(id){
    fetch(`https://localhost:7284/api/FlashNote/${id}`, {
        method: 'DELETE', //need that so http request know it wants a delete request, reminder. Also, fetch
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            clearField();
            getFlashNotes();
        })
}

deleteButton.addEventListener('click', function() {
    const id = deleteButton.dataset.id;
    deleteFlashCard(id);
});

showButton.addEventListener('click', function(){
    const id = showButton.dataset.id;
    if(id){
        const flashNoteElement = document.querySelector(`.Flash_note[data-id="${id}"]`);
        if(flashNoteElement){
            const answerParagraph = flashNoteElement.querySelector('.answer');
            if(answerParagraph){
                answerParagraph.classList.toggle('hidden');
            }
        }

    }
})