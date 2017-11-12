'use strict'
const path = require('path')
const glob = require('glob');
const config = require('../config')
const ExtractTextPlugin = require('extract-text-webpack-plugin')
const HtmlWebpackPlugin = require('html-webpack-plugin')

exports.assetsPath = function (_path) {
  const assetsSubDirectory = process.env.NODE_ENV === 'production'
    ? config.build.assetsSubDirectory
    : config.dev.assetsSubDirectory
  return path.posix.join(assetsSubDirectory, _path)
};

exports.cssLoaders = function (options) {
  options = options || {}

  const cssLoader = {
    loader: 'css-loader',
    options: {
      minimize: process.env.NODE_ENV === 'production',
      sourceMap: options.sourceMap
    }
  }

  // generate loader string to be used with extract text plugin
  function generateLoaders(loader, loaderOptions) {
    const loaders = [cssLoader]
    if (loader) {
      loaders.push({
        loader: loader + '-loader',
        options: Object.assign({}, loaderOptions, {
          sourceMap: options.sourceMap
        })
      })
    }

    // Extract CSS when that option is specified
    // (which is the case during production build)
    if (options.extract) {
      return ExtractTextPlugin.extract({
        use: loaders,
        fallback: 'vue-style-loader'
      })
    } else {
      return ['vue-style-loader'].concat(loaders)
    }
  }

  const defaultTheme = `../node_modules/muse-ui/src/styles/themes/variables/default.less`;
  const themePath = path.resolve(__dirname, defaultTheme);

  // https://vue-loader.vuejs.org/en/configurations/extract-css.html
  return {
    css: generateLoaders(),
    postcss: generateLoaders(),
    less: generateLoaders('less', {
      globalVars: {
        museUiTheme: `'${themePath}'`
      }
    }),
    sass: generateLoaders('sass', { indentedSyntax: true }),
    scss: generateLoaders('sass'),
    stylus: generateLoaders('stylus'),
    styl: generateLoaders('stylus')
  }
};

// Generate loaders for standalone style files (outside of .vue)
exports.styleLoaders = function (options) {
  const output = []
  const loaders = exports.cssLoaders(options)
  for (const extension in loaders) {
    const loader = loaders[extension]
    output.push({
      test: new RegExp('\\.' + extension + '$'),
      use: loader
    })
  }
  return output
};

exports.getEntries = function (globPath) {
  var entries = {};
  glob.sync(globPath).forEach(entry => {
    var strs = entry.split('/').splice(-3);
    var key = strs[1];
    entries[key] = entry;
  });
  // console.log("base-entries:");
  // console.log(entries);
  return entries;
};

exports.getHtmlPlugins = function (globPath, isdev) {
  var plugins = [];
  glob.sync(globPath).forEach(item => {
    var strs = item.split('/').splice(-3);
    var key = strs[1]; //name of module
    var plugin = new HtmlWebpackPlugin({
      filename: key == 'app' ? 'index.html' : key + '/index.html',
      template: item,
      inject: true,
      chunks: [key, "vendor", "manifest"],
      minify: isdev ? false : {
        removeComments: true,
        collapseWhitespace: true,
        removeAttributeQuotes: true
      }
    });
    plugins.push(plugin);
  });
  // console.log(plugins);
  // console.log(plugins[0].options.chunks);
  return plugins;
};
