export interface IStandarResponse<T> {
  errors: string[],
  isError: boolean,
  messages: string[],
  warnings: string[],
  data: T
}