// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var modal = document.getElementById("bookModal");
var span = document.getElementsByClassName("close")[0];

// This method is run when you click on a bookcard, it fetches info about the book clicked by using the key and then finding the matching book with that key in the users session.
// The other solution would be to use the encoded info in the html but seemed like a bad idea.
function openModal(bookKey) {
    var xhr = new XMLHttpRequest();

    xhr.open('GET', '/Books/' + encodeURIComponent(bookKey), true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            var book = JSON.parse(xhr.responseText);
            console.log(book);
            document.getElementById("bookKey").textContent = book.key; 
            document.getElementById("bookImage").src = book.imageUrl;
            document.getElementById("bookTitle").innerHTML = `<h2>${book.title}</h2>`;
            document.getElementById("bookAuthor").innerHTML = `<p><strong>Author: </strong> ${book.author_name ? book.author_name.join(", ") : "None"}</p>`;
            document.getElementById("bookSubject").innerHTML = `<p><strong>Subjects: </strong>${book.subject ? book.subject.join(", ") : "None"}</p>`;
            document.getElementById("bookPublishYear").innerHTML = `<p><strong>Published: </strong> ${book.first_publish_year ? book.first_publish_year : "None"}</p>`;
            document.getElementById("bookPublisher").innerHTML = `<p><strong>Publisher: </strong> ${book.publisher ? book.publisher.join(", ") : "None"}</p>`;
            /*modalContent.innerHTML = `<h2>${book.title}</h2><p><strong>Author:</strong> ${book.author_name.join(", ")}</p>`; // Adjust according to your book object structure*/
            modal.style.display = "block";
        }
    };
    xhr.send();
}

// Fetch the book from the database and populate the modal with the info.
function openModalFromDatabase(bookKey) {
    var xhr = new XMLHttpRequest();

    xhr.open('GET', '/Books/DetailsFromDb/' + encodeURIComponent(bookKey), true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            var book = JSON.parse(xhr.responseText);

            document.getElementById("bookKey").textContent = book.key;
            document.getElementById("bookImage").src = book.imageUrl;
            document.getElementById("bookTitle").innerHTML = `<h2>${book.title}</h2>`;
            document.getElementById("bookAuthor").innerHTML = `<p><strong>Author: </strong> ${book.authorName ? book.authorName.split(',').join(', ') : "None"}</p>`;
            document.getElementById("bookSubject").innerHTML = `<p><strong>Subjects: </strong>${book.subject ? book.subject.split(',').join(', ') : "None"}</p>`;
            document.getElementById("bookPublishYear").innerHTML = `<p><strong>Published: </strong> ${book.firstPublishYear ? book.firstPublishYear : "None"}</p>`;
            document.getElementById("bookPublisher").innerHTML = `<p><strong>Publisher: </strong> ${book.publisher ? book.publisher.split(',').join(', ') : "None"}</p>`;
            document.getElementById("bookScore").value = book.score;
            document.getElementById("bookReview").value = book.review;

            modal.style.display = "block";
        }
    };
    xhr.send();
}

// This button saves the selected book to the database.
var modalButton = document.getElementById("modalButton");
if (modalButton) {
    modalButton.addEventListener("click", function () {
        var bookKey = document.getElementById("bookKey").textContent;
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Books/' + encodeURIComponent(bookKey), true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log("hello");
                var successMessage = document.getElementById("successMessage");
                successMessage.textContent = "Book added successfully!";

                successMessage.style.display = "block";
                successMessage.style.animationName = 'fadeIn';

                setTimeout(function () {
                    successMessage.style.animationName = 'fadeOut';

                    setTimeout(function () {
                        successMessage.style.display = "none";
                    }, 1000); 
                }, 1500);
            }
        };
        xhr.send();
    });
}

// This button updates the score and review of the book in the database.
var modalButtonSave = document.getElementById("modalButtonSave");
if (modalButtonSave) {
    modalButtonSave.addEventListener("click", function () {
        var bookKey = document.getElementById("bookKey").textContent;
        var score = document.getElementById("bookScore").value; 
        var review = document.getElementById("bookReview").value;

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Books/UpdateBook', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log("Update successful");
                var successMessage = document.getElementById("successMessage");
                successMessage.textContent = "Book updated successfully!";

                successMessage.style.display = "block";
                successMessage.style.animationName = 'fadeIn';

                setTimeout(function () {
                    successMessage.style.animationName = 'fadeOut';


                    setTimeout(function () {
                        successMessage.style.display = "none";
                    }, 1000); 
                }, 1500);
            } else {
                console.log("Update failed");
                var failureMessage = document.getElementById("failureMessage");
                failureMessage.textContent = "Book not updated successfully.";


                failureMessage.style.display = "block";
                failureMessage.style.animationName = 'fadeIn';


                setTimeout(function () {
                    failureMessage.style.animationName = 'fadeOut';


                    setTimeout(function () {
                        failureMessage.style.display = "none";
                    }, 1000); 
                }, 1500);
            }
        };

        var payload = JSON.stringify({ key: bookKey, score: score, review: review });
        xhr.send(payload);
    });
}

// This button deletes the selected book from the database.
var modalButtonDelete = document.getElementById("modalButtonDelete");
if (modalButtonDelete) {
    modalButtonDelete.addEventListener("click", function () {
        var bookKey = document.getElementById("bookKey").textContent;

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Books/DeleteBook', true); 
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log("Book successfully deleted");
                modal.style.display = "none";
                location.reload()
            } else {
                console.log("Failed to delete book");
            }
        };

        var payload = JSON.stringify({ key: bookKey });
        xhr.send(payload);
    });
}

// onclick functions to close the modal when pressed outside on the X-button or outside the modal.
span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}