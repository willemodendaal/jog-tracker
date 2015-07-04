(function() {

    angular.module('jogTracker.api')
        .factory('accountFactory', accountFactory);

    accountFactory.$inject = ['$log', '$http', '$q', 'apiUrls'];

    function accountFactory($log, $http, $q, apiUrls) {

        var _successLog = function (data, status, headers, config) {
            $log.info('Successful response from Api. Data: ', data, ', status: ', status, ', headers: ', headers, 'config: ', config);
        };

        /*          *
         *  Login   *
         *          */
        var login = function(email, password) {
            //Logs in, and resolves promise with Token.
            var payload = {
                userName: email,
                password: password,
                grant_type: 'password'
            };

            var deferred = $q.defer();

            //Need to encode this way to make ASP.NET WebAPI happy.
            var stringPayload = "userName=" + encodeURIComponent(email) +
                            "&password=" + encodeURIComponent(password) +
                            "&grant_type=password";

            $http({
                url: apiUrls.login,
                data: stringPayload,
                method: 'post'
            })
                .success(function (data, status, headers, config) {
                    $log.info('Login ok.');
                    var token = data.access_token;
                    deferred.resolve(token);
                })
                .error(function (err) {
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        /*             *
         *   Register  *
         *             */
        var register = function(email, firstName, lastName, password) {
            var payload = {
                email: email,
                firstName: firstName,
                lastName: lastName,
                password: password
            };

            return $http.post(apiUrls.register, payload).success(_successLog);
        };


        /*                    *
         *   RegisterAsAdmin  *
         *                    */
        //Protected by role-based security on the server.
        var registerAsAdmin = function(email, firstName, lastName, password, makeAdmin) {
            var payload = {
                email: email,
                firstName: firstName,
                lastName: lastName,
                password: password,
                admin: makeAdmin
            };

            return $http.post(apiUrls.registerAsAdmin, payload).success(_successLog);
        };


        return {
            register : register,
            registerAsAdmin: registerAsAdmin,
            login : login
        };
    }
}());

