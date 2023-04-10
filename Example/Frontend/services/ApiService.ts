import { AxiosPromise, AxiosRequestConfig } from 'axios'
import { ErrorService } from './ErrorService'

export class ApiService {
	call = async <T>(request: AxiosPromise, cb: () => void = () => { }): Promise<T> => {
		try {
			const response = await request
			if (response.data.hasError) {
				throw {
					isControlled: true,
					message: ErrorService.GetErrorMessage(response.data.error.message)
				}
			}
			return response.data.payload as T
		} catch (err: any) {
			if (err.isControlled) {
				throw new Error(err.message)
			} else {
				throw new Error(ErrorService.defaultErrorMessage)
			}
		} finally {
			cb()
		}
	}

	static call = async <T>(request: AxiosPromise, cb: () => void = () => { }): Promise<T> => {
		try {
			const response = await request
			if (response.data.hasError) {
				throw {
					isControlled: true,
					message: ErrorService.GetErrorMessage(response.data.error.message)
				}
			}
			return response.data.payload as T
		} catch (err: any) {
			if (err.isControlled) {
				throw new Error(err.message)
			} else {
				throw new Error(ErrorService.defaultErrorMessage)
			}
		} finally {
			cb()
		}
	}
}
