$(document).on('click', '#prdouct-favori', function () {

    var productId = $(this).parent().parent().children().children().val();
    console.log(productId)
    $.ajax({
        type: "POST",
        url: '/WishList/AddProductToWishList',
        data: { productId: productId },
        success: function () {
            if ($("#favorite" + productId).hasClass("fa-regular fa-heart")) {
                $("#favorite" + productId).removeClass("fa-regular fa-heart");
                $("#favorite" + productId).addClass("fa-solid fa-heart");
            }
            else {
                $("#favorite" + productId).removeClass("fa-solid fa-heart");
                $("#favorite" + productId).addClass("fa-regular fa-heart");
            }
        },
    });

})

$(document).on('click', '#prdouct-compare', function () {

    var productId = $(this).parent().parent().children().children().val();

    $.ajax({
        type: "POST",
        url: '/Compare/AddProductToCompare',
        data: { productId: productId },
        success: function () {},
    });


})

$(document).on('click', '#add-product-to-cart', function () {

    var productId = $(this).parent().parent().children().children().val();
    console.log(productId)
    $.ajax({
        type: "POST",
        url: '/Basket/AddToBasket',
        data: { productId: productId },
        success: function () {           
        },
    });

})


$(document).on('change', '#product-quality-from-icon', function () {
    var productId = $(this).parent().children().val();
    var count = $(this).val();
   
    $.ajax({
        type: "POST",
        url: '/Basket/ChangeProductQuality',
        data: { productId: productId , count : count},
        success: function () {
        },
    });


});




