import * as CodeMirror from 'codemirror';

import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/material.css';

import './vectorLangMode'

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
}
