import * as monaco from 'monaco-editor';

export interface InteropTextLocation {
    line: number;
    column: number;
}

export interface InteropTextSelection {
    start: InteropTextLocation;
    end: InteropTextLocation;
}

export const interopReportSeverityMap = {
    0: monaco.MarkerSeverity.Info,
    1: monaco.MarkerSeverity.Warning,
    2: monaco.MarkerSeverity.Error,
};

export type InteropReportSeverity = keyof typeof interopReportSeverityMap;

export interface InteropReport {
    selection: InteropTextSelection;
    severity: InteropReportSeverity;
    message: string;
}

export const interopCompletionKindMap = {
    0: monaco.languages.CompletionItemKind.Keyword,
    1: monaco.languages.CompletionItemKind.Class,
    2: monaco.languages.CompletionItemKind.Field,
    3: monaco.languages.CompletionItemKind.Method,
    4: monaco.languages.CompletionItemKind.Function,
    5: monaco.languages.CompletionItemKind.Variable,
    6: monaco.languages.CompletionItemKind.Constant,
};

export type InteropCompletionKind = keyof typeof interopCompletionKindMap;

export interface InteropCompletion {
    kind: InteropCompletionKind;
    label: string;
    detail: string | null;
    value: string;
    scope: InteropTextSelection;
}
