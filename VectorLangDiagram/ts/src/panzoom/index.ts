import panzoom from 'panzoom';

export function createPanZoom(elementSelector: string) {
    const domElement = document.querySelector(elementSelector) as HTMLElement;

    return panzoom(domElement, {
        minZoom: 1,
        maxZoom: 10,
    });
}
