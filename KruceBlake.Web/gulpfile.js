`use strict`;

const autoprefixer = require(`gulp-autoprefixer`),
    distPath = `./wwwroot/dist`,
    sourcePath = `./wwwroot/source`,
    cssmin = require(`gulp-cssmin`),
    concat = require(`gulp-concat`),
    del = require(`del`),
    gulp = require(`gulp`),
    header = require(`gulp-header`),
    merge = require(`merge-stream`),
    plumber = require(`gulp-plumber`),
    sass = require(`gulp-sass`),
    uglify = require(`gulp-uglify-es`).default,
    pkg = require(`./package.json`), // Load package.json for banner
    banner = [`/*!\n`, // Set the banner content
        ` * kruceblake.com - <%= pkg.title %> v<%= pkg.version %> (<%= pkg.homepage %>)\n`,
        ` * Copyright 2020-` + (new Date()).getFullYear(), ` <%= pkg.author %>\n`,
        ` */\n`,
        `\n`
    ].join(``);

///minifies javascript
gulp.task(`min:js`, async function () {
    return gulp.src([`${sourcePath}/js/**/*.js`, `!${sourcePath}/js/**/*.min.js`])
        .pipe(concat(`site.min.js`, { newLine: `` }))
        .pipe(uglify())
        .pipe(gulp.dest(`${distPath}/js/`));
});

///compiles sass
gulp.task(`min:sass`, function () { //leaving this task synchronus for the series where we need this to finish before minifying css.
    return gulp
        .src(`${sourcePath}/scss/**/*.scss`)
        .pipe(plumber())
        .pipe(sass({
            outputStyle: `expanded`,
            includePaths: `./node_modules`,
        }))
        .on(`error`, sass.logError)
        .pipe(autoprefixer({
            cascade: false
        }))
        .pipe(header(banner, {
            pkg: pkg
        }))
        .pipe(gulp.dest(`${sourcePath}/css`));
});

///minifies javascript
gulp.task(`min:css`, async function () {
    return gulp.src([`${sourcePath}/css/**/*.css`, `!${sourcePath}/css/**/*.min.css`])
        .pipe(concat(`site.min.css`, { newLine: `` }))
        .pipe(cssmin())
        .pipe(gulp.dest(`${distPath}/css`));
});

///clears lib folder and redownloads all necessary modules
gulp.task(`modules`, async function () {
    // Bootstrap
    var bootstrap = gulp.src(`./node_modules/bootstrap/dist/**/*`).pipe(gulp.dest(`${distPath}/lib/bootstrap`));
    // Font Awesome CSS
    var fontAwesomeCSS = gulp.src(`./node_modules/@fortawesome/fontawesome-free/css/**/*`).pipe(gulp.dest(`${distPath}/lib/fontawesome-free/css`));
    // Font Awesome Webfonts
    var fontAwesomeWebfonts = gulp.src(`./node_modules/@fortawesome/fontawesome-free/webfonts/**/*`).pipe(gulp.dest(`${distPath}/lib/fontawesome-free/webfonts`));
    // jQuery Easing
    var jqueryEasing = gulp.src(`./node_modules/jquery.easing/*.js`).pipe(gulp.dest(`${distPath}/lib/jquery-easing`));
    // jQuery
    var jquery = gulp.src([`./node_modules/jquery/dist/*`, `!./node_modules/jquery/dist/core.js`]).pipe(gulp.dest(`${distPath}/lib/jquery`));
    // AOS
    var aos = gulp.src(`./node_modules/aos/dist/**/*`).pipe(gulp.dest(`${distPath}/lib/aos`));
    // jQuery validation unobtrusive
    var jqueryValidationUnobtrusive = gulp.src(`./node_modules/jquery-validation-unobtrusive/dist/**/*`).pipe(gulp.dest(`${distPath}/lib/jquery-validation-unobtrusive`));
    // jQuery validation
    var jqueryValidation = gulp.src([`./node_modules/jquery-validation/dist/*`, `!./node_modules/jquery-validation/dist/localization`]).pipe(gulp.dest(`${distPath}/lib/jquery-validation`));

    merge(del([`${distPath}/lib/`]), bootstrap, fontAwesomeCSS, fontAwesomeWebfonts, jqueryEasing, jquery, aos, jqueryValidationUnobtrusive, jqueryValidation);
});

///minifies sass, css and javascript
gulp.task(`min`, gulp.series([`min:js`, `min:sass`, `min:css`]));

///watches files and runs task if changes occur
gulp.task(`watch`, () => {
    gulp.watch([`${sourcePath}/js/**/*.js`, `!${sourcePath}/js/**/*.min.js`], gulp.series([`min:js`]));
    gulp.watch([`${sourcePath}/scss/**/*`, `${sourcePath}/css/**/*.css`, `!${sourcePath}/js/**/*.min.css`,], gulp.series([`min:sass`, `min:css`]));
});

//default task that installs modules and minifies files
gulp.task(`default`, gulp.series(`min`));