(function() {

    angular.module('jogTracker')
        .factory('notificationUtils', notificationUtils);

    notificationUtils.$inject = ['$log', 'toastr'];

    function notificationUtils($log, toastr) {
        $log.info('notificationUtils loaded.');

        var showErrorToast = function(err, title) {
            //Show error in a toast.
            if (err.data.Message) {
                toastr.error(err.data.Message, title);
            }
            else {
                toastr.error(err.statusText, title);
            }
        };

        return {
            showErrorToast : showErrorToast
        };
    }
}());
