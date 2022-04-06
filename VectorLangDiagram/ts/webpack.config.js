const { resolve } = require('path');
const TerserPlugin = require('terser-webpack-plugin');
const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = {
    entry: './src/index.ts',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.ttf$/,
                type: 'asset/resource',
                generator: {
                    filename: '../fonts/[name][ext][query]'
                },
            }
        ],
    },
    resolve: {
        extensions: ['.js', '.ts', '.css'],
    },
    optimization: {
        minimizer: [new TerserPlugin({ extractComments: false })],
    },
    plugins: [
        new MonacoWebpackPlugin({ languages: [] }),
    ],
    output: {
        filename: 'interop.js',
        library: 'interop',
        path: resolve(__dirname, '../wwwroot/bundle/js'),
        clean: true,
    },
};
