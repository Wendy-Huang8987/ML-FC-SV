// Upload Pagination
document.addEventListener("DOMContentLoaded", function () {
    const table = document.getElementById("upload-table");
    const rowsPerPage = 2; // Adjust this value based on the number of rows you want per page

    // Calculate the total number of pages
    const totalRows = table.tBodies[0].rows.length;
    const totalPages = Math.ceil(totalRows / rowsPerPage);

    // Initialize current page
    let currentPage = 1;

    function showPage(page) {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        // Hide all rows
        for (let i = 0; i < totalRows; i++) {
            table.tBodies[0].rows[i].style.display = "none";
        }

        // Show the rows for the current page
        for (let i = start; i < Math.min(end, totalRows); i++) {
            table.tBodies[0].rows[i].style.display = "";
        }
    }

    function updatePagination() {
        const paginationContainer = document.getElementById("Uploadpagination");
        paginationContainer.innerHTML = ""; // Clear existing pagination links

        // Previous button
        const previousButton = document.createElement("li");
        previousButton.className = "page-item";
        const previousLink = document.createElement("a");
        previousLink.className = "page-link";
        previousLink.href = "#";
        previousLink.textContent = "Previous";
        previousLink.addEventListener("click", function () {
            if (currentPage > 1) {
                currentPage--;
                showPage(currentPage);
            }
        });
        previousButton.appendChild(previousLink);
        paginationContainer.appendChild(previousButton);

        // Page buttons
        for (let i = 1; i <= totalPages; i++) {
            const pageButton = document.createElement("li");
            pageButton.className = "page-item";
            const pageLink = document.createElement("a");
            pageLink.className = "page-link";
            pageLink.href = "#";
            pageLink.textContent = i;
            pageLink.addEventListener("click", function () {
                currentPage = i;
                showPage(currentPage);
            });
            pageButton.appendChild(pageLink);
            paginationContainer.appendChild(pageButton);
        }

        // Next button
        const nextButton = document.createElement("li");
        nextButton.className = "page-item";
        const nextLink = document.createElement("a");
        nextLink.className = "page-link";
        nextLink.href = "#";
        nextLink.textContent = "Next";
        nextLink.addEventListener("click", function () {
            if (currentPage < totalPages) {
                currentPage++;
                showPage(currentPage);
            }
        });
        nextButton.appendChild(nextLink);
        paginationContainer.appendChild(nextButton);

        // Show the initial page
        showPage(currentPage);
    }

    // Initial setup
    updatePagination();
});