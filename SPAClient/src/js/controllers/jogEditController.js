(function (moment) {

    angular
        .module('jogTracker')
        .controller('jogEditController', jogEditController);

    jogEditController.$inject = [
        '$scope',
        '$log',
        '$rootScope',
        '$stateParams',
        'jogDataFactory',
        'notificationUtils',
        'validatorUtils'];

    function jogEditController($scope, $log, $rootScope, $stateParams, jogDataFactory, notificationUtils, validatorUtils) {

        $scope.panelOpen = true; //Show 'add' panel by default.
        $scope.dtFormat = 'yyyy/MM/dd';
        $scope.date = moment().format('YYYY/MM/DD');

        $scope.$on('editJog', function(event, jog) {
            _editJog(jog);
        });

        var _reset = function() {
            $scope.jogId = null;
            $scope.title = 'Record Jog';
            $scope.buttonText = 'Create New';
            $scope.friendlyErrors = []; //validation errors.
            $scope.disableButton = false;
            $scope.opened = false;
            $scope.durationMinutes = 0;
            $scope.distanceKm = 0;
        };

        var _editJog = function(jog) {
            _reset();
            $scope.jogId = jog.id;
            $scope.panelOpen = true;
            $scope.title = 'Edit Jog';
            $scope.buttonText = 'Update';
            $scope.date = jog.date;
            $scope.durationMinutes = moment.duration(jog.duration).asMinutes();
            $scope.distanceKm = jog.distanceKm;
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


        function _createNewJog(durationString) {
            jogDataFactory.create($scope.date, $scope.distanceKm, durationString)
                .then(function (data) {
                    notificationUtils.showSuccess('Jog recorded.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
                    $scope.closePanel();
                })
                .catch(function (err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error recording jog');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        }

        function _updateJog(jogId, durationString) {
            jogDataFactory.update(jogId, $scope.date, $scope.distanceKm, durationString)
                .then(function (data) {
                    notificationUtils.showSuccess('Jog updated.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
                    $scope.closePanel();
                })
                .catch(function (err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error updating jog');
                    } else {
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        }

        $scope.save = function(validForm) {

            if (! validForm) {
                return;
            }

            _setDisabled(true);
            $scope.friendlyErrors = [];

            var duration = moment.duration(Number($scope.durationMinutes), 'minutes');
            var durationString = duration.hours() + ':' + duration.minutes() + ':' + duration.seconds();

            if ($scope.jogId == null) {
                _createNewJog(durationString);
            }
            else {
                _updateJog($scope.jogId, durationString);
            }
        };

        _reset();
    }

}(moment));
