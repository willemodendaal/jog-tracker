(function() {

    angular.module('jogTracker.api')
        .factory('jogDataFactory', jogDataFactory);

    jogDataFactory.$inject = [
        '$log',
        'apiUrls',
        '$http'
    ];

    function jogDataFactory($log, apiUrls, $http) {
        $log.info('jogDataFactory loaded.');

        /*             *
         *   getList   *
         *             */
        var getList = function(fromDate, toDate, pageIndex, pageSize) {
            var payLoad = {
                fromDate: fromDate,
                toDate: toDate,
                pageIndex: pageIndex,
                pageSize: pageSize
            };

            return $http.get(apiUrls.jogs(), {
                params: payLoad
            });
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
                distance: distance,
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
                distance: distance,
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
