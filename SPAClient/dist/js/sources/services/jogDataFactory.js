(function() {

    angular.module('jogTracker.api')
        .factory('jogDataFactory', jogDataFactory);

    jogDataFactory.$inject = [
        '$log',
        'apiUrls',
        '$http',
        '$q'
    ];

    var dateFormat = 'YYYY-MM-DDTHH:mm:ss';

    function jogDataFactory($log, apiUrls, $http, $q) {
        $log.info('jogDataFactory loaded.');

        /*             *
         *   getList   *
         *             */
        var getList = function(fromDate, toDate, pageIndex, pageSize) {
            var deferred = $q.defer();

            var payLoad = {
                fromDate: fromDate.format(dateFormat),
                toDate: toDate.format(dateFormat),
                pageIndex: pageIndex,
                pageSize: pageSize
            };

            $http.get(apiUrls.jogs(), {
                    params: payLoad
                })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                })
                .error(function (err) {
                    deferred.reject(err);
                });
            ;

            return deferred.promise;
        };

        /*             *
         *   Get       *
         *             */
        var get = function(jogId) {
            return $http.get(apiUrls.jogs(jogId), {
                params: { jogId: jogId }
            });
        };

        /*             *
         *   Del       *
         *             */
        var del = function(jogId) {
            return $http.delete(apiUrls.jogs(jogId), {
                params: { jogId: jogId }
            });
        };

        /*             *
         *   Create    *
         *             */
        var create = function(dateTime, distance, duration) {
            var payLoad = {
                dateTime: dateTime,
                distanceKm: distance,
                duration: duration
            };

            return $http.post(apiUrls.newJog, payLoad);
        };

        /*             *
         *   Update    *
         *             */
        var update = function(jogId, dateTime, distance, duration) {
            var payLoad = {
                dateTime: dateTime,
                distanceKm: distance,
                duration: duration
            };

            return $http.put(apiUrls.updateJog(jogId), payLoad);
        };

        return {
            getList : getList,
            get : get,
            del: del,
            create: create,
            update: update
        };
    }
}());
