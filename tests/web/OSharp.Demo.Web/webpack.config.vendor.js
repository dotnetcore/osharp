//这个文件是打包压缩静态第三方资源的，平时不大会更新，需要更新时要手动执行打包命令
// webpack --config webpack.config.vendor.js
var path = require("path");
var webpack = require("webpack");
var outputDir = "./wwwroot/dist";
var extractTextPlugin = require("extract-text-webpack-plugin");

module.exports = env => {
    var isDevBuild = !(env && env.prod);
    var extractCSS = new extractTextPlugin("vendor.css");

    return [{
        stats: { modules: false },
        resolve: { extensions: [".js"] },
        entry: {
            vendor: [
                "bootstrap",
                "bootstrap/dist/css/bootstrap.css",
                "event-source-polyfill",
                "isomorphic-fetch",
                "jquery",
                "vue",
                "vue-router"
            ]
        },
        output: {
            path: path.join(__dirname, outputDir),
            filename: "[name].js",
            library: "[name]_[hash]"
        },
        module: {
            rules: [
                { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? "css-loader" : "css-loader?minimize" }) },
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: "url-loader?limit=100000" }
            ]
        },
        plugins: [
            extractCSS,
            new webpack.ProvidePlugin({ $: "jquery", jQuery: "jquery" }),
            new webpack.DefinePlugin({ "process.env.NODE_ENV": isDevBuild ? '"development"' : '"production"' }),
            new webpack.DllPlugin({
                path: path.join(__dirname, outputDir, "[name]-manifest.json"),
                name: "[name]_[hash]"
            })
        ].concat(isDevBuild ? [] : [new webpack.optimize.UglifyJsPlugin()])
    }];
};