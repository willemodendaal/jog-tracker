(function () {

    angular
        .module('jogTracker')
        .controller('accountController', accountController);

    accountController.$inject = [
        '$scope',
        'accountFactory',
        'notificationUtils',
        '$state'];

    function accountController($scope, accountFactory, notificationUtils, $state) {
        $scope.email = 'willem.odendaal@gmail.com';
        $scope.firstName = 'willem';
        $scope.lastName = 'odendaal';
        $scope.editMode = false;
        $scope.buttonText = 'Save';
        $scope.disableButton = false;
        $scope.loading = true;
        $scope.edit = {firstName: $scope.firstName, lastName: $scope.lastName };

        $scope.editDetails = function() {
            $scope.editMode = true;
            $scope.edit.firstName = $scope.firstName;
            $scope.edit.lastName = $scope.lastName;
        };

        var _fetchUserInfo = function() {
            accountFactory.getUserInfo()
                .then(function(userInfo) {
                    $scope.email = userInfo.data.email;
                    $scope.firstName = userInfo.data.firstName;
                    $scope.lastName = userInfo.data.lastName;
                    $scope.loading = false;

                })
                .catch(function(err) {
                    notificationUtils.showErrorToast(err, 'Error fetching info');
                    $scope.loading = false;
                });
        };

        $scope.resetPassword = function() {
            $state.go('resetpassword');
        };

        var _setDisabled = function (disabled) {
            if (disabled) {
                $scope.disableButton = true;
                $scope.buttonText = "Save...";
            }
            else {
                $scope.disableButton = false;
                $scope.buttonText = "Save";
            }
        };

        $scope.logout = function() {
            sessionStorage.access_token = null;
            $state.go('login');
        };

        $scope.save = function(valid) {
            if (!valid) {
                return;
            }

            accountFactory.update($scope.edit.firstName, $scope.edit.lastName)
                .then(function () {
                    notificationUtils.showSuccess('Details updated.', 'Success');
                    $scope.cancel();
                    $scope.firstName = $scope.edit.firstName;
                    $scope.lastName = $scope.edit.lastName;
                })
                .catch(function (err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error saving details');
                    } else {
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        };

        $scope.cancel = function() {
            $scope.editMode = false;
        };

        _fetchUserInfo();
    }

}());
