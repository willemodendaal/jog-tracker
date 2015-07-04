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
        $scope.dtFormat = 'yyyy/MM/dd';

        $scope.date = moment();
        $scope.durationMinutes = 20;
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

        $scope.openDatePicker = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };


        $scope.save = function() {
            _setDisabled(true);
            $scope.friendlyErrors = [];

            var duration = moment.duration($scope.durationMinutes, 'minutes');
            var durationString = duration.hours() + ':' + duration.minutes() + ':' + duration.seconds();

            jogDataFactory.create($scope.date, $scope.distanceKm, durationString)
                .then(function(data)
                {
                    notificationUtils.showSuccess('Jog logged.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
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
