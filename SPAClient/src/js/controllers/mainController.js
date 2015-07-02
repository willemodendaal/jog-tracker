(function () {

    angular
        .module('jogTracker')
        .controller('mainController', mainController);

    mainController.$inject = ['$scope', '$log', 'jogDataFactory', 'accountFactory'];

    function mainController($scope, $log, jogDataFactory, accountFactory) {
        $scope.registerPlease = function () {
            var p = accountFactory.register($scope.email, $scope.firstName, $scope.lastName, $scope.password);

            p.then(function () {
                alert('Registration done, and happy.');
            })
                .catch(function () {
                    alert('something went wrong.');
                });
        };

        $log.info('Main controller loaded.');
    }

}());
