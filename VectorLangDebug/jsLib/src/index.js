import { CodeEditor } from './CodeEditor';

import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/material.css';

export function initializeCodeEditor(textAreaSelector, initialProgram = "") {
    const editor = new CodeEditor(textAreaSelector, initialProgram);

    editor.focus();

    return editor;
}
