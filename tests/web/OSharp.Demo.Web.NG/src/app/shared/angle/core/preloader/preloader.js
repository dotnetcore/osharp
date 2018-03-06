(function(global) {

    var counter = 0, timeout;
    var preloader = document.querySelector('.preloader');
    var progressBar = document.querySelector('.preloader-progress-bar');
    var body = document.querySelector('body');

    // if preloader not present => abort
    if (!preloader) return;

    // disables scrollbar
    body.style.overflow = 'hidden';

    timeout = setTimeout(startCounter, 20);

    // main.ts call this function once the app is boostrapped
    global.appBootstrap = function() {
        setTimeout(endCounter, 1000);
    };

    function startCounter() {
        var remaining = 100 - counter;
        counter = counter + (0.015 * Math.pow(1 - Math.sqrt(remaining), 2));

        if (progressBar) progressBar.style.width = Math.round(counter) + '%';

        timeout = setTimeout(startCounter, 20);
    }

    function endCounter() {

        clearTimeout(timeout);

        if (progressBar) progressBar.style.width = '100%';

        setTimeout(function() {
            // animate preloader hiding
            removePreloader();
            // retore scrollbar
            body.style.overflow = '';
        }, 300);
    }

    function removePreloader() {
        preloader.addEventListener('transitionend', function() {
            preloader.className = 'preloader-hidden';
        });
        preloader.className += ' preloader-hidden-add preloader-hidden-add-active';
    };

})(window);
