(function () {

    angular
        .module('jogTracker')
        .controller('mainController', mainController);

    mainController.$inject = [
        '$scope',
        '$log',
        '$state',
        'accountFactory',
        'md5',
        'notificationUtils'];

    function mainController($scope, $log, $state, accountFactory, md5, notificationUtils) {

        $scope.userFirstName = 'User';
        $scope.userImage = '';
        $scope.isAdmin = false;

        $scope.getUserImage = function(email, size) {
            var hash = md5.createHash(email || '');
            return 'http://www.gravatar.com/avatar/' + hash + '?s=' + size + '&d=identicon';
        };

        var _fetchUserInfo = function() {
            accountFactory.getUserInfo()
                .then(function(userInfo) {
                    $scope.userFirstName = userInfo.data.firstName;
                    $scope.userImage = $scope.getUserImage(userInfo.data.email, 20);
                    $scope.isAdmin = userInfo.data.isAdmin;
                })
                .catch(function(err) {
                    notificationUtils.showErrorToast(err, 'Error fetching info');
                });
        };

        $scope.registerPlease = function () {
            var p = accountFactory.register($scope.email, $scope.firstName, $scope.lastName, $scope.password);

            p.then(function () {
                    alert('Registration done, and happy.');
                })
                .catch(function () {
                    alert('something went wrong.');
                });
        };

        $scope.$on('must-login', function() {
            $state.go('login');
        });

        _fetchUserInfo();
        $log.info('Main controller loaded.');
    }

}());
