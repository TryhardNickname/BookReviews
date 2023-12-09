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
            var modalContent = document.getElementById("modalBookDetails");
            modalContent.innerHTML = `<h2>${book.title}</h2><p><strong>Author:</strong> ${book.author_name.join(", ")}</p>`; // Adjust according to your book object structure
            modal.style.display = "block";
        }
    };
    xhr.send();
}

//function openModal(bookKey) {
//    // Make an AJAX call to fetch book details
//    // For example, using fetch API
//    fetch('/BooksController/GetBookDetails?key=' + bookKey)
//        .then(response => response.json())
//        .then(data => {
//            // Assuming data has book details
//            document.getElementById('modalTitle').innerText = data.title;
//            // Populate other modal elements here
//            document.getElementById('bookModal').style.display = "block";
//        });
//}

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