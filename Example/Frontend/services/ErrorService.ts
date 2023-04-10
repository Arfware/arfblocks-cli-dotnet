const t = (str: string) => {
	return {
		"USER_NOT_FOUND": "TESTTESTTEST"
	}[str]
}

export class ErrorService {
	static defaultErrorMessage = "An Error Occured"
	static GetErrorMessage = (code: string): string => {
		const errorMessage = t(`errors.${code}`)
		console.log('errorMessage:', errorMessage)
		return errorMessage || ErrorService.defaultErrorMessage
	}
}

