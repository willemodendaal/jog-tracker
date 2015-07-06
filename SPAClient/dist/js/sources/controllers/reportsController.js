(function (_, moment) {

    angular
        .module('jogTracker')
        .controller('reportsController', reportsController);

    reportsController.$inject = [
        '$scope',
        'jogDataFactory',
        'notificationUtils'];

    var momentDateFormat = 'dddd, DD MMM YYYY';

    function reportsController($scope, jogDataFactory, notificationUtils) {

        $scope.jogCount = 0;
        $scope.distanceKm = 0;
        $scope.duration = 0;
        $scope.averageKmh = 0;
        $scope.currentDate = moment();
        $scope.weekStart = moment($scope.currentDate).startOf('week').add(1, 'days').format(momentDateFormat);
        $scope.weekEnd = moment($scope.currentDate).endOf('week').add(1, 'days').format(momentDateFormat);

        $scope.previousWeek = function () {
            var current = moment($scope.currentDate).subtract(7, 'days');
            $scope.currentDate = current;
            $scope.weekStart = moment(current).startOf('week').add(1, 'days').format(momentDateFormat);
            $scope.weekEnd = moment(current).endOf('week').add(1, 'days').format(momentDateFormat);
            _reloadData();
        };

        $scope.nextWeek = function () {
            var current = moment($scope.currentDate).add(7, 'days');
            $scope.currentDate = current;
            $scope.weekStart = moment(current).startOf('week').add(1, 'days').format(momentDateFormat);
            $scope.weekEnd = moment(current).endOf('week').add(1, 'days').format(momentDateFormat);
            _reloadData();
        };

        function _calculateAggregate(data) {
            $scope.jogCount = data.length;
            var totalDistance = 0;
            var totalDuration = moment.duration(0);

            for (var i = 0; i < data.length; i++) {
                totalDistance += Number(data[i].distanceKm);
                totalDuration.add(data[i].duration);
            }

            $scope.distanceKm = totalDistance;
            $scope.duration = totalDuration.hours().pad(2) + ':' + totalDuration.minutes().pad(2) + ':' + totalDuration.seconds().pad(2);

            if (totalDuration.asHours() > 0) {
                $scope.averageKmh = totalDistance / totalDuration.asHours();
            }
            else {
                $scope.averageKmh = 0;
            }

        }

        var _reloadData = function () {
            $scope.jogCount = 'Loading...';
            jogDataFactory.getListForWeek($scope.currentDate)
                .then(function (data) {
                    _calculateAggregate(data);
                })
                .catch(function (err) {
                    notificationUtils.showErrorToast(err, 'Error listing jogs');
                });
        };


        _reloadData();
    }

}(_, moment));
