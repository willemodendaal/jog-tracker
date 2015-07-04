(function (moment) {

    angular
        .module('jogTracker')
        .controller('jogEditController', jogEditController);

    jogEditController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory',
        'notificationUtils',
        'validatorUtils'];

    function jogEditController($scope, $log, jogDataFactory, notificationUtils, validatorUtils) {

        $scope.title = 'New Jog';
        $scope.buttonText = 'Create New';
        $scope.friendlyErrors = []; //validation errors.
        $scope.disableButton = false;

        $scope.date = moment();
        $scope.durationMinutes = 1;
        $scope.distanceKm = 0;

        var _setDisabled = function(disabled) {
            if (disabled) {
                $scope.disableButton = true;
                $scope.buttonText = "Create new...";
            }
            else {
                $scope.disableButton = false;
                $scope.buttonText = "Create new";
            }
        };

        $scope.save = function() {
            _setDisabled(true);

            jogDataFactory.create($scope.date, $scope.distanceKm, $scope.durationMinutes)
                .then(function(data)
                {
                    notificationUtils.showSuccess('Jog logged.', 'Success');
                    $scope.$broadcast('refresh'); //Indicate that a refresh is required.
                })
                .catch(function(err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error saving jog');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        };

        $log.info('JogEdit controller loaded.');
    }

}(moment));
