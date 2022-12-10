export function CopyString(text) {
    navigator.clipboard.writeText(text).catch(console.error);
}

export function CopyInnerTextOf(elementSelector) {
    const element = document.querySelector(elementSelector);
    if (element) {
        CopyString(element.innerText);
    }
}
