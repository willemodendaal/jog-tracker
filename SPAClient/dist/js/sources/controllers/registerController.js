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
        'toastr'
    ];

    function registerController($scope, $log, $state, accountFactory, userInfo, toastr) {

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
                    _toastError(err, 'Login Error');
                    _setDisabled(false);
                });
        };

        var _toastError = function(err, title) {
            //Show error in a toast.
            if (err.data.Message) {
                toastr.error(err.data.Message, title);
            }
            else {
                toastr.error(err.statusText, title);
            }
        };

        var _getValidationErrors = function(err) {
            if (!err.data || !err.data.ModelState) {
                return 'Request contained invalid data.';
            }

            var messages = [];
            for (var field in err.data.ModelState) {
                    if (err.data.ModelState.hasOwnProperty(field)) {

                        var array = err.data.ModelState[field];
                        for(var i = 0; i < array.length; i++) {
                            messages.push(array[i]);
                        }
                }
            }

            return messages;
        };

        $scope.register = function () {
            _setDisabled(true);

            accountFactory.register( $scope.email, $scope.firstName, $scope.lastName, $scope.password )
                .then(_doLogin)
                .catch(function(err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        _toastError(err, 'Registration Error');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = _getValidationErrors(err);
                    }
                });
        };

        $log.info('Login controller loaded.');
    }

}());
