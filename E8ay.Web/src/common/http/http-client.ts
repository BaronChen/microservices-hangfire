import * as fetch from 'isomorphic-fetch';
import { storeRegistry } from '../../store-registry';
import { getAuthToken } from '../../login/reducers';
import { IStandarResponse } from './standard-response';
import { push } from 'react-router-redux';

export interface IErrorModel {
  status: number,
  errors: string[]
}

export interface IHttpError {
  error: string;
}

const getResult = async <T>(response: Response): Promise<IStandarResponse<T> | null> => {
  let result: IStandarResponse<T>;

  try {
    result = await response.json();
  } catch {
    return null;
  }

  return result;
}

const getToken = (): string => {
  return getAuthToken(storeRegistry.getStore().getState());
}

const configAuthHeader = (headers: Headers): boolean => {
  const token = getToken();
  if (!token || token === '') {
    storeRegistry.getStore().dispatch(push('/login'));
    return false;
  }
  headers.append('Authorization', `Bearer ${token}`);
  return true;
}

const handleResponse = async <T>(response: Response): Promise<T | null> => {
  const result = await getResult<T>(response);

  if (response.status !== 200) {
    const error: IErrorModel = {
      status: response.status,
      errors: result !== null && result.isError ? result.errors : []
    }
    throw error;
  }

  return result ? result.data : null;
}

export const post = async <T>(url: string, data: any, requireAuth: boolean = false): Promise<T | null> => {

  const option: RequestInit = {
    body: JSON.stringify(data),
    method: "POST"
  };

  option.headers = new Headers();
  option.headers.append('Content-Type', 'application/json');

  if (requireAuth) {
    if (!configAuthHeader(option.headers)){
      return null;
    }
  }

  const response = await fetch(url, option);
  return handleResponse<T>(response);
}

export const get = async <T>(url: string, requireAuth: boolean = false): Promise<T | null> => {

  const option: RequestInit = {
    method: 'GET'
  };

  option.headers = new Headers();
  option.headers.append('Accept', 'application/json');

  if (requireAuth) {
    if (!configAuthHeader(option.headers)){
      return null;
    }
  }

  const response = await fetch(url, option);
  return handleResponse<T>(response);
}
