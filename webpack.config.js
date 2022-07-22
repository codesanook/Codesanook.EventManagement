// This webpack is only for build Sass to CSS
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const RemoveEmptyScriptsPlugin = require('webpack-remove-empty-scripts');

const mainWebProjectName = 'Topf.Cms'; // Change this value to your main web project name
const outputFileName = 'codesanook-event-management';

module.exports = {
  entry: {
    [`../../${mainWebProjectName}/wwwroot/scripts/${outputFileName}`]: './src/main',
    [`wwwroot/styles/${outputFileName}`]: './src/scss/style',
  },
  mode: 'development',
  output: {
    path: __dirname, // Output dir must be absolute path
    filename: '[name].js',
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', '.jsx', '.scss']
  },
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/,
        use: [
          'babel-loader',
        ],
        exclude: /node_modules/
      },
      {
        test: /\.scss$/,
        use: [
          // Extract to CSS file
          MiniCssExtractPlugin.loader,
          // Translates CSS to CommonJS and ignore solving URL of images
          'css-loader?url=false',
          // Compiles Sass to CSS
          'sass-loader',
        ],
        exclude: /node_modules/,
      },
    ]// End rules
  },
  plugins: [
    new RemoveEmptyScriptsPlugin({ verbose: true }),
    new MiniCssExtractPlugin({
      // Configure the output of CSS.
      // It is relative to output dir, only relative path work, absolute path does not work.
      filename: "[name].css",
    }),
  ],
  externals: {
    'react': 'React',
    'react-dom': 'ReactDOM'
  },
  // https://webpack.js.org/configuration/devtool/
  devtool: 'inline-source-map',
};