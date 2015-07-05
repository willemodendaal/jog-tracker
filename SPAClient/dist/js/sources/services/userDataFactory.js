(function() {

    angular.module('jogTracker.api')
        .factory('userDataFactory', userDataFactory);

    userDataFactory.$inject = [
        '$log',
        'apiUrls',
        '$http',
        '$q'
    ];

    function userDataFactory($log, apiUrls, $http, $q) {

        /*             *
         *   getList   *
         *             */
        var getList = function(pageIndex, pageSize) {
            var deferred = $q.defer();

            var payLoad = {
                pageIndex: pageIndex,
                pageSize: pageSize
            };

            $http.get(apiUrls.users(), {
                    params: payLoad
                })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                })
                .error(function (err) {
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        /*             *
         *   Get       *
         *             */
        var get = function(userId) {
            return $http.get(apiUrls.user(userId), {
                params: { userId: userId }
            });
        };

        /*             *
         *   Update    *
         *             */
        var update = function(userId, email, firstName, lastName) {
            var payLoad = {
                userId: userId,
                email: email,
                firstName: firstName,
                lastName: lastName
            };

            return $http.put(apiUrls.updateUser(userId), payLoad);
        };

        return {
            getList : getList,
            get : get,
            update: update
        };
    }
}());
