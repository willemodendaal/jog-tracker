(function() {
    var apiModule = angular.module('jogTracker.api', ['jogTracker.api.auth']);

    var host = 'https://dev.jogTracker.api';
    var baseUrl = host + '/api/v1';

    var jogUrl = function(jogId) {
        if (jogId) {
            return baseUrl + '/jog/' + jogId;
        }

        return baseUrl + '/jog';
    };

    var updateJogUrl = function(jogId) {
        return baseUrl + '/jog/' + jogId + '/update';
    };

    apiModule.constant('apiUrls', {
        register: baseUrl + '/account/register',
        registerAsAdmin: baseUrl + '/account/registerAsAdmin',
        requestResetPwd: baseUrl + '/account/requestResetPwd',
        resetPwd: baseUrl + '/account/resetPwd',

        login: host + '/token',
        userInfo: baseUrl + '/account',

        jogs: jogUrl,
        newJog: baseUrl + '/jog/new',
        updateJog: updateJogUrl
    });

}());

