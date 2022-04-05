import * as CodeMirror from 'codemirror';

import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/material.css';

import './vectorLangMode'

const errorStyle = `
    text-decoration: red wavy underline;
    background-color: #ba060644;
    box-shadow: #ba060622 0px 0px 10px;
`;

export function initializeCodeMirror(textAreaSelector, initialProgram = "") {
    const textarea = document.querySelector(textAreaSelector);

    const editor = CodeMirror.fromTextArea(textarea, {
        value: initialProgram,
        lineNumbers: true,
        theme: "material",
        mode: "vectorLang",
    });

    editor.on('change', () => {
        textarea.innerHTML = editor.getValue();
        textarea.dispatchEvent(new Event('change', { bubbles: true }));
    });

    editor.focus();

    return editor;
}

export function markError(editor, selection) {
    const { start, end } = selection;

    const from = { line: start.line - 1, ch: start.column - 1 };
    const to = { line: end.line - 1, ch: end.column - 1 };

    editor.getDoc().markText(from, to, { css: errorStyle });
}

export function clearErrors(editor) {
    editor.doc.getAllMarks().forEach(marker => marker.clear());
}
