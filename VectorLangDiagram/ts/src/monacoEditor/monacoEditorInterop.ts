import * as monaco from 'monaco-editor';
import { DotNetObjectReference } from '../DotNetObjectReference';
import { InteropCompletion, InteropReport, interopCompletionKindMap, interopReportSeverityMap } from './interopTypes';

const markersOwner = "monaco-editor-interop";

export class MonacoEditorInterop {
    private static _instance?: MonacoEditorInterop = undefined;

    public readonly model: monaco.editor.ITextModel;

    constructor(
        public readonly editor: monaco.editor.IStandaloneCodeEditor,
        private readonly _dotNetRef: DotNetObjectReference,
    ) {
        this.model = this.editor.getModel()!;

        this.model.onDidChangeContent(async () => {
            await this._dotNetRef.invokeMethodAsync('OnDidChangeContent', this.model.getValue());
        });

        MonacoEditorInterop._instance = this;
    }

    public static get instance() {
        return MonacoEditorInterop._instance;
    }

    public addReport({ selection, message, severity }: InteropReport) {
        const markers = monaco.editor.getModelMarkers({ owner: markersOwner }) as monaco.editor.IMarkerData[];

        markers.push({
            startLineNumber: selection.start.line,
            startColumn: selection.start.column,
            endLineNumber: selection.end.line,
            endColumn: selection.end.column,

            severity: interopReportSeverityMap[severity],
            message,
        });

        monaco.editor.setModelMarkers(this.model, markersOwner, markers);
    }

    public clearReports() {
        monaco.editor.setModelMarkers(this.model, markersOwner, []);
    }

    public async fetchCompletions(): Promise<Omit<monaco.languages.CompletionItem, 'range'>[]> {
        const cursorPosition = this.editor.getPosition()!;

        const completions = await this._dotNetRef.invokeMethodAsync("FetchCompletions", cursorPosition.lineNumber, cursorPosition.column) as InteropCompletion[];

        return completions.map(info => ({
            label: info.label,
            kind: interopCompletionKindMap[info.kind],
            detail: info.detail ?? undefined,
            insertText: info.value,
        }));
    }
}
