(function() {

    angular.module('jogTracker')
        .factory('notificationUtils', notificationUtils);

    notificationUtils.$inject = ['toastr'];

    function notificationUtils(toastr) {

        var showErrorToast = function(err, title) {
            //Show error in a toast.
            if (err.status == 401) {
                toastr.info('Please sign in with an account that has access to this resource.', 'Login Required');
            }
            else if (err.data && err.data.Message) {
                toastr.error(err.data.Message, title);
            }
            else {
                toastr.error(err.statusText, title);
            }
        };

        var showSuccess = function(message, title) {
            toastr.success(message, title);
        };

        return {
            showErrorToast : showErrorToast,
            showSuccess: showSuccess
        };
    }
}());
