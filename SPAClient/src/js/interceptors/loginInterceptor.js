//Injects access token into all outgoing requests, and
//  intercepts HTTP 401's and 403's, and broadcasts an event that can be
//  handled, by the UI (forcing the user to login).
(function(){
    angular.module('jogTracker.api.auth')
        .service('loginInterceptor', loginInterceptor);

    loginInterceptor.$inject = ['$rootScope', 'authToken'];

    function loginInterceptor($rootScope, authToken) {
        var service = this;

        service.request = function(config) {

            if (authToken) {
                config.headers.authorization = access_token;
            }
            return config;
        };

        service.responseError = function(response) {
            if (response.status === 401 || response.status === 403) {
                //Broadcast so that app can nav to the login page.
                $rootScope.$broadcast('must-login');
            }
            return response;
        };
    }
}());
