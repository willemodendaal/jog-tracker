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
        $scope.pageNumber = 1;
        $scope.pageSize = 3;
        $scope.totalItems = 0;

        $scope.noData = function() {
            return $scope.jogs.length == 0;
        };

        $scope.$on('refresh', function() {
            _reloadData();
        });

        $scope.pageChanged = function() {
            _reloadData();
        };

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
            jogDataFactory.getList($scope.fromDate, $scope.toDate, $scope.pageNumber - 1, $scope.pageSize)
                .then(function(data)
                {
                    $scope.totalItems = data.TotalResults;
                    $scope.pageNumber = data.PageIndex + 1;
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
