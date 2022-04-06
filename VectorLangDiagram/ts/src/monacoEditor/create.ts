import * as monaco from 'monaco-editor';
import { vectorLangId } from './vectorLang';

export function createMonacoEditor(containterSelector: string) {
    const container = document.querySelector(containterSelector) as HTMLElement;

    monaco.editor.create(container, {
        language: vectorLangId,

        automaticLayout: true,
        lineNumbers: 'on',
        theme: 'monokai',

        selectionHighlight: true,

        minimap: {
            enabled: false,
        },
    });
}
