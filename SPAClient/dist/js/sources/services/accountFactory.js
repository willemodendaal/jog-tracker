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
                    var token = 'bearer ' + data.access_token;
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
        var registerAsAdmin = function(email, firstName, lastName, password, makeAdmin, makeUserManager) {
            var payload = {
                email: email,
                firstName: firstName,
                lastName: lastName,
                password: password,
                admin: makeAdmin,
                userManager: makeUserManager
            };

            return $http.post(apiUrls.registerAsAdmin, payload).success(_successLog);
        };

        /*                      *
         *   Request Reset Pwd  *
         *                      */
        var requestResetPwd = function(email) {
            var payload = {
                email: email
            };

            return $http.post(apiUrls.requestResetPwd, payload).success(_successLog);
        };

        /*              *
         *   Reset Pwd  *
         *              */
        var resetPwd = function(userId, newPassword, token) {
            var payload = {
                userId: userId,
                newPassword: newPassword,
                token: token
            };

            return $http.post(apiUrls.resetPwd, payload).success(_successLog);
        };

        /*                                *
         *   Get logged in user info      *
         *                                */
        var getUserInfo = function(jogId) {
            console.log('Fetching current user info...');
            return $http.get(apiUrls.userInfo);
        };

        /*             *
         *   Update    *
         *             */
        var update = function(firstName, lastName) {
            var payLoad = {
                firstName: firstName,
                lastName: lastName
            };

            return $http.put(apiUrls.updateAccount, payLoad);
        };

        return {
            register : register,
            registerAsAdmin: registerAsAdmin,
            login : login,
            getUserInfo: getUserInfo,
            update: update,
            requestResetPwd: requestResetPwd,
            resetPwd: resetPwd
        };
    }
}());

