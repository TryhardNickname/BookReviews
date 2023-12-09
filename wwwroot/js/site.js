// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var modal = document.getElementById("bookModal");
var span = document.getElementsByClassName("close")[0];

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

document.getElementById("modalButton").addEventListener("click", function () {
    var bookKey = document.getElementById("bookKey").textContent;                                                         // get the book key
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Books/' + encodeURIComponent(bookKey), true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            console.log("hello");
            // Handle success (change button text, show message, etc.)  IMPLEMENT SOME SUCCESSTHINGY
        }
    };
    xhr.send();
});

// Close the modal when the user clicks on <span> (x)
span.onclick = function () {
    modal.style.display = "none";
}

// Close the modal when the user clicks anywhere outside of the modal
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}