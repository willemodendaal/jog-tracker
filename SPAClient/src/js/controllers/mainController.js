angular
    .module('jogTracker')
    .controller('mainController', mainController);

mainController.$inject = ['$log', 'jogDataFactory', 'accountFactory'];

function mainController($log, jogDataFactory, accountFactory) {
    $log.info('Main controller loaded.');
}
