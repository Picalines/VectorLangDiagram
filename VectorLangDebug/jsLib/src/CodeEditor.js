import * as CodeMirror from 'codemirror';

import './vectorLangMode';

const errorStyle = `
    text-decoration: red wavy underline;
    background-color: #ba060644;
    box-shadow: #ba060622 0px 0px 10px;
`;

export class CodeEditor {
    #editor

    constructor(textareaSelector, initialProgram = '') {
        const textarea = document.querySelector(textareaSelector);

        this.#editor = CodeMirror.fromTextArea(textarea, {
            value: initialProgram,
            lineNumbers: true,
            theme: "material",
            mode: "vectorLang",
        });

        this.#editor.on('change', () => {
            textarea.innerHTML = this.#editor.getValue();
            textarea.dispatchEvent(new Event('change', { bubbles: true }));
        });
    }

    focus() {
        this.#editor.focus();
    }

    markError(selection) {
        const { start, end } = selection;

        const from = { line: start.line - 1, ch: start.column - 1 };
        const to = { line: end.line - 1, ch: end.column - 1 };

        this.#editor.getDoc().markText(from, to, { css: errorStyle });
    }

    clearErrors() {
        this.#editor.doc.getAllMarks().forEach(marker => marker.clear());
    }
}
