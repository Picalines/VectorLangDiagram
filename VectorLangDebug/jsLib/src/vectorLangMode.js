import * as CodeMirror from 'codemirror';

import 'codemirror/addon/mode/simple';

CodeMirror.defineSimpleMode("vectorLang", {
    start: [
        {
            regex: /(\d+(\.\d+)?|\.\d+)\w*/,
            token: "number"
        },
        {
            regex: /""|''|(['"]).*?[^\\]\1/,
            token: "string"
        },
        {
            regex: /#[a-fA-F\d]{6}/,
            token: "color"
        },
        {
            regex: /\b(def|external|val|plot)\b/,
            token: "keyword"
        },
        {
            regex: /\/\/.*/,
            token: "comment"
        },
        {
            regex: /[-+\/*=>!\.]+/,
            token: "operator"
        },
        {
            regex: /[\{\[\(]/,
            indent: true
        },
        {
            regex: /[\}\]\)]/,
            dedent: true
        },
        {
            regex: /\b[a-zA-Z_][a-zA-Z0-9_]*\b/,
            token: "variable-3",
        },
    ],
});
