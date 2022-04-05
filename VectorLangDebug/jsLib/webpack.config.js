const { resolve } = require("path");

module.exports = {
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    output: {
        path: resolve(__dirname, '../wwwroot/js'),
        filename: "jsLib.bundle.js",
        library: "JsLib",
    }
};
