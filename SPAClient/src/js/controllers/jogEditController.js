(function (moment) {

    angular
        .module('jogTracker')
        .controller('jogEditController', jogEditController);

    jogEditController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory'];

    function jogEditController($scope, $log, jogDataFactory) {

        $scope.date = moment();
        $scope.durationMinutes = 1;
        $scope.distanceKm = 0;

        $log.info('JogEdit controller loaded.');
    }

}(moment));
