import { post } from '../common/http';


export const login = async (username: string, password: string) => {
  const data = {
    password,
    username
  }
 
  try {
    const result = await post('http://localhost:8100/api/auth/login', data);
    return result;
  }catch(error) {
    if (error.status === 401) {
      throw 'Invalid credential';
    }
  }
}