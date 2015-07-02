(function() {
    var apiModule = angular.module('jogTracker.api', ['jogTracker.api.auth']);

    var host = 'https://dev.jogTracker.api';
    var baseUrl = host + '/api/v1';

    apiModule.constant('apiUrls', {
        register: baseUrl + '/account/register',
        registerAsAdmin: baseUrl + '/account/registerAsAdmin',
        requestResetPwd: baseUrl + '/account/requestResetPwd',
        resetPwd: baseUrl + '/account/resetPwd',
        login: host + '/token'
    });

}());

