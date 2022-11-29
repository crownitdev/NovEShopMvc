$("div.offcanvas-search form.offcanvas-search-form").bind('keypress', function (e) {
    if (e.key == "Enter") {
        $("div.offcanvas-search form.offcanvas-search-form").submit();
    }
});


$(document).ready(function () {
    $("div.shop-top-bar-right div.shop-short-by form select.nice-select")
        .on("change", function () {
            var pageSizeValueOption = $(this).val();
            alert(pageSizeValueOption)
        });
});
