import { PanZoom } from 'panzoom';

export class PanZoomInterop {
    constructor(
        public readonly panzoom: PanZoom
    ) { }

    public reset() {
        this.panzoom.moveTo(0, 0);
        this.panzoom.zoomAbs(0, 0, 1);
    }
}
