window.BlazorUtils = {

    focusOnElement: (selector) => {
        const element = document.querySelector(selector);
        if (!(element instanceof HTMLElement)) {
            return;
        }

        element.focus();
    },

    addEventListener: (objectEvalString, event, dotNetObject, dotNetMethod, once = false) => {
        const object = eval(objectEvalString);

        object.addEventListener(event, () => dotNetObject.invokeMethodAsync(dotNetMethod), { once });
    },

}
