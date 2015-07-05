(function () {

    angular
        .module('jogTracker')
        .controller('resetPasswordController', resetPasswordController);

    resetPasswordController.$inject = [
        '$scope',
        'accountFactory',
        'notificationUtils'
    ];

    function resetPasswordController($scope, accountFactory, notificationUtils) {

        $scope.showSuccess = false;
        $scope.resetPassword = function () {
            $scope.showSuccess = true;

            accountFactory.requestResetPwd( $scope.email )
                .then(function() {
                    //All good.
                })
                .catch(function(err) {
                    $scope.showSuccess = false;
                    notificationUtils.showErrorToast(err, 'Reset Password Error');
                });
        };
    }

}());
