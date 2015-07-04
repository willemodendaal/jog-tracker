(function () {

    angular
        .module('jogTracker')
        .controller('loginController', loginController);

    loginController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'userInfo'
       ];

    function loginController($scope, $log, $state, accountFactory, userInfo) {
        $scope.login = function () {
            var p = accountFactory.login( $scope.email, $scope.password )
                .then(function(token) {
                    userInfo.access_token = token;
                    $state.go('main');
                })
                .catch(function(err) {
                    //Todo: show toast.
                    alert('Unable to login. Message: ' + err);
                });
        };

        $log.info('Login controller loaded.');
    }

}());
