(function (moment, _) {

    angular
        .module('jogTracker')
        .controller('jogsController', jogsController);

    jogsController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory',
        'notificationUtils'];

    function jogsController($scope, $log, jogDataFactory, notificationUtils) {

        $scope.jogs = [];
        $scope.fromDate = moment().subtract(7, 'days'); //Default filter from last week.
        $scope.toDate = moment();
        $scope.pageIndex = 0;
        $scope.pageSize = 30;

        $scope.noData = function() {
            return $scope.jogs.length == 0;
        };

        $scope.$on('refresh', function() {
            _reloadData();
        });

        $scope.selectJog = function(jog) {
            _deselectOtherJogs(jog);
            jog.selected = true;
            //Open in 'edit' panel.
        };

        var _deselectOtherJogs = function(jog) {
            var otherJogs = _.filter(
                $scope.jogs,
                function(j) {
                    return j.id != jog.id;
                });

            _.each(
                otherJogs,
                function(j) {
                    j.selected = false;
                });
        };

        var _reloadData = function() {
            jogDataFactory.getList($scope.fromDate, $scope.toDate, $scope.pageIndex, $scope.pageSize)
                .then(function(data)
                {
                    $scope.jogs = data.Items;
                })
                .catch(function(err) {
                    notificationUtils.showErrorToast(err, 'Error listing jogs');
                });
        };

        _reloadData();
        $log.info('Jogs controller loaded.');
    }

}(moment, _));
