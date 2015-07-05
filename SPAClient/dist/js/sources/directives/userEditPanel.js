(function () {

    angular
        .module('jogTracker')
        .directive('userEditPanel', function() {
            return {
                restrict: 'E',
                templateUrl: '/partials/userEditPanel.html'
            };
        });

}());
