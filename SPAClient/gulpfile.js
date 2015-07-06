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
gulp.task('default', ['copystatic', 'scripts', 'styles', 'watch', 'vendorcss', 'vendorfonts']);


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
        .pipe(sourcemaps.init())
            .pipe(concat('app.min.js'))
            //.pipe(uglify())
        .pipe(sourcemaps.write('./', {includeContent: false, sourceRoot: '/js/sources'}))
        .pipe(gulp.dest('./dist/js/'));
});

gulp.task('vendorcss', function() {

    var vendorCss = [
        './bower_components/bootstrap/dist/css/bootstrap.min.css',
        './bower_components/angular-toastr/dist/angular-toastr.min.css'
        ];

    return gulp.src(vendorCss)
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(concat('vendorstyles.css'))
        .pipe(gulp.dest('./dist/css/'));
});

gulp.task('vendorscripts', function() {
    var vendorScripts = [
        './bower_components/angular/angular.min.js',
        './bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
        './bower_components/angular-ui-router/release/angular-ui-router.min.js',
        './bower_components/moment/min/moment.min.js',
        './bower_components/angular-toastr/dist/angular-toastr.min.js',
        './bower_components/angular-toastr/dist/angular-toastr.tpls.min.js',
        './bower_components/angular-animate/angular-animate.min.js',
        './bower_components/angular-md5/angular-md5.min.js',
        './bower_components/underscore/underscore-min.js'
    ];

    return gulp.src(vendorScripts)
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourcemaps.init({loadMaps: true}))
            .pipe(concat('vendor.js'))
        .pipe(sourcemaps.write('./', {includeContent: false, sourceRoot: '/js/sources'}))
        .pipe(gulp.dest('./dist/js/'));
});

gulp.task('vendorfonts', function() {
    return gulp.src('./bower_components/bootstrap/dist/fonts/**/*')
        .pipe(gulp.dest('./dist/fonts/'));
});

gulp.task('scripts', ['vendorscripts', 'localscripts', 'copysources']);

gulp.task('copystatic', function() {
    var staticSources = [
        './src/**/*.{html,ico,txt,xml}',
        './src/partials/**/*.html'
    ];

    return gulp.src(staticSources)
        .pipe(gulp.dest('./dist'));
});

gulp.task('styles', function() {
    var styleSources = [
        './src/sass/*.{scss,css}'
    ];

    return gulp.src(styleSources)
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(compass({
            css: './src/css',
            sass: './src/sass'
        }))
        .pipe(concat('styles.css'))
        .pipe(gulp.dest('./dist/css'))
        .pipe(notify({ message: 'Styles task complete' }));
});


gulp.task('watch', function() {

    gulp.watch('./src/**/*.{html,xml,txt,ico}', ['copystatic']);
    gulp.watch('./src/sass/**/*.scss', ['styles']);
    gulp.watch('./src/js/**/*.js', ['scripts']);

});

