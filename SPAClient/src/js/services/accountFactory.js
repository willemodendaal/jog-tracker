(function() {

    angular.module('jogTracker.api')
        .factory('accountFactory', accountFactory);

    accountFactory.$inject = ['$log', 'apiUrls'];

    function accountFactory($log, apiUrls) {

        var register = function(email, firstName, lastName, password) {

            var payload = {
                email: email,
                firstName: firstName,
                lastName: lastName,
                password: password
            };

            $http.post(apiUrls.register, payload).
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

