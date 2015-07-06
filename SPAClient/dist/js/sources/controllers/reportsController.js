(function (_, moment) {

    angular
        .module('jogTracker')
        .controller('reportsController', reportsController);

    reportsController.$inject = [
        '$scope',
        'jogDataFactory',
        'notificationUtils'];

    var momentDateFormat = 'YYYY/MM/DD';

    function reportsController($scope, jogDataFactory, notificationUtils) {

        $scope.jogCount = 0;
        $scope.distanceKm = 0;
        $scope.duration = 0;
        $scope.averageKmh = 0;
        $scope.currentDate = moment();
        $scope.weekStart = $scope.currentDate.startOf('week').add(1,'days').format(momentDateFormat);
        $scope.weekEnd = $scope.currentDate.endOf('week').add(1,'days').format(momentDateFormat);

        $scope.previousWeek = function() {
            var current = $scope.currentDate.add(-7, 'days');
            $scope.currentDate = current;
            $scope.weekStart = current.startOf('week').add(1,'days').format(momentDateFormat);
            $scope.weekEnd = current.add(1,'week').add(1,'days').format(momentDateFormat);
            _reloadData();

            console.log($scope.currentDate.format('YYYY/MM/DD') + ', starts: ' + $scope.weekStart + ', end: ' + $scope.weekEnd);
        };

        $scope.nextWeek = function() {
            var current = $scope.currentDate.add(1, 'weeks');
            $scope.currentDate = current;
            $scope.weekStart = current.startOf('week').add(1,'days').format(momentDateFormat);
            $scope.weekEnd = current.add(1,'week').add(1,'days').format(momentDateFormat);
            _reloadData();

            console.log($scope.currentDate.format('YYYY/MM/DD') + ', starts: ' + $scope.weekStart + ', end: ' + $scope.weekEnd);

        };

        function _calculateAggregate(data) {
            $scope.jogCount = data.length;
            var totalDistance = 0;
            var totalDuration = moment.duration(0);

            for(var i = 0; i < data.length; i++) {
                totalDistance += Number(data[i].distanceKm);
                totalDuration.add(data[i].duration);
            }

            $scope.distanceKm = totalDistance;
            $scope.duration = totalDuration;

            if (totalDuration.asHours() > 0) {
                $scope.averageKmh = totalDistance / totalDuration;
            }
            else {
                $scope.averageKmh = 0;
            }
        }

        var _reloadData = function() {
            ;
            jogDataFactory.getListForWeek($scope.currentDate)
                .then(function(data)
                {
                    _calculateAggregate(data);
                })
                .catch(function(err) {
                    notificationUtils.showErrorToast(err, 'Error listing jogs');
                });
        };


        _reloadData();
    }

}(_, moment));
