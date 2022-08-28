$(document).ready(function () {
    let productImageGroups = [];
    $('.img-fluid').each(function () {
        let productImageSource = $(this).attr('src');
        let productImageTag = $(this).attr('tag');
        let productImageTitle = $(this).attr('title');
        if (productImageTitle) {
            productImageTitle = 'title="' + productImageTitle + '" ';
        } else {
            productImageTitle = '';
        }
        $(this).
        wrap('<a class="boxedThumb ' + productImageTag + '" ' +
            productImageTitle + 'href="' + productImageSource + '"></a>');
        productImageGroups.push('.' + productImageTag);
    });
    jQuery.unique(productImageGroups);
    productImageGroups.forEach(productImageGroupsSet);

    function productImageGroupsSet(value) {
        $(value).simpleLightbox();
    }
})