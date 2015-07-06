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

    var updateUserUrl = function(userId) {
        return baseUrl + '/user/' + userId + '/update';
    };

    var userUrl = function(userId) {
        if (userId) {
            return baseUrl + '/user/' + userId;
        }

        return baseUrl + '/user';
    };

    var jogsForWeek = function(weekDate) {
            return baseUrl + '/jog/week/' + weekDate;
    };

    apiModule.constant('apiUrls', {
        register: baseUrl + '/account/register',
        registerAsAdmin: baseUrl + '/account/registerAsAdmin',
        requestResetPwd: baseUrl + '/account/requestResetPwd',
        resetPwd: baseUrl + '/account/resetPwd',
        updateAccount: baseUrl + '/account',

        login: host + '/token',
        userInfo: baseUrl + '/account',
        users: userUrl,
        user: userUrl,
        updateUser: updateUserUrl,

        jogs: jogUrl,
        jogsForWeek: jogsForWeek,
        newJog: baseUrl + '/jog/new',
        updateJog: updateJogUrl
    });

}());

