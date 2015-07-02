(function() {

    angular.module('jogTracker.api')
        .factory('accountFactory', accountFactory);

    accountFactory.$inject = ['$log', '$http', 'apiUrls'];

    function accountFactory($log, $http, apiUrls) {

        var register = function(email, firstName, lastName, password) {

            var payload = {
                email: email,
                firstName: firstName,
                lastName: lastName,
                password: password
            };

            return $http.post(apiUrls.register, payload).
                success(function (data, status, headers, config) {
                    $log.info('Register success.');
                }).
                error(function (data, status, headers, config) {
                    $log.info('register failure.');
                });
        };

        return {
            register : register
        };
    }
}());

