(function () {

    angular
        .module('jogTracker')
        .directive('jogEditPanel', function() {
            return {
                restrict: 'E',
                templateUrl: '/partials/jogEditPanel.html'
            };
        });

}());
