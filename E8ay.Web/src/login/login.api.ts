import { post } from '../common/http';
import { IJwt } from './models';


export const login = async (username: string, password: string):Promise<IJwt> => {
  const data = {
    password,
    username
  }
 
  try {
    const result = await post<IJwt>('http://localhost:8100/api/auth/login', data);
    
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