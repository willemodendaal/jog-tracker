(function(){
    var authModule = angular.module('jogTracker.api.auth', [])
        .config(['$httpProvider', function($httpProvider) {
            $httpProvider.interceptors.push('loginInterceptor');
        }]);

    authModule.value('userInfo', { access_token: null }); //Will be updated by the loginInterceptor.
}());
