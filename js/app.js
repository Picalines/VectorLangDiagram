// is-resizing class on body element
(() => {

    const isResizingClass = 'is-resizing';
    const isResizingClassRemoveDelay = 100;

    let resizeTimer;

    window.addEventListener('resize', () => {
        document.body.classList.add(isResizingClass);

        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            document.body.classList.remove(isResizingClass);
        }, isResizingClassRemoveDelay)
    });

})();
