var path = require("path");
var webpack = require("webpack");
var outputDir = "./wwwroot/dist";
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var CheckerPlugin = require("awesome-typescript-loader").CheckerPlugin;

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);

    return [{
        stats: { modules: false },
        context: __dirname,
        resolve: { extensions: [".js", ".ts", ".vue"] },
        entry: { build: "./ClientApp/boot.ts" },
        output: {
            path: path.join(__dirname, outputDir),
            filename: "[name].js",
            publicPath: "dist/"
        },
        module: {
            rules: [
                { test: /\.vue\.html$/, include: /ClientApp/, loader: "vue-loader", options: { loaders: { js: "awesome-typescript-loader?silent=true" } } },
                { test: /\.vue$/, include: /ClientApp/, loader: "vue-loader", options: { loaders: {} } },
                { test: /\.ts$/, include: /ClientApp/, use: "awesome-typescript-loader?silent=true" },
                { test: /\.css$/, include: /ClientApp/, use: isDevBuild ? ["style-loader", "css-loader"] : extractTextPlugin.extract({ use: "css-loader?minimize" }) },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, include: /ClientApp/, use: "url-loader?limit=25000" }
            ]
        },
        plugins: [
            new CheckerPlugin(),
            new webpack.DefinePlugin({
                'process.env': {
                    NODE_ENV: JSON.stringify(isDevBuild ? "development" : "production")
                }
            }),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require("./wwwroot/dist/vendor-manifest.json")
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: "[file].map", // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(outputDir, "[resourcePath]") // Point sourcemap entries to the original file locations on disk
            })
        ] : [
                // Plugins that apply in production builds only
                new webpack.optimize.UglifyJsPlugin(),
                new ExtractTextPlugin("site.css")
            ])
    }];
};
