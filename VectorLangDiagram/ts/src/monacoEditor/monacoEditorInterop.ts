import * as monaco from 'monaco-editor';
import { DotNetObjectReference } from '../DotNetObjectReference';

interface InteropTextLocation {
    readonly line: number;
    readonly column: number;
}

interface InteropSelection {
    readonly start: InteropTextLocation;
    readonly end: InteropTextLocation;
}

export class MonacoEditorInterop {
    private readonly model: monaco.editor.ITextModel;

    private readonly markersOwner = "monaco-editor-interop";

    constructor(
        public readonly editor: monaco.editor.IStandaloneCodeEditor,
        private readonly dotNetRef: DotNetObjectReference,
    ) {
        this.model = this.editor.getModel()!;

        this.model.onDidChangeContent(async () => {
            await this.dotNetRef.invokeMethodAsync('OnDidChangeContent', this.model.getValue());
        });
    }

    public addError(selection: InteropSelection, message: string) {
        const markers = monaco.editor.getModelMarkers({ owner: this.markersOwner }) as monaco.editor.IMarkerData[];

        markers.push({
            startLineNumber: selection.start.line,
            startColumn: selection.start.column,
            endLineNumber: selection.end.line,
            endColumn: selection.end.column,

            severity: monaco.MarkerSeverity.Error,
            message,
        });

        monaco.editor.setModelMarkers(this.model, this.markersOwner, markers);
    }

    public clearErrors() {
        monaco.editor.setModelMarkers(this.model, this.markersOwner, []);
    }
}
