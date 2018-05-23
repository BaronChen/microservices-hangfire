import { post } from '../common/http/http-client';
import { IJwt } from './models';
import { userServiceUrl } from '../common/config';

export const login = async (username: string, password: string):Promise<IJwt> => {
  const data = {
    password,
    username
  }
 
  try {
    const result = await post<IJwt>(`${userServiceUrl}/api/auth/login`, data);
    
    if (result === null) {
      throw 'Unexpected Error!';
    }
      
    return result;
  }catch(error) {
    if (error.status === 401) {
      throw 'Invalid credential';
    }

    throw JSON.stringify(error);
  }
}