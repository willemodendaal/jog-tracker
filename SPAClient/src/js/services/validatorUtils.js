(function() {

    angular.module('jogTracker.validation')
        .factory('validatorUtils', validatorUtils);

    validatorUtils.$inject = [];

    function validatorUtils() {

        //Return collection of strings of errors to show on front-end.
        //  (get errors from server response)
        var getValidationErrors = function(httpError) {
            if (!httpError.data || !httpError.data.ModelState) {

                if (httpError.error_description) { //returned by Owin/OAuth
                    return [httpError.error_description];
                }

                return ['Request contained invalid data.'];
            }

            var messages = [];
            for (var field in httpError.data.ModelState) {
                if (httpError.data.ModelState.hasOwnProperty(field)) {

                    var array = httpError.data.ModelState[field];
                    for(var i = 0; i < array.length; i++) {
                        messages.push(array[i]);
                    }
                }
            }

            return messages;
        };


        return {
            getValidationErrors : getValidationErrors
        };
    }
}());
