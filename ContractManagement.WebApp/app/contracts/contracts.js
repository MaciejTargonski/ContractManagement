(function () {
    'use strict';

    angular.module('app.controllers')
    .controller("ContractsController", [
        '$scope',
        'uiGridConstants',
        '_',
        '$state',
        '$window',
        '$rootScope',
        '$exceptionHandler',
        '$http',
        '$resource',
        function (
            $scope,
            uiGridConstants,
            _,
            $state,
            $window,
            $rootScope,
            $exceptionHandler,
            $http,
            $resource) {



            $scope.$root.title = 'Contracts';


            $scope.load = function () {

                getPage();

            };

            var paginationOptions = {
                pageNumber: 1,
                pageSize: 10,
                sortColName: null,
                sortDir: null
            };

            var setDataToGrid = function (gridData) {
                $scope.gridOptions.data = gridData.value;
            };

            $scope.optionalFilters = {
                programmers5Years: false,
                salary5000: false,
            };

            var getPage = function () {

                
                var url = 'http://localhost:24454/odata/Contracts?$inlinecount=allpages';


                url = url + '&$top=' + paginationOptions.pageSize + '&$skip=' + (paginationOptions.pageNumber - 1) * paginationOptions.pageSize;
                
                if (paginationOptions.sortColName !== null)
                {
                    url = url + '&$orderby=' + paginationOptions.sortColName + ' ' + paginationOptions.sortDir;
                }

                if ($scope.optionalFilters.programmers5Years && $scope.optionalFilters.salary5000)
                {
                    url = url + '&$filter=ExperienceInYears ge 5 and Salary gt 5000';
                } else if ($scope.optionalFilters.salary5000) {
                    url = url + '&$filter=Salary gt 5000';
                } else if ($scope.optionalFilters.programmers5Years){
                    url = url + '&$filter=ExperienceInYears ge 5';
                }
                

                $http.get(url)
                    .success(function (data) {
                        $scope.gridOptions.totalItems = data["odata.count"];
                        
                        $scope.gridOptions.data = data.value;
                    });

                
            }

            $scope.refreshGrid = getPage;

            $scope.gridOptions = {
                enableSorting: true,
                enableRowSelection: true,
                enableRowHeaderSelection: false,
                enableColumnMenus: false,
                enableFiltering: true,

                paginationPageSizes: [10, 25, 50, 75],
                paginationPageSize: 10,
                useExternalPagination: true,
                useExternalSorting: true,

                columnDefs: [
                    {
                        field: 'Name',
                        filter: { condition: uiGridConstants.filter.CONTAINS },
                        headerCellClass: 'test-automation-header-delivery-name text-left',
                        cellClass: 'test-automation-delivery-name text-left',
                        enableSorting: true
                    },
                    {
                        field: 'ContractType',
                        //cellTemplate: "<div class='ui-grid-cell-contents'>{{row.entity.dateSubmitted }}</div>",
                        sort: { direction: uiGridConstants.DESC, priority: 1 },
                        enableFiltering: false,
                        headerCellClass: 'test-automation-header-delivery-date-submitted text-center',
                        cellClass: 'test-automation-delivery-date-submitted text-center'
                    },
                    {
                        field: 'ExperienceInYears',
                        //cellTemplate: "<div class='ui-grid-cell-contents'>{{row.entity.dateSubmitted }}</div>",
                        sort: { direction: uiGridConstants.DESC, priority: 1 },
                        enableFiltering: false,
                        headerCellClass: 'test-automation-header-delivery-date-submitted text-center',
                        cellClass: 'test-automation-delivery-date-submitted text-center',
                        cellFilter: 'number'
                    },
                    {
                        field: 'Salary',
                        //cellTemplate: "<div class='ui-grid-cell-contents'>{{row.entity.dateSubmitted }}</div>",
                        sort: { direction: uiGridConstants.DESC, priority: 1 },
                        enableFiltering: false,
                        headerCellClass: 'test-automation-header-delivery-date-submitted text-center',
                        cellClass: 'test-automation-delivery-date-submitted text-center',
                        cellFilter: 'number'
                    }
                    
                ],

                showGridFooter: true,
                showColumnFooter: true,
                enableFooterTotalSelected: false,
                multiSelect: false,
                noUnselect: true,
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;

                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (sortColumns.length === 0) {
                            paginationOptions.sortColName = null;
                            paginationOptions.sortDir = null;
                        } else {
                            paginationOptions.sortColName = sortColumns[0].name;
                            paginationOptions.sortDir = sortColumns[0].sort.direction;
                        }
                        getPage();
                    });
                    $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                        paginationOptions.pageNumber = newPage;
                        paginationOptions.pageSize = pageSize;
                        getPage();
                    });

                   
                },
                enableGridMenu: true

            };

            $scope.gridOptions.enableHorizontalScrollbar = uiGridConstants.scrollbars.NEVER;

            function saveGridState() {
                if ($scope.gridApi && $scope.gridApi.saveState) {
                    $window.sessionStorage["gridState"] = angular.toJson($scope.gridApi.saveState.save());
                }
                $scope.isGridStateRestored = false;
            }

            $scope.isGridStateRestored = false;
            function restoreGridState() {
                if (!$scope.isGridStateRestored && $window.sessionStorage.gridState && $scope.gridApi) {
                    $scope.gridApi.saveState.restore($scope, angular.fromJson($window.sessionStorage.gridState));
                    $scope.isGridStateRestored = true;
                }
            }


            $scope.$on('$destroy', saveGridState);

            angular.element($window).bind("beforeunload", saveGridState);

            var gridRefresh = function () {
                if ($scope.gridApi) {
                    $scope.gridApi.core.refresh();
                }
            };


            $scope.load();

            $scope.addNewContract = function () {

                $state.go('newContract');

            };

            $scope.getRecommendedSalary = function () {


                var url = 'http://localhost:24454/api/recommendedsalary/' +
                         $scope.newContract.contractType.name +
                         '/' + $scope.newContract.experienceInYears;

                $http.get(url)
                    .success(function (data) {
                        $scope.newContract.salary = data.netSalary;
                    });

            };

            $scope.addNewContract = function () {
                $scope.getRecommendedSalary();

                var url = 'http://localhost:24454/odata/Contracts' 
                var data =

                   {
                       "Name": $scope.newContract.name, 
                       "ContractType": $scope.newContract.contractType.name,
                       "ExperienceInYears": $scope.newContract.experienceInYears,
                       "Salary": $scope.newContract.salary
                   };
            
                var config = {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }

                $http.post(url, data, config)
                    .success(function (data) {
                        $state.go('contracts');
                    })
                .error(function (data, status, header, config) {
                    if (data["odata.error"].innererror === undefined) {
                        $scope.errorMessage = data["odata.error"].message.value
                    }
                    else {
                        $scope.errorMessage = data["odata.error"].innererror.message;
                    }
                    $state.go('newContract');
                });

            };

            $scope.contractTypes = [
            { name: 'Programmer', id: 1 },
            { name: 'Tester', id: 2 }
            ]

            $scope.newContract = {
                name: null,
                salary: null,
                experienceInYears: 1,
                contractType: $scope.contractTypes[0]
            };

            $scope.errorMessage = null;


            $scope.isErrorMessage = function () {
                if ($scope.errorMessage !== null)
                    return true;
                else
                    return false;
            };
            

        }]);

})();
