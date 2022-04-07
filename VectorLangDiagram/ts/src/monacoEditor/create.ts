import * as monaco from 'monaco-editor';
import { DotNetObjectReference } from '../DotNetObjectReference';
import { MonacoEditorInterop } from './monacoEditorInterop';
import { vectorLangId } from './vectorLang';

export function createMonacoEditor(dotNetRef: DotNetObjectReference, containterSelector: string, initialCode: string = '') {
    const container = document.querySelector(containterSelector) as HTMLElement;

    const editor = monaco.editor.create(container, {
        value: initialCode,

        language: vectorLangId,

        fontSize: 22,

        automaticLayout: true,
        lineNumbers: 'on',
        theme: 'monokai',

        selectionHighlight: true,

        minimap: {
            enabled: false,
        },
    });

    return new MonacoEditorInterop(editor, dotNetRef);
}
