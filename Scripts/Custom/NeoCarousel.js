var slidingTimer;
var sliderIndex = 0;
$(document).ready(function () {
    $(".sliderHolder .slide").width($("body").width());
    $(".sliderHolder").width($("body").width() * 3);

    slidingTimer = setTimeout("nextSlide()", 5000);

    $(".sliderBullets span").click(function () {
        clearTimeout(slidingTimer);

        console.log($(".sliderBullets span").index(this));

        sliderIndex = $(".sliderBullets span").index(this);

        $(".sliderHolder").animate({
            left: "-" + sliderIndex * $(".sliderHolder .slide").eq(0).width() + "px"
        }, 700);

        slidingTimer = setTimeout("nextSlide()", 5000);
    });
});
function nextSlide() {
    var slides = $(".sliderHolder .slide");

    sliderIndex++;
    if (sliderIndex == 3)
        sliderIndex = 0;

    $(".sliderHolder").animate({
        left: "-" + sliderIndex * $(".sliderHolder .slide").eq(0).width() + "px"
    }, 700);

    slidingTimer = setTimeout("nextSlide()", 5000);
}

$(window).resize(function () {
    clearTimeout(slidingTimer);

    $(".sliderHolder .slide").width($("body").width());
    $(".sliderHolder").width($("body").width() * 3);
    $(".sliderHolder").css("left", "-" + sliderIndex * $(".sliderHolder .slide").eq(0).width() + "px");

    slidingTimer = setTimeout("nextSlide()", 5000);
});