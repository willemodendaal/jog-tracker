(function() {
    var apiModule = angular.module('jogTracker.api', []);

    var baseUrl = 'https://dev.jogTracker.api/api/v1';

    apiModule.constant('apiUrls', {
        register: baseUrl + '/account/register',
        registerAsAdmin: baseUrl + '/account/registerAsAdmin',
        requestResetPwd: baseUrl + '/account/requestResetPwd',
        resetPwd: baseUrl + '/account/resetPwd'
    });

}());

