import panzoom from 'panzoom';
import { PanZoomInterop } from './panzoomInterop';

export function createPanZoom(elementSelector: string) {
    const domElement = document.querySelector(elementSelector) as HTMLElement;

    const panzoomInstance = panzoom(domElement, {
        minZoom: 0.1,
        maxZoom: 10,
    });

    return new PanZoomInterop(panzoomInstance);
}
