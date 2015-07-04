(function () {

    angular
        .module('jogTracker')
        .controller('loginController', loginController);

    loginController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'userInfo',
        'notificationUtils',
        'validatorUtils'
       ];

    function loginController($scope, $log, $state, accountFactory, userInfo, notificationUtils, validatorUtils) {

        $scope.disableButton = false;
        $scope.buttonText = "Login";
        $scope.friendlyErrors = [];

        var _setDisabled = function(disabled) {
            if (disabled) {
                $scope.disableButton = true;
                $scope.buttonText = "Login...";
            }
            else {
                $scope.disableButton = false;
                $scope.buttonText = "Login";
            }
        };

        $scope.login = function () {
            _setDisabled(true);

            accountFactory.login( $scope.email, $scope.password )
                .then(function(token) {
                    userInfo.access_token = token;
                    $state.go('main');
                })
                .catch(function(err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Login Error');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        };

        $log.info('Login controller loaded.');
    }

}());
