(function() {

    angular.module('jogTracker.api')
        .factory('jogDataFactory', jogDataFactory);

    jogDataFactory.$inject = [
        '$log',
        'apiUrls',
        '$http',
        '$q'
    ];

    var dateFormat = 'YYYY-MM-DDT00:00:00';

    function jogDataFactory($log, apiUrls, $http, $q) {
        $log.info('jogDataFactory loaded.');

        /*             *
         *   getList   *
         *             */
        var getList = function(fromDate, toDate, pageIndex, pageSize) {
            var deferred = $q.defer();

            var from;
            if (fromDate.format) {
                //already a moment date.
                from = fromDate.format(dateFormat);
            }
            else {
                from = moment(fromDate).format(dateFormat);
            }

            var to;
            if (toDate.format) {
                //already a moment date.
                //Add 1 day, we want the whole of the end date.
                to = toDate.add(1,'days').format(dateFormat);
            }
            else {
                //Add 1 day, we want the whole of the end date.
                to = moment(toDate).add(1,'days').format(dateFormat);
            }

            //Swap the dates if they are the wrong way around (lets be nice to the user).
            if (moment(fromDate) > moment(toDate)) {
                var temp = to;
                to = from;
                from = temp;
            }

            var payLoad = {
                fromDate: from,
                toDate: to,
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


            return deferred.promise;
        };


        /*                      *
         *   getListForWeek     *
         *                      */
        var getListForWeek = function(weekDate) {
            var deferred = $q.defer();

            var dateToSendToService;
            if (weekDate.format) {
                //already a moment date.
                dateToSendToService = weekDate.format(dateFormat);
            }
            else {
                dateToSendToService = moment(weekDate).format(dateFormat);
            }

            var payLoad = {
                week: dateToSendToService
            };

            $http.get(apiUrls.jogsForWeek(), {
                params: payLoad
            })
                .success(function (data) {
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

            var dateString = moment(dateTime).format('DD MMMM YYYY');

            var payLoad = {
                dateTime: dateString,
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
            getListForWeek: getListForWeek,
            get : get,
            del: del,
            create: create,
            update: update
        };
    }
}());
