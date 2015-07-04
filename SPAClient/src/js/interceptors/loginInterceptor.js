//Injects access token into all outgoing requests, and
//  intercepts HTTP 401's and 403's, and broadcasts an event that can be
//  handled, by the UI (forcing the user to login).
(function(){
    angular.module('jogTracker.api.auth')
        .service('loginInterceptor', loginInterceptor);

    loginInterceptor.$inject = ['$rootScope', '$q', 'userInfo'];

    function loginInterceptor($rootScope, $q, userInfo) {
        var service = this;

        service.request = function(config) {

            if (userInfo.access_token) {
                config.headers.authorization = userInfo.access_token;
            }
            return config;
        };

        service.responseError = function(response) {
            if (response.status === 401 || response.status === 403) {
                //Ensure token is not set. If it was, there is something wrong with it.
                userInfo.access_token = null;

                //Broadcast so that app can nav to the login page.
                $rootScope.$broadcast('must-login');
            }
            return $q.reject(response); //must reject explicitly.
        };
    }
}());
