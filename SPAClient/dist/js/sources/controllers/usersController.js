(function (_) {

    angular
        .module('jogTracker')
        .controller('usersController', usersController);

    usersController.$inject = [
        '$scope',
        'userDataFactory',
        'notificationUtils',
        '$rootScope'];

    function usersController($scope, userDataFactory, notificationUtils, $rootScope) {

        $scope.users = [];
        $scope.pageNumber = 1;
        $scope.pageSize = 3;
        $scope.totalItems = 0;

        $scope.noData = function() {
            return $scope.users.length == 0;
        };

        $scope.$on('refresh', function() {
            _reloadData();
        });

        $scope.pageChanged = function() {
            _reloadData();
        };

        $scope.selectUser = function(user) {
            _selectOnlyUser(user.id);
            $rootScope.$broadcast('editUser', user);
        };

        var _selectOnlyUser = function(userId) {
            _.each(
                $scope.users,
                function(j) {
                    j.selected = j.id == userId;
                });
        };

        var _reloadData = function() {

            userDataFactory.getList($scope.pageNumber - 1, $scope.pageSize)
                .then(function(data)
                {
                    $scope.totalItems = data.TotalResults;
                    $scope.users = data.Items;
                })
                .catch(function(err) {
                    notificationUtils.showErrorToast(err, 'Error listing users');
                });
        };

        _reloadData();
    }

}(_));
