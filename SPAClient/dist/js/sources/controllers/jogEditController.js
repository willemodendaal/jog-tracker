(function (moment) {

    angular
        .module('jogTracker')
        .controller('jogEditController', jogEditController);

    jogEditController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory'];

    function jogEditController($scope, $log, jogDataFactory) {

        $scope.title = 'New Jog';
        $scope.buttonText = 'Create New';

        $scope.date = moment();
        $scope.durationMinutes = 1;
        $scope.distanceKm = 0;

        $scope.save = function() {
            alert('Saved');
        };

        $log.info('JogEdit controller loaded.');
    }

}(moment));
