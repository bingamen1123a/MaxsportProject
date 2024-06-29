function sortTable(n) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("orders");
    switching = true;
    dir = "asc";
    while (switching) {
        switching = false;
        rows = table.rows;
        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchcount++;
        } else {
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}

function createPagination() {
    var table = document.getElementById("orders");
    var rows = table.rows.length;
    var pages = Math.ceil(rows / 5);
    var pagination = document.querySelector(".pagination");
    for (var i = 1; i <= pages; i++) {
        var link = document.createElement("a");
        link.href = "#";
        link.innerText = i;
        link.onclick = function () {
            var pageNum = parseInt(this.innerText);
            var start = (pageNum - 1) * 5 + 1;
            var end = pageNum * 5;
            for (var j = 1; j < start; j++) {
                table.rows[j].style.display = "none";
            }
            for (var j = end + 1; j < rows; j++) {
                table.rows[j].style.display = "none";
            }
            for (var j = start; j <= end && j < rows; j++) {
                table.rows[j].style.display = "";
            }
            var links = document.querySelectorAll(".pagination a");
            for (var j = 0; j < links.length; j++) {
                links[j].classList.remove("active");
            }
            this.classList.add("active");
        };
        pagination.appendChild(link);
    }
    pagination.firstChild.classList.add("active");
}

createPagination();
