"use strict";

// Load plugins
const autoprefixer = require("gulp-autoprefixer");
const cleanCSS = require("gulp-clean-css");
const del = require("del");
const gulp = require("gulp");
const header = require("gulp-header");
const merge = require("merge-stream");
const plumber = require("gulp-plumber");
const rename = require("gulp-rename");
const sass = require("gulp-sass");
const uglify = require("gulp-uglify");

// Load package.json for banner
const pkg = require('./package.json');

// Set the banner content
const banner = ['/*!\n',
    ' * Start Bootstrap - <%= pkg.title %> v<%= pkg.version %> (<%= pkg.homepage %>)\n',
    ' * Copyright 2013-' + (new Date()).getFullYear(), ' <%= pkg.author %>\n',
    ' */\n',
    '\n'
].join('');

// Clean lib
function clean() {
    return del(["./wwwroot/lib/"]);
}

// Bring third party dependencies from node_modules into lib directory
function modules() {
    // Bootstrap
    var bootstrap = gulp.src('./node_modules/bootstrap/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/bootstrap'));
    // Font Awesome CSS
    var fontAwesomeCSS = gulp.src('./node_modules/@fortawesome/fontawesome-free/css/**/*')
        .pipe(gulp.dest('./wwwroot/lib/fontawesome-free/css'));
    // Font Awesome Webfonts
    var fontAwesomeWebfonts = gulp.src('./node_modules/@fortawesome/fontawesome-free/webfonts/**/*')
        .pipe(gulp.dest('./wwwroot/lib/fontawesome-free/webfonts'));
    // jQuery Easing
    var jqueryEasing = gulp.src('./node_modules/jquery.easing/*.js')
        .pipe(gulp.dest('./wwwroot/lib/jquery-easing'));
    // jQuery
    var jquery = gulp.src([
        './node_modules/jquery/dist/*',
        '!./node_modules/jquery/dist/core.js'
    ]).pipe(gulp.dest('./wwwroot/lib/jquery'));
    // AOS
    var aos = gulp.src('./node_modules/aos/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/aos'));
    // jQuery validation unobtrusive
    var jqueryValidationUnobtrusive = gulp.src('./node_modules/jquery-validation-unobtrusive/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/jquery-validation-unobtrusive'));
    // jQuery validation
    var jqueryValidation = gulp.src([
        './node_modules/jquery-validation/dist/*',
        '!./node_modules/jquery-validation/dist/localization'
    ]).pipe(gulp.dest('./wwwroot/lib/jquery-validation'));
    return merge(bootstrap, fontAwesomeCSS, fontAwesomeWebfonts, jqueryEasing, jquery, aos, jqueryValidationUnobtrusive, jqueryValidation);
}

// CSS task
function css() {
    return gulp
        .src("./wwwroot/scss/**/*.scss")
        .pipe(plumber())
        .pipe(sass({
            outputStyle: "expanded",
            includePaths: "./node_modules",
        }))
        .on("error", sass.logError)
        .pipe(autoprefixer({
            cascade: false
        }))
        .pipe(header(banner, {
            pkg: pkg
        }))
        .pipe(gulp.dest("./wwwroot/css"))
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(cleanCSS())
        .pipe(gulp.dest("./wwwroot/css"));
}

// JS task
function js() {
    return gulp
        .src([
            './wwwroot/js/*.js',
            '!./wwwroot/js/*.min.js'
        ])
        .pipe(uglify())
        .pipe(header(banner, {
            pkg: pkg
        }))
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('./wwwroot/js'));
}

// Watch files
function watchFiles() {
    gulp.watch("./wwwroot/scss/**/*", css);
    gulp.watch(["./wwwroot/js/**/*", "!./wwwroot/js/**/*.min.js"], js);
}

// Define complex tasks
const lib = gulp.series(clean, modules);
const build = gulp.series(lib, gulp.parallel(css, js));
const watch = gulp.series(build, gulp.parallel(watchFiles));

// Export tasks
exports.css = css;
exports.js = js;
exports.clean = clean;
exports.lib = lib;
exports.build = build;
exports.watch = watch;
exports.default = build;
