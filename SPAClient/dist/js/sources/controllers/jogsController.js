(function (moment, _) {

    angular
        .module('jogTracker')
        .controller('jogsController', jogsController);

    jogsController.$inject = [
        '$scope',
        'jogDataFactory',
        'notificationUtils',
        '$rootScope'];

    function jogsController($scope, jogDataFactory, notificationUtils, $rootScope) {

        $scope.jogs = [];
        $scope.pageNumber = 1;
        $scope.pageSize = 4;
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
            _selectOnlyJog(jog.id);
            $rootScope.$broadcast('editJog', jog);
        };

        $scope.toggleDatePicker = function($event, picker) {
            $event.preventDefault();
            $event.stopPropagation();
            picker.opened = !picker.opened;
        };

        var _selectOnlyJog = function(jogId) {
            _.each(
                $scope.jogs,
                function(j) {
                    j.selected = j.id == jogId;
                });
        };

        var _reloadData = function() {

            var fromDate = $scope.dtPickers.from.date;
            var toDate = $scope.dtPickers.to.date;

            if (!fromDate) {
                fromDate = moment().startOf('day').subtract(1, 'months');
            }

            if (!toDate) {
                toDate = moment().endOf('day');
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
    }

}(moment, _));
