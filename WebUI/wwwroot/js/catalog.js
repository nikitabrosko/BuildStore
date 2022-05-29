let catalog = document.getElementById("catalog");

catalog.addEventListener("mouseover",
    function() {
        catalog.classList.toggle("show");
    });

catalog.addEventListener("mouseleave",
    function () {
        if (catalog.classList.contains("show")) {
            catalog.classList.remove("show");
        }
});