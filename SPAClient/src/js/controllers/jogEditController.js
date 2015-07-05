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

        $scope.panelOpen = true; //Show 'add' panel by default.
        $scope.dtFormat = 'yyyy/MM/dd';
        $scope.date = moment().format('YYYY/MM/DD');


        var _reset = function() {
            $scope.title = 'Record Jog';
            $scope.buttonText = 'Create New';
            $scope.friendlyErrors = []; //validation errors.
            $scope.disableButton = false;
            $scope.opened = false;
            $scope.durationMinutes = 0;
            $scope.distanceKm = 0;
        };

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

        $scope.startAddNew = function() {
            _reset();
            $scope.panelOpen = true;
        };

        $scope.closePanel = function() {
            $scope.panelOpen = false;
            _reset();
        };

        $scope.toggleDatePicker = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = !$scope.opened;
        };


        $scope.save = function(validForm) {

            if (! validForm) {
                return;
            }

            _setDisabled(true);
            $scope.friendlyErrors = [];

            var duration = moment.duration(Number($scope.durationMinutes), 'minutes');
            var durationString = duration.hours() + ':' + duration.minutes() + ':' + duration.seconds();

            jogDataFactory.create($scope.date, $scope.distanceKm, durationString)
                .then(function(data)
                {
                    notificationUtils.showSuccess('Jog logged.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
                    $scope.closePanel();
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

        _reset();
        $log.info('JogEdit controller loaded.');
    }

}(moment));
