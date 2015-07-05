(function () {

    angular
        .module('jogTracker')
        .controller('accountController', accountController);

    accountController.$inject = [
        '$scope',
        'accountFactory',
        'notificationUtils'];

    function accountController($scope, accountFactory, notificationUtils) {
        $scope.email = 'willem.odendaal@gmail.com';
        $scope.firstName = 'willem';
        $scope.lastName = 'odendaal';
        $scope.editMode = false;
        $scope.edit = {firstName: $scope.firstName, lastName: $scope.lastName };

        $scope.editDetails = function() {
            $scope.editMode = true;
            $scope.edit.firstName = $scope.firstName;
            $scope.edit.lastName = $scope.lastName;
        };

        $scope.resetPassword = function() {
        };

        $scope.save = function(valid) {
            if (!valid) {
                return;
            }

            alert('save!');
        };

        $scope.cancel = function() {
            $scope.editMode = false;
        };
    }

}());
