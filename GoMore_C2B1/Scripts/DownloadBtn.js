$(document).ready(function () {
    $('.expand-collapse h4').each(function () {
        var tis = $(this), state = false, answer = tis.next('div').slideUp();
        tis.click(function () {
            state = !state;
            answer.slideToggle(state);
            tis.toggleClass('active', state);
        });
    });
});
if (document.getElementById("Permission_").innerHTML == "0") {
    document.getElementById("DownloadBtn").style.display = "none";
} else {
    document.getElementById("DownloadBtn").style.display = "block";
}