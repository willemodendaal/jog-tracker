(function () {

    angular
        .module('jogTracker')
        .directive('errorPanel', function() {
            return {
                restrict: 'E',
                templateUrl: '/partials/errorPanel.html'
            };
        });

}());
