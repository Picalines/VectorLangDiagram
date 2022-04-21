import * as monaco from 'monaco-editor';

export const vectorLangId = "vectorLang";

monaco.languages.register({ id: vectorLangId });

const keywords = ['def', 'const', 'external', 'val', 'if', 'else', 'true', 'false', 'and', 'or', 'not'];

const typeKeywords = ['number', 'boolean', 'vector', 'color', 'void'];

monaco.languages.setMonarchTokensProvider(vectorLangId, {
    keywords,

    typeKeywords,

    operators: ['!=', '=', '+', '-', '*', '/', '%', '->'],

    operatorSymbols: /[=>+\-*\/%]+/,

    tokenizer: {
        root: [
            // function
            [/(\b[a-z_][a-zA-Z0-9_]*)(\s*)(\()/, [
                {
                    cases: {
                        '@typeKeywords': 'type.identifier',
                        '@keywords': 'keyword',
                        '@default': 'entity.name.function',
                    }
                },
                {
                    token: 'white'
                },
                {
                    token: 'delimiter'
                }
            ]],

            // identifiers and keywords
            [/[a-zA-Z_][a-zA-Z0-9_]*/, {
                cases: {
                    '@typeKeywords': 'type.identifier',
                    '@keywords': 'keyword',
                    '@default': 'variable.parameter',
                },
            }],

            // whitespace
            { include: '@whitespace' },

            // delimiters and operators
            [/[{}()\[\]]/, '@brackets'],
            [/[<>](?!@operatorSymbols)/, '@brackets'],
            [/@operatorSymbols/, {
                cases: {
                    '@operators': 'operator',
                    '@default': ''
                }
            }],

            // numbers
            [/(\d+(\.\d+)?|\.\d+)(\w*)/, 'number.float'],

            // color
            [/#[a-fA-F\d]{6}/, 'constant.language'],

            // delimiter: after number because of .\d floats
            [/[;,.]/, 'delimiter'],
        ],

        comment: [
            [/\/\/.*$/, 'comment']
        ],

        whitespace: [
            [/[ \t\r\n]+/, 'white'],
            [/\/\/.*$/, 'comment'],
        ],
    },
});

monaco.languages.setLanguageConfiguration(vectorLangId, {
    comments: {
        lineComment: '//',
    },

    autoClosingPairs: [
        { open: '(', close: ')' },
        { open: '[', close: ']' },
        { open: '{', close: '}' },
    ],

    brackets: [
        ['(', ')'],
        ['[', ']'],
        ['{', '}'],
    ],
})

monaco.languages.registerCompletionItemProvider(vectorLangId, {
    provideCompletionItems: (model, position) => {
        const word = model.getWordUntilPosition(position);

        const range: monaco.IRange = {
            startLineNumber: position.lineNumber,
            endLineNumber: position.lineNumber,
            startColumn: word.startColumn,
            endColumn: word.endColumn
        };

        return {
            suggestions: [
                ...keywords.map(keyword => ({
                    label: keyword,
                    kind: monaco.languages.CompletionItemKind.Keyword,
                    insertText: keyword,
                    range,
                })),
                ...typeKeywords.map(type => ({
                    label: type,
                    kind: monaco.languages.CompletionItemKind.Class,
                    insertText: type,
                    range,
                })),
            ]
        }
    },
});
