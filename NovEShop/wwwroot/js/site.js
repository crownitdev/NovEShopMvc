// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("div.offcanvas-search form.offcanvas-search-form").bind('keypress', function (e) {
    if (e.key == "Enter") {
        $("div.offcanvas-search form.offcanvas-search-form").submit();
    }
});
