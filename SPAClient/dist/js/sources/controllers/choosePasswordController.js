(function () {

    angular
        .module('jogTracker')
        .controller('choosePasswordController', choosePasswordController);

    choosePasswordController.$inject = [
        '$scope',
        '$log',
        'accountFactory',
        'notificationUtils',
        '$stateParams',
        'validatorUtils'
    ];

    function choosePasswordController($scope, $log, accountFactory, notificationUtils, $stateParams, validatorUtils) {

        $scope.friendlyErrors = [];
        $scope.showSuccess = false;
        $scope.userId = $stateParams.uid;
        $scope.token = $stateParams.token;
        $scope.disableButton = false;

        $log.info('Reset page loaded with userId ['+ $scope.userId + '] and token [' + $scope.token + ']');

        $scope.resetPassword = function () {
            $scope.friendlyErrors = [];
            $scope.disableButton = true;

            accountFactory.resetPwd( $scope.userId, $scope.newPassword, $scope.token )
                .then(function() {
                    $scope.showSuccess = true;
                    $scope.disableButton = false;
                })
                .catch(function(err) {
                    $scope.disableButton = false;
                    $scope.showSuccess = false;
                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Reset password error');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        };
    }

}());
