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
        $scope.pageNumber = 1;
        $scope.pageSize = 3;
        $scope.totalItems = 0;
        $scope.dtPickers = {
            from: { opened: false, date: moment().subtract(1, 'months').format($scope.dtFormat)},
            to: { opened: false, date: moment().format($scope.dtFormat)}
        };
        $scope.dtFormat = 'yyyy/MM/dd';

        $scope.noData = function() {
            return $scope.jogs.length == 0;
        };

        $scope.$on('refresh', function() {
            _reloadData();
        });

        $scope.pageChanged = function() {
            _reloadData();
        };

        $scope.dateChanged = function() {
            _reloadData();
        };

        $scope.selectJog = function(jog) {
            _deselectOtherJogs(jog);
            jog.selected = true;
        };

        $scope.toggleDatePicker = function($event, picker) {
            $event.preventDefault();
            $event.stopPropagation();
            picker.opened = !picker.opened;
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

            var fromDate = $scope.dtPickers.from.date;
            var toDate = $scope.dtPickers.to.date;

            if (!fromDate) {
                fromDate = moment().subtract(1, 'months');
            }

            if (!toDate) {
                toDate = moment();
            }

            jogDataFactory.getList(fromDate, toDate, $scope.pageNumber - 1, $scope.pageSize)
                .then(function(data)
                {
                    $scope.totalItems = data.TotalResults;
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
