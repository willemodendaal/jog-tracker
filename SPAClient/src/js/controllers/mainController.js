(function () {

    angular
        .module('jogTracker')
        .controller('mainController', mainController);

    mainController.$inject = [
        '$scope',
        '$log',
        '$location',
        'accountFactory'];

    function mainController($scope, $log, $location, accountFactory) {
        $scope.registerPlease = function () {
            var p = accountFactory.register($scope.email, $scope.firstName, $scope.lastName, $scope.password);

            p.then(function () {
                    alert('Registration done, and happy.');
                })
                .catch(function () {
                    alert('something went wrong.');
                });
        };

        $scope.$on('must-login', function() {
            $location.path('/login');
        });


        $log.info('Main controller loaded.');
    }

}());
