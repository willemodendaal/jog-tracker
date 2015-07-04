(function () {

    angular
        .module('jogTracker')
        .controller('mainController', mainController);

    mainController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'userInfo',
        'md5'];

    function mainController($scope, $log, $state, accountFactory, userInfo, md5) {

        $scope.userFirstName = userInfo.firstName;
        $scope.userImage = _getUserImage(userInfo.email);

        var _getUserImage = function(email) {
            var hash = md5.createHash(email || '');
            return 'http://www.gravatar.com/avatar/' + hash + '?s=20';
        };

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
