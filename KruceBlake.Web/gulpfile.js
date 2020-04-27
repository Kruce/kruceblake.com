`use strict`;

const autoprefixer = require(`gulp-autoprefixer`),
    cssmin = require('gulp-cssmin'),
    concat = require(`gulp-concat`),
    del = require(`del`),
    gulp = require(`gulp`),
    header = require(`gulp-header`),
    merge = require(`merge-stream`),
    plumber = require(`gulp-plumber`),
    rename = require(`gulp-rename`),
    sass = require(`gulp-sass`),
    uglify = require(`gulp-uglify-es`).default,
    pkg = require(`./package.json`), // Load package.json for banner
    banner = [`/*!\n`, // Set the banner content
        ` * kruceblake.com - <%= pkg.title %> v<%= pkg.version %> (<%= pkg.homepage %>)\n`,
        ` * Copyright 2020-` + (new Date()).getFullYear(), ` <%= pkg.author %>\n`,
        ` */\n`,
        `\n`
    ].join(``),
    regex = { //regex for identifying file types
        css: /\.css$/,
        html: /\.(html|htm)$/,
        js: /\.js$/,
        sass: /\.scss$/
    },
    bundleconfig = require(`./bundleconfig.json`),
    getBundles = (regexPattern) => { //gets all bundles by given regex
        return bundleconfig.filter(bundle => {
            return regexPattern.test(bundle.outputFileName);
        });
    };

///minifies javascript
gulp.task('min:js', async function () {
    merge(getBundles(regex.js).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName, { newLine: '' }))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    }))
});

///compiles sass
gulp.task(`min:sass`, async function () {
    return gulp
        .src(`./wwwroot/scss/**/*.scss`)
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
        .pipe(gulp.dest(`./wwwroot/css`))
        .pipe(rename({
            suffix: `.min`
        }))
        .pipe(cssmin())
        .pipe(gulp.dest(`./wwwroot/css`));
});

///minifies css
gulp.task(`min:css`, async function () {
    merge(getBundles(regex.css).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: `.` })
            .pipe(concat(bundle.outputFileName, { newLine: '' }))
            .pipe(cssmin({keepSpecialComments: 1})) //keepSpecialComments changes in updates
            .pipe(gulp.dest(`.`));
    }))
});

///minifies sass, css and javascript
gulp.task(`min`, gulp.series([`min:js`, `min:sass`, `min:css`]));

///Deletes lib folder then redownloads all modules
gulp.task(`modules`, async function () {
    // Bootstrap
    var bootstrap = gulp.src(`./node_modules/bootstrap/dist/**/*`)
        .pipe(gulp.dest(`./wwwroot/lib/bootstrap`));
    // Font Awesome CSS
    var fontAwesomeCSS = gulp.src(`./node_modules/@fortawesome/fontawesome-free/css/**/*`)
        .pipe(gulp.dest(`./wwwroot/lib/fontawesome-free/css`));
    // Font Awesome Webfonts
    var fontAwesomeWebfonts = gulp.src(`./node_modules/@fortawesome/fontawesome-free/webfonts/**/*`)
        .pipe(gulp.dest(`./wwwroot/lib/fontawesome-free/webfonts`));
    // jQuery Easing
    var jqueryEasing = gulp.src(`./node_modules/jquery.easing/*.js`)
        .pipe(gulp.dest(`./wwwroot/lib/jquery-easing`));
    // jQuery
    var jquery = gulp.src([
        `./node_modules/jquery/dist/*`,
        `!./node_modules/jquery/dist/core.js`
    ]).pipe(gulp.dest(`./wwwroot/lib/jquery`));
    // AOS
    var aos = gulp.src(`./node_modules/aos/dist/**/*`)
        .pipe(gulp.dest(`./wwwroot/lib/aos`));
    // jQuery validation unobtrusive
    var jqueryValidationUnobtrusive = gulp.src(`./node_modules/jquery-validation-unobtrusive/dist/**/*`)
        .pipe(gulp.dest(`./wwwroot/lib/jquery-validation-unobtrusive`));
    // jQuery validation
    var jqueryValidation = gulp.src([
        `./node_modules/jquery-validation/dist/*`,
        `!./node_modules/jquery-validation/dist/localization`
    ]).pipe(gulp.dest(`./wwwroot/lib/jquery-validation`));
    merge(del([`./wwwroot/lib/`]), bootstrap, fontAwesomeCSS, fontAwesomeWebfonts, jqueryEasing, jquery, aos, jqueryValidationUnobtrusive, jqueryValidation);
})

///watches files and runs task if changes occur
gulp.task('watch', () => {
    getBundles(regex.js).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(["min:js"])));
    gulp.watch(`./wwwroot/scss/**/*`, gulp.series([`min:sass`, `min:css`]));
    getBundles(regex.css).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(["min:css"])));
});

///cleans any task files
gulp.task('clean', () => {
    return del(bundleconfig.map(bundle => bundle.outputFileName));
});

gulp.task('default', gulp.series("min"));