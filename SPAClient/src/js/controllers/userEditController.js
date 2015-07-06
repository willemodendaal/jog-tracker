(function () {

    angular
        .module('jogTracker')
        .controller('userEditController', userEditController);

    userEditController.$inject = [
        '$scope',
        'userDataFactory',
        'accountFactory',
        'notificationUtils',
        'validatorUtils'];

    function userEditController($scope, userDataFactory, accountFactory, notificationUtils, validatorUtils) {

        $scope.panelOpen = false; //Hide 'add' panel by default.
        $scope.editMode = false;

        $scope.$on('editUser', function (event, user) {
            _editUser(user);
        });

        var _reset = function () {
            $scope.editMode = false;
            $scope.userId = null;
            $scope.title = '* New User';
            $scope.buttonText = 'Create New';
            $scope.friendlyErrors = []; //validation errors.
            $scope.disableButton = false;
            $scope.opened = false;
            $scope.email = '';
            $scope.firstName = '';
            $scope.lastName = '';
            $scope.password = '';
            $scope.userManager = false;
        };

        var _editUser = function (user) {
            _reset();
            $scope.editMode = true;
            $scope.userId = user.id;
            $scope.panelOpen = true;
            $scope.title = 'Edit User';
            $scope.buttonText = 'Update';
            $scope.email = user.email;
            $scope.firstName = user.firstName;
            $scope.lastName = user.lastName;
            $scope.password = 'NotUsed'; //We don't do anything with password in 'edit' mode.
            $scope.userManager = user.isUserManager;
        };

        var _setDisabled = function (disabled) {
            if (disabled) {
                $scope.disableButton = true;
                $scope.buttonText = "Create new...";
            }
            else {
                $scope.disableButton = false;
                $scope.buttonText = "Create new";
            }
        };

        $scope.startAddNew = function () {
            _reset();
            $scope.panelOpen = true;
        };

        $scope.closePanel = function () {
            $scope.panelOpen = false;
            _reset();
        };

        function _createNewUser() {
            accountFactory.registerAsAdmin($scope.email, $scope.firstName, $scope.lastName, $scope.password, false, $scope.userManager)
                .then(function () {
                    notificationUtils.showSuccess('User created.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
                    $scope.closePanel();
                })
                .catch(function (err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error creating user');
                    } else {
                        //Show error on the page (could be something like 'user name taken already'.
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        }

        function _updateUser(userId) {
            userDataFactory.update(userId, $scope.email, $scope.firstName, $scope.lastName)
                .then(function () {
                    notificationUtils.showSuccess('User updated.', 'Success');
                    $scope.$emit('refresh'); //Indicate that a refresh is required.
                    $scope.closePanel();
                })
                .catch(function (err) {
                    _setDisabled(false);

                    if (err.status == 500) {
                        notificationUtils.showErrorToast(err, 'Error updating user');
                    } else {
                        $scope.friendlyErrors = validatorUtils.getValidationErrors(err);
                    }
                });
        }

        $scope.save = function (validForm) {

            if (!validForm) {
                return;
            }

            _setDisabled(true);
            $scope.friendlyErrors = [];

            if ($scope.userId == null) {
                _createNewUser();
            }
            else {
                _updateUser($scope.userId);
            }
        };

        _reset();
    }

}());
