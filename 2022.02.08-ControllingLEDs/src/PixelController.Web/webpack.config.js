// lib
const autoprefixer = require('autoprefixer');
const path = require('path');
const webpack = require('webpack');

// plugins
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
//const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = (env, argv) => {
    const isProd = argv.mode === 'production';

    // paths
    const distPath = path.resolve(__dirname, './wwwroot');
    const imgDistPath = path.resolve(distPath, './img');
    const srcPath = '.';
    const templatePath = path.resolve(__dirname);

    return {
        devtool: isProd ? 'source-map' : 'eval-cheap-module-source-map',
        entry: {
            main: './scripts/main.js'
        },
        output: {
            filename: 'js/[name].[hash].js',
            path: distPath,
            publicPath: '/'
        },

        module: {
            rules: [
                {
                    test: /\.css$/,
                    use: [
                        { loader: MiniCssExtractPlugin.loader },
                        { loader: 'css-loader' },
                        { loader: 'postcss-loader', options: { postcssOptions: { ident: 'postcss', plugins: () => {autoprefixer()} } } }
                    ]
                },
                {
                    test: /\.(png|jpg|gif|svg)$/,
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]?[hash]'
                    }
                }
            ]
        },
        plugins: [
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: ['**/*', '!lib/**']
            }),

            // copy src images to wwwroot
            // new CopyWebpackPlugin({
            //     patterns: [
            //         // {from: 'favicon', to: distPath},
            //         // {from: 'icons', to: iconDistPath},
            //         { from: 'images', to: imgDistPath, context: srcPath }
            //     ]
            // }),

            new MiniCssExtractPlugin({
                filename: `./css/[name].[hash].css`
            }),

            new HtmlWebpackPlugin({
                filename: path.resolve(__dirname, './wwwroot/index.html'),
                inject: false,
                minify: false,
                template: path.resolve(__dirname, './index.html')
            })
        ],
        resolve: {
            extensions: ['.ts', '.js', '.json'],
            modules: [
                path.resolve(__dirname, './node_modules'),
                srcPath
            ]
        }
    };
};