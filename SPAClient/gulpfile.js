var gulp       = require('gulp'),
    compass    = require('gulp-compass'),
    concat     = require('gulp-concat'),
    notify     = require('gulp-notify'),
    plumber    = require('gulp-plumber'),
    uglify     = require('gulp-uglify'),
    watch      = require('gulp-watch'),
    sourcemaps = require('gulp-sourcemaps');


var onError = function(err) {
    console.log(err);
};

// Lets us type "gulp" on the command line and run all of our tasks
gulp.task('default', ['copystatic', 'scripts', 'styles', 'watch']);


//Copies local source files, so that it can be found by the source maps.
gulp.task('copysources', function () {
    return gulp.src('./src/js/**/*.js')
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(gulp.dest('./dist/js/sources'));
});

gulp.task('localscripts', function () {
    return gulp.src('./src/js/**/*.js')
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourcemaps.init()) //Load existing source maps.
            .pipe(concat('local.min.js'))
            .pipe(uglify())
        .pipe(sourcemaps.write('./', {includeContent: false, sourceRoot: '/js/sources'}))
        .pipe(gulp.dest('./dist/js/'));
});

gulp.task('vendorscripts', function() {
    var vendorScripts = [
        './bower_components/angular/angular.min.js',
        './bower_components/angular-bootstrap/ui-bootstrap.min.js'
    ];

    return gulp.src(vendorScripts)
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourcemaps.init({loadMaps: true}))
            .pipe(concat('vendor.min.js'))
            .pipe(uglify())
        .pipe(sourcemaps.write('./', {includeContent: false, sourceRoot: '/js/sources'}))
        .pipe(gulp.dest('./dist/js/'));
});


gulp.task('scripts', ['vendorscripts', 'localscripts', 'copysources']);



gulp.task('copystatic', function() {
    return gulp.src('./src/static/**/*.{html,ico}')
        .pipe(gulp.dest('./dist'));
});

gulp.task('styles', function() {
    return gulp.src('./src/sass/*.scss')
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(compass({
            css: './src/css',
            sass: './src/sass'
        }))
        .pipe(gulp.dest('./dist/css'))
        .pipe(notify({ message: 'Styles task complete' }));
});


gulp.task('watch', function() {

    // Watch .scss files
    gulp.watch('./src/static/**/*.*', ['copystatic']);

    // Watch .js files
    gulp.watch('./src/sass/**/*.scss', ['styles']);

    // Watch image files
    gulp.watch('./src/js/**/*.js', ['scripts']);

});

