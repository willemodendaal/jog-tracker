(function () {

    angular
        .module('jogTracker')
        .controller('jogsController', jogsController);

    jogsController.$inject = [
        '$scope',
        '$log',
        'jogDataFactory'];

    function jogsController($scope, $log, jogDataFactory) {


        $log.info('Jogs controller loaded.');
    }

}());
