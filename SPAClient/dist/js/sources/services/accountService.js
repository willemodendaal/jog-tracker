angular.module('jogTracker.api')
    .factory('accountFactory', accountFactory);

accountFactory.$inject = ['$log'];

function accountFactory($log) {
    $log.info('accountFactory loaded.');

    return
    {};
}
