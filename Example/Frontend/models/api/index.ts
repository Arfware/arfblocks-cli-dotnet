/*
!!! ATTENTION !!!
THIS FILE HAS BEEN AUTO-GENERATED
DO NOT EDIT THIS FILE
*/

// Imports 
import { api as axios } from 'mtsk-config'; 
import { ApiService } from '../services';

// Custom Types 
type Guid = string; 
type Double = number;

// Settings...

export namespace API {

	export namespace Enums {

		export enum DepartmentTypes {
			Sales = 0,
			Marketing = 1,
			Software = 2,
			HumanResources = 3,
		}

		export enum TodoTaskStatus {
			Pending = 0,
			Completed = 1,
			Rejected = 2,
		}

	}

	export namespace Users {

		export namespace All {
			export const RequestPath = AppConfig.ApiUrl + '/Users/All';
			export const Request = () => ApiService.call<IResponseModel[]>(axios.post(RequestPath,{}));

			export interface IResponseModel {
				id: Guid;
				email: string;
				firstName: string;
				lastName: string;
				displayName: string;
				lastLoginedAt?: Date;
				assignedDepartments: IAssignedDepartmentResponseModel[];
			}
			export interface IAssignedDepartmentResponseModel {
				id: Guid;
				name: string;
			}
		}

	}

	export namespace Departments {

		export namespace Detail {
			export const RequestPath = AppConfig.ApiUrl + '/Departments/Detail';
			export const Request = (data: IRequestModel) => ApiService.call<IResponseModel>(axios.post(RequestPath,{...data}));
			export interface IRequestModel {
				id: Guid;
			}
			export interface IResponseModel {
				id: Guid;
				name: string;
				employeeCount: number;
				departmentFactor: Double;
				departmentType: Enums.DepartmentTypes;
				taskStatus: Enums.TodoTaskStatus;
				aliases: string[];
			}
		}

		export namespace All {
			export const RequestPath = AppConfig.ApiUrl + '/Departments/All';
			export const Request = (data: IRequestModel) => ApiService.call<IResponseModel[]>(axios.post(RequestPath,{...data}));
			export interface IRequestModel {
				listAll: boolean;
				isDeleted: boolean;
			}
			export interface IResponseModel {
				id: Guid;
				name: string;
				employeeCount: number;
				departmentFactor: Double;
				departmentType: Enums.DepartmentTypes;
				taskStatus: Enums.TodoTaskStatus;
				aliases: string[];
			}
		}

	}

}