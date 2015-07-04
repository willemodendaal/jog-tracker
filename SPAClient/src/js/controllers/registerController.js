(function () {

    angular
        .module('jogTracker')
        .controller('registerController', registerController);

    registerController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'userInfo',
        'validatorUtils',
        'notificationUtils'
    ];

    function registerController($scope, $log, $state, accountFactory, userInfo, validatorUtils, notificationUtils) {

        $scope.friendlyErrors = []; //Registration error message (i.e. invalid password)
        $scope.disableButton = false;
        $scope.buttonText = "Register";

        var _setDisabled = function(disabled) {
            if (disabled) {
                $scope.disableButton = true;
                $scope.buttonText = "Registering...";
            }
            else {
                $scope.disableButton = false;
                $scope.buttonText = "Register";
            }
        };

        var _doLogin = function() {
            //Success. Do login and redirect.
            _setDisabled(true);
            accountFactory.login($scope.email, $scope.password)
                .then(function (token) {
                    userInfo.access_token = token;
                    $state.go('main');
                })
                .catch(function (err) {
                    notificationUtils.showErrorToast(err, 'Login Error');
                    _setDisabled(false);
                });
        };

        $scope.register = function () {
            _setDisabled(true);
            $scope.friendlyErrors = [];

            accountFactory.register( $scope.email, $scope.firstName, $scope.lastName, $scope.password )
                .then(_doLogin)
                .catch(function(err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Registration Error');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        };

        $log.info('Login controller loaded.');
    }

}());
