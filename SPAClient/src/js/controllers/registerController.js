(function () {

    angular
        .module('jogTracker')
        .controller('registerController', registerController);

    registerController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'userInfo'
    ];

    function registerController($scope, $log, $state, accountFactory, userInfo) {

        var _doLogin = function() {
            //Success. Do login and redirect.
            accountFactory.login($scope.email, $scope.password)
                .then(function (token) {
                    userInfo.access_token = token;
                    $state.go('main');
                })
                .catch(function (err) {
                    //Todo: show toast.
                    alert('Unable to login. Message: ' + err);
                });

            $state.go('main');
        };


        $scope.register = function () {
            accountFactory.register( $scope.email, $scope.firstName, $scope.lastName, $scope.password )
                .then(_doLogin)
                .catch(function(err) {

                    if (err.status == 500) {
                        //Show error in a toast.

                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                    }

                    //Todo: show toast.
                    alert('Unable to register. Message: ' + err);
                });
        };

        $log.info('Login controller loaded.');
    }

}());
