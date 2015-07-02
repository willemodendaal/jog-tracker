(function () {

    angular
        .module('jogTracker')
        .controller('mainController', mainController);

    mainController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory'];

    function mainController($scope, $log, $state, accountFactory) {
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
            $state.go('login');
        });


        $log.info('Main controller loaded.');
    }

}());
