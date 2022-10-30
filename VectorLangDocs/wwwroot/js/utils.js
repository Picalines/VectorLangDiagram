function BlazorScrollToElement(selector) {
    const element = document.querySelector(selector);
    if (!(element instanceof HTMLElement)) {
        return;
    }

    element.scrollIntoView();
}

function BlazorFocusOnElement(selector) {
    const element = document.querySelector(selector);
    if (!(element instanceof HTMLElement)) {
        return;
    }

    element.focus();
}

function BlazorAddTemporaryClass(selector, className, durationMs) {
    const element = document.querySelector(selector);
    if (!(element instanceof HTMLElement)) {
        return;
    }

    element.__temporaryClassTimeouts ??= {};

    element.classList.add(className);
    const timeout = setTimeout(() => {
        element.classList.remove(className);
        delete element.__temporaryClassTimeouts[className];
    }, durationMs);

    clearTimeout(element.__temporaryClassTimeouts[className])
    element.__temporaryClassTimeouts[className] = timeout;
}
