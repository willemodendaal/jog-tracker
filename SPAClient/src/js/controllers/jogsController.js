(function (moment) {

    angular
        .module('jogTracker')
        .controller('jogsController', jogsController);

    jogsController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory'];

    function jogsController($scope, $log, jogDataFactory) {

        $scope.jogs = [];
        $scope.fromDate = moment().subtract(7, 'days'); //Default filter from last week.
        $scope.toDate = moment();
        $scope.pageIndex = 0;
        $scope.pageSize = 30;

        $scope.noData = function() {
            return $scope.jogs.length == 0;
        };

        var _reloadData = function() {
            jogDataFactory.getList($scope.fromDate, $scope.toDate, $scope.pageIndex, $scope.pageSize)
                .then(function(data)
                {
                    $scope.jogs = data.Items;
                })
                .catch(function(err) {
                    //Todo: show toast.
                    alert('Unable to load jogs data. Message: ' + err);
                });
        };

        _reloadData();
        $log.info('Jogs controller loaded.');
    }

}(moment));
