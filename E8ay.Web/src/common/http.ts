import * as fetch from 'isomorphic-fetch';

export interface IErrorModel {
  status: number,
  message?: string
}

const getResult = async (response:Response) => {
  let result = null;

  try {
    result = await response.json();
  }catch {
    result = null;
  }

  return result;
}

export const post = async (url:string, data:any) => {
  const response = await fetch(url, {
    body: JSON.stringify(data),
    headers: {
      'Content-Type': 'application/json'
    },
    method: "POST",
  });

  const result = await getResult(response);

  if (response.status !== 200) {
    const error:IErrorModel = {
      status: response.status,
      message: result !== null ? result.message : ''
    }
    throw error;
  }
  
  return result;
}

