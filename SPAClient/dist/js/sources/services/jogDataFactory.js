angular.module('jogTracker.api')
    .factory('jogDataFactory', jogDataFactory);

jogDataFactory.$inject = ['$log'];

function jogDataFactory($log) {
    $log.info('jogDataFactory loaded.');

    return 100;
}
